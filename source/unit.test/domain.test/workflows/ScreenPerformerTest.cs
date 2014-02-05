﻿using System;
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
    public class ScreenPerformerTest
    {
        private Mock<IRepository<UserProfile>> m_usersRepos;
        private Mock<IDirectoryService> m_ds;
        [SetUp]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());

            var qp = new Mock<QueryProvider>(MockBehavior.Loose);
            ObjectBuilder.AddCacheList(qp.Object);


            m_ds = new Mock<IDirectoryService>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList(m_ds.Object);

            m_usersRepos = new Mock<IRepository<UserProfile>>(MockBehavior.Strict);
            m_usersRepos.Setup(x => x.LoadOneAsync(It.IsAny<IQueryable<UserProfile>>()))
                .Returns(Task.FromResult(new UserProfile
                {
                    UserName = "ima",
                    Email = "ima@bespoke.com.my",
                    Department = "IT",
                    Designation = "Programmer"
                }));


            ObjectBuilder.AddCacheList(m_usersRepos.Object);
        }

        [Test]
        public async Task UserName()
        {
            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "UserName",
                    Value = "ima"
                }
            };

            var users = await screen.GetUsersAsync(null);
            CollectionAssert.Contains(users, "ima");

        }

        [Test]
        public async Task UserNameExpression()
        {
            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "UserName",
                    Value = "=item.CurrentUser"
                }
            };
            var wf = new TestWf { CurrentUser = "ima" };
            var users = await screen.GetUsersAsync(wf);
            CollectionAssert.Contains(users, "ima");

        }

        [Test]
        public async Task Department()
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
        public async Task DepartmentExpression()
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
        [Test]
        public async Task Designation()
        {
            // add ima to the list of person in every department
            m_usersRepos.Setup(x => x.GetListAsync(It.IsAny<IQueryable<UserProfile>>(),
                It.IsAny<Expression<Func<UserProfile, string>>>()))
                .Returns(Task.FromResult((new[] { "ima" }).AsEnumerable()));

            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "Designation",
                    Value = "Kerani"
                }
            };

            var users = await screen.GetUsersAsync(null);
            CollectionAssert.Contains(users, "ima");

        }

        [Test]
        public async Task DesignationExpression()
        {
            {
                // add ima to the list of person in every department
                m_usersRepos.Setup(x => x.GetListAsync(It.IsAny<IQueryable<UserProfile>>(),
                    It.IsAny<Expression<Func<UserProfile, string>>>()))
                    .Returns(Task.FromResult((new[] { "imca" }).AsEnumerable()));

                var screen = new ScreenActivity
                {
                    Performer = new Performer
                    {
                        UserProperty = "Designation",
                        Value = "=item.CurrentDesignation"
                    }
                };
                var wf = new TestWf { CurrentDesignation = "Kerani" };
                var users = await screen.GetUsersAsync(wf);
                CollectionAssert.Contains(users, "imca");

            }

        }
        [Test]
        public async Task Roles()
        {
            m_ds.Setup(x => x.GetUserInRolesAsync("admin"))
                .Returns(Task.FromResult((new[] { "ima" })));

            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "Roles",
                    Value = "admin"
                }
            };

            var users = await screen.GetUsersAsync(null);
            CollectionAssert.Contains(users, "ima");

        }

        [Test]
        public async Task RolesExpression()
        {
            m_ds.Setup(x => x.GetUserInRolesAsync("admin"))
                .Returns(Task.FromResult((new[] { "ima" })));
            var screen = new ScreenActivity
            {
                Performer = new Performer
                {
                    UserProperty = "Roles",
                    Value = "=item.CurrentRole"
                }
            };
            var wf = new TestWf { CurrentRole = "admin" };
            var users = await screen.GetUsersAsync(wf);
            CollectionAssert.Contains(users, "ima");



        }

    }
    public class TestWf : Workflow
    {
        public string CurrentUser { get; set; }
        public string CurrentDepartment { get; set; }
        public string CurrentDesignation { get; set; }
        public string CurrentRole { get; set; }
    }
}
