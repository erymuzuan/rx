using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class ListenActivity : Activity
    {
        public override bool IsAsync
        {
            get { return true; }
        }

        public override void BeforeGenerate(WorkflowDefinition wd)
        {
            var triggersId = this.ListenBranchCollection.Select(a => a.NextActivityWebId).ToArray();
            var triggers = wd.ActivityCollection.Where(a => triggersId.Contains(a.WebId));
            foreach (var t in triggers)
            {
                t.AfterExcuteCode = string.Format("     await this.FireListenTrigger{1}(\"{0}\");", t.WebId, this.MethodName);
            }
        }

        public async override Task InitiateAsync(Workflow wf)
        {
            await wf.ExecuteAsync(this.WebId);
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            var errors = from a in this.ListenBranchCollection
                         where string.IsNullOrWhiteSpace(a.NextActivityWebId)
                         select new BuildError
                         {
                             Message = string.Format("[ListenActivity] -> Branch \"{0}\" is missing next activity", a.Name)
                         };
            var errors2 = from a in this.ListenBranchCollection
                          where !string.IsNullOrWhiteSpace(a.NextActivityWebId)
                          let next = wd.ActivityCollection.SingleOrDefault(t => t.WebId == a.NextActivityWebId)
                          where !next.IsAsync
                          select new BuildError
                          {
                              Message = string.Format("[ListenActivity] -> Branch \"{0}\" is not an async activity", a.Name)
                          };

            result.Errors.AddRange(errors);
            result.Errors.AddRange(errors2);

            return result;
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();

            // triggered fires
            code.AppendLinf("   public async Task FireListenTrigger{0}(string webId)", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var self = this.GetActivity<ListenActivity>(\"{0}\");", this.WebId);
            code.AppendLinf("       var triggersId = self.ListenBranchCollection" +
                            ".Where(a =>a.NextActivityWebId != webId)" +
                            ".Select(a => a.NextActivityWebId).ToArray();");
            code.AppendLinf("       var fired = this.GetActivity<Activity>(webId);");
            code.AppendLinf("       foreach(var t in triggersId)");
            code.AppendLine("       {");
            code.AppendLine("               var t1 = t;");
            code.AppendLine("               var act = this.GetActivity<Activity>(t1);");
            code.AppendLine("               await act.CancelAsync(this);");
            code.AppendLine("       }");
            code.AppendLine("   }");



            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       var script = ObjectBuilder.GetObject<IScriptEngine>();");
            var count = 1;
            code.AppendLinf("       var listen = this.GetActivity<ListenActivity>(\"{0}\");", this.WebId);
            code.AppendLine();
            // call initiate async for both
            foreach (var branch in this.ListenBranchCollection)
            {
                code.AppendLinf("       var branch{0} = listen.ListenBranchCollection.Single(a => a.WebId == \"{1}\");", count, branch.WebId);
                code.AppendLinf("       var trigger{0} =  this.GetActivity<Activity>(\"{1}\");", count, branch.NextActivityWebId);
                code.AppendLinf("       await trigger{0}.InitiateAsync(this);", count, branch.WebId);
                code.AppendLine();
                count++;
            }
            // set it to -> waiting for one of branch to fire
            code.AppendLine("       this.State = \"WaitingAsync\";");
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.WebId); // set it to this activity
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       return result;");

            code.AppendLine("   }");


            return code.ToString();
        }




    }
}