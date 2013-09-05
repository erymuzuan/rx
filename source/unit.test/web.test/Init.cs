using NUnit.Framework;

namespace web.test
{
    [TestFixture]
   public class Init :BrowserTest
    {
     
        public void ClearDatabase()
        {
            this.ExecuteNonQuery("DELETE ");
        }
    }
}
