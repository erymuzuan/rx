using System;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowScreenPerformerTest
    {
      
        [Test]
        public void ViewScreenByPerformer()
        {
            var profile = new UserProfile
            {
                Username = "ima",
                Department = "IT",
                Designation = "test",
                RoleTypes ="admin_user,test_user,cashier"
            };
            var wd = new WorkflowDefinition {Name = "ApprovalScreen"};
            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "Roles",
                    Value = "admin_user"
                }
            };
            wd.ActivityCollection.Add(screen);

            var canview = false;
            switch (screen.Performer.UserProperty)
            {
                case "Username":
                    canview = screen.Performer.Value == profile.Username;
                    break;
                case "Department":
                    canview = screen.Performer.Value == profile.Department;
                    break;
                case "Designation":
                    canview = screen.Performer.Value == profile.Designation;
                    break;
                case "Roles":
                    canview = profile.RoleTypes.Contains(screen.Performer.Value);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            Console.WriteLine("user department {0}", profile.Department);
            Console.WriteLine("screen performer {0} {1}", screen.Performer.UserProperty, screen.Performer.Value);
            Assert.AreEqual(canview,true);
           
        }
    }
}
