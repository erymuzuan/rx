using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Bespoke.Sph.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

namespace Bespoke.Sph.Mangements.ViewModels
{
    [Export]
    public class MigrationScriptViewModel : ViewModelBase, IView
    {
        public DispatcherObject View { get; set; }

        public ObjectCollection<string> PlanCollection { get; } = new ObjectCollection<string>();
        public ObjectCollection<MemberChange> ChangeCollection { get; } = new ObjectCollection<MemberChange>();

        private string m_script;
        private MemberChange m_selected;
        private string m_selectedPlan;


        public string SelectedPlan
        {
            get => m_selectedPlan;
            set
            {
                m_selectedPlan = value;
                RaisePropertyChanged("SelectedPlan");
            }
        }


        public MemberChange Selected
        {
            get => m_selected;
            set
            {
                m_selected = value;
                RaisePropertyChanged("Selected");
            }
        }

        public string Script
        {
            get => m_script;
            set
            {
                m_script = value;
                RaisePropertyChanged("Script");
            }
        }
        public RelayCommand SaveCommand { get; set; }

        public MigrationScriptViewModel()
        {
            this.SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            //
            if (string.IsNullOrWhiteSpace(this.Script)) return;

            this.Selected.MigrationScript = this.Script;
            this.Selected.MigrationStrategy = MemberMigrationStrategies.Script;

            var json = File.ReadAllText($"{ConfigurationManager.SphSourceDirectory}\\MigrationPlan\\{SelectedPlan}.json");
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var plan = JsonConvert.DeserializeObject<MigrationPlan>(json, setting);

            var change = plan.ChangeCollection.Single(x => x.WebId == this.Selected.WebId);
            change.MigrationScript = this.Script;
            change.MigrationStrategy = MemberMigrationStrategies.Script;

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\MigrationPlan\\{SelectedPlan}.json", plan.ToJson());
        }

        public Task Load()
        {
            var plans = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\MigrationPlan", "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
            this.PlanCollection.ClearAndAddRange(plans);
            return Task.FromResult(0);
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
            if (propertyName == nameof(SelectedPlan))
            {
                this.ChangeCollection.Clear();
                var json = File.ReadAllText($"{ConfigurationManager.SphSourceDirectory}\\MigrationPlan\\{SelectedPlan}.json");
                var setting = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                var plan = JsonConvert.DeserializeObject<MigrationPlan>(json, setting);

                this.ChangeCollection.AddRange(plan.ChangeCollection);

            }

            if (propertyName == nameof(Selected) && null != this.Selected)
            {
                var script = this.Selected.MigrationScript;
                if (string.IsNullOrWhiteSpace(script))
                    this.Script = this.SuggestScript();
                else
                    this.Script = this.Selected.MigrationScript;
            }
        }

        private string SuggestScript()
        {
            var id = this.Selected.WebId;
            var edName = this.SelectedPlan.Split(new string[] {"-"}, StringSplitOptions.RemoveEmptyEntries).First();
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == edName);
            var members = new List<Member>(ed.MemberCollection);

            void AddMembers(Member[] list)
            {
                foreach (var mbr in list.Where(x => x.MemberCollection.Count > 0))
                {
                    members.AddRange(mbr.MemberCollection);
                    AddMembers(mbr.MemberCollection.ToArray());
                }
            }

            AddMembers(members.ToArray());
            var member = members.FirstOrDefault(x => x.WebId == id);


            var type = member?.GetMemberTypeName()?? "/* TODO : specify your return type here*/";
            return $@"
// NOTE : the return type must be the type expected by {member?.Name}
public {type} GetValue(string source)
{{
    var json = Newtonsoft.Json.Linq.JObject.Parse(source);
    /* 
    NOTE : sample how to extract value from json
    var myval = json.SelectToken(""{this.Selected.OldPath}"").Value<{type}>();
    return myval;
    */
    return default({type});
    
}}";
        }
    }
}
