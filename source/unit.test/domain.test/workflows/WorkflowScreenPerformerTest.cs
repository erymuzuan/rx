using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowScreenPerformerTest
    {
        private Mock<IRepository<UserProfile>> m_usersRepos;

        [SetUp]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());

            var qp = new Mock<QueryProvider>(MockBehavior.Loose);
            ObjectBuilder.AddCacheList(qp.Object);


            var ds = new Mock<IDirectoryService>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList(ds.Object);

            m_usersRepos = new Mock<IRepository<UserProfile>>(MockBehavior.Strict);
            m_usersRepos.Setup(x => x.LoadOneAsync(It.IsAny<IQueryable<UserProfile>>()))
                .Returns(Task.FromResult(new UserProfile
                {
                    Username = "ima",
                    Email = "ima@bespoke.com.my",
                    Department = "IT",
                    Designation = "Programmer"
                }));


            ObjectBuilder.AddCacheList(m_usersRepos.Object);
        }

        [Test]
        public async Task ViewScreenByPerformerByUserName()
        {
            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "Username",
                    Value = "ima"
                }
            };

            var users = await screen.GetUsersAsync(null);
            CollectionAssert.Contains(users, "ima");

        }

        [Test]
        public async Task ViewScreenByPerformerByUserNameExpression()
        {
            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "Username",
                    Value = "=item.CurrentUser"
                }
            };
            var wf = new TestWf { CurrentUser = "ima" };
            var users = await screen.GetUsersAsync(wf);
            CollectionAssert.Contains(users, "ima");

        }

        [Test]
        public async Task ViewScreenByPerformerByDepartment()
        {
            // add ima to the list of person in every department
            m_usersRepos.Setup(x => x.GetListAsync(It.IsAny<IQueryable<UserProfile>>(),
                It.IsAny<Expression<Func<UserProfile, string>>>()))
                .Returns(Task.FromResult((new[] { "ima" }).AsEnumerable()));

            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "Department",
                    Value = "IT"
                }
            };

            var users = await screen.GetUsersAsync(null);
            CollectionAssert.Contains(users, "ima");

        }

        [Test]
        public async Task ViewScreenByPerformerByDepartmentExpression()
        {
            {
                // add ima to the list of person in every department
                m_usersRepos.Setup(x => x.GetListAsync(It.IsAny<IQueryable<UserProfile>>(),
                    It.IsAny<Expression<Func<UserProfile, string>>>()))
                    .Returns(Task.FromResult((new[] { "ima" }).AsEnumerable()));

                var screen = new ScreenActivity
                {
                    Performer = new Performer
                    {
                        UserProperty = "Department",
                        Value = "=item.CurrentDepartment"
                    }
                };
                var wf = new TestWf { CurrentUser = "ima" };
                var users = await screen.GetUsersAsync(wf);
                CollectionAssert.Contains(users, "ima");

            }

        }

    }
    public class TestWf : Workflow
    {
        public string CurrentUser { get; set; }
        public string CurrentDepartment { get; set; }
    }
}
