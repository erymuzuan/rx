using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ScreenActivityForm : Form
    {
        public BuildValidationResult ValidateBuild(WorkflowDefinition wd, ScreenActivity activity)
        {
            var errors = from f in this.FormDesign.FormElementCollection
                         where f.IsPathIsRequired
                               && string.IsNullOrWhiteSpace(f.Path) && (f.Name != "HTML Section")

                         select new BuildError
                             (
                             this.WebId,
                             string.Format("[ScreenActivity] : {0} => '{1}' does not have path", activity.Name, f.Label)
                             );
            var elements = from f in this.FormDesign.FormElementCollection
                           let err = f.ValidateBuild(wd/*, activity*/)
                           where null != err
                           select err;

            var result = new BuildValidationResult();
            result.Errors.AddRange(errors);
            result.Errors.AddRange(elements.SelectMany(v => v));


            return result;
        }



        public string GetView(WorkflowDefinition wd, ScreenActivity activity)
        {

            var code = new StringBuilder();

            // buttons
            var buttonCommands = this.FormDesign.FormElementCollection.OfType<Button>()
                .Where(b => b.CommandName != "save")
                .Select(b => string.Format("{0} : function(){{" +
                                           "{1}" +
                                           "}}", b.CommandName, b.Command))
                                           .ToArray();
            var buttonCommandJs = string.Join(",\r\n", buttonCommands);
            var saveCommand = string.Format(@",
                save : function(){{
                      var tcs = new $.Deferred(),
                            data = ko.mapping.toJSON(vm.instance),
                            button = $(this);

                        button.prop('disabled', true);
                        context.post(data, ""/wf/{0}/v{1}/{2}"")
                            .then(function(result) {{
                                tcs.resolve(result);
                                @if(Model.Screen.ConfirmationOptions.Type == ""Message"")
                                {{
                                    <text>
                                    var msg = _.template('@Html.Raw(confirmationText)')(result.wf);
                                    app.showMessage(msg, '@Model.Screen.Name', ['OK'])
                                        .done(function(dr){{
                                            console.log(dr);
                                        }});
                                    </text>
                                }}else
                                {{
                                    <text>
                                    window.location = ""@confirmationText"";
                                    </text>
                                }}
                       

                            }});
                        return tcs.promise();
                }}", wd.Id, wd.Version, activity.Name.ToIdFormat());
            if (buttonCommandJs.Length > 0)
                buttonCommandJs = saveCommand + "," + buttonCommandJs;
            else
                buttonCommandJs = saveCommand;

            code.AppendLine("@using System.Web.Mvc.Html");
            code.AppendLine("@using Bespoke.Sph.Domain");
            code.AppendLine("@using Newtonsoft.Json");
            code.AppendLine("@model " + wd.CodeNamespace + "." + activity.ViewModelType);

            code.AppendFormat(@"
@{{
    ViewBag.Title = Model.Instance.Name;
    Layout = ""~/Views/Shared/_Layout.cshtml"";
    const string controllerString = ""Controller"";
    var setting = new JsonSerializerSettings {{TypeNameHandling = TypeNameHandling.All}};
    var confirmationText = Model.Screen.ConfirmationOptions.Value;
    
}}

<div class=""row"">
    <h1>@Model.Screen.FormDesign.Name</h1>
    <span>@Model.Screen.FormDesign.Description</span>
</div>
<div class=""row"">
    <form class=""form-horizontal"" id=""workflow-start-form"" data-bind=""with: instance"">
        @foreach (var fe in Model.Screen.FormDesign.FormElementCollection)
        {{
            var fe1 = fe;
            fe1.Path = fe.Path.ConvertJavascriptObjectToFunction();
            fe1.SetDefaultLayout(Model.Screen.FormDesign);
            @(fe.UseDisplayTemplate ? Html.DisplayFor(f => fe1) : Html.EditorFor(f => fe1))
        }}
   
    </form>

</div>


@section scripts
{{
    <script type=""text/javascript"" src=""/wf/{0}/v{1}/schemas""></script>
    <script type=""text/javascript"">
        require(['services/datacontext', 'jquery','services/app', 'services/system', 'services/config'], function(context,jquery,app, system, config) {{

            
           var instance = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Instance, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               screen = context.toObservable(@Html.Raw(JsonConvert.SerializeObject(Model.Screen, setting)),/@Model.Namespace.Replace(""."",""\\."")\.(.*?),/),
               vm = {{
                id : ""{0}"",
                instance : ko.observable(instance),    
                screen : ko.observable(screen),
                config : config,
                isBusy : ko.observable(){2}
            }};
            
            instance.addChildItem = function(list, type){{
                return function(){{
                     list.push(new type(system.guid()));                  
                }}
            }};
            
            instance.removeChildItem = function(list, obj){{
                return function(){{
                    list.remove(obj);
                }}
            }};

            ko.applyBindings(vm, document.getElementById('body'));
            @*  the div#body is defined in _Layout.cshtml, if you use different Layout then this has got to changed accordingly *@

      
        }});

    </script>
}}", wd.Id, wd.Version, buttonCommandJs);


            return code.ToString();
        }


        public JsRoute CreateJsRoute()
        {
            return new JsRoute
            {
                Title = this.Name,
                Route = string.Format("{0}/:id", this.Route.ToLowerInvariant()),
                Caption = this.Name,
                //Icon = t.IconClass,
                ModuleId = string.Format("viewmodels/{0}", this.Route.ToLowerInvariant()),
                Nav = false
            };
        }
    }
}