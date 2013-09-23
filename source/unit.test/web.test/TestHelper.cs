namespace web.test
{
    public static class TestHelper
    {
        public static TestUser CreateSpaceAdmin()
        {
            var spaceAdminUser = new TestUser
            {
                UserName = "ruang-admin",
                FullName = "Ruang Admin",
                Email = "ruang.admin@bespoke.com.my",
                Department = "Test",
                Designation = "Space Admin Manager",
                Password = "abcad12334535",
                StartModule = "space.list",
                Roles = new[] { "can_add_space", "can_edit_space", "can_edit_space_template", "can_edit_building_template" }
            };
            return spaceAdminUser;
        }
    }
}
