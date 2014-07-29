using System.Text.RegularExpressions;
using Bespoke.Sph.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace http.adapter.test
{
    [TestClass]
    public class NamingUtilityTextFixture
    {
        const string Expected = "PostNewBookingAsync";
        [TestMethod]
        public void Undescore()
        {
            Assert.AreEqual(Expected, "POST_new_booking_Async".ToCsharpIdentitfier());
        }

        [TestMethod]
        public void Lower()
        {
            Assert.AreEqual(Expected, "post_new_booking_async".ToCsharpIdentitfier());
        }

        [TestMethod]
        public void Upper()
        {
            Assert.AreEqual(Expected, "POST_NEW_BOOKING_ASYNC".ToCsharpIdentitfier());
        }
        [TestMethod]
        public void Pascal()
        {
            Assert.AreEqual(Expected, "POST_NewBooking_Async".ToCsharpIdentitfier());
        }
        [TestMethod]
        public void RegexWithPascal()
        {
            const string INPUT = "POST_NewBooking_Async";
            var rg = new Regex("([a-z])([A-Z])");

            var t = rg.Replace(INPUT, "$1_$2");
            Assert.AreEqual(Expected, t.ToCsharpIdentitfier());
        }
        [TestMethod]
        public void Sentence()
        {
            Assert.AreEqual(Expected, "POST_ new booking _Async".ToCsharpIdentitfier());
        }
        [TestMethod]
        public void Sentence2()
        {
            Assert.AreEqual(Expected, "POST  new booking Async".ToCsharpIdentitfier());
        }
        [TestMethod]
        public void CamelCase()
        {
            Assert.AreEqual(Expected, "POST_ newBooking _Async".ToCsharpIdentitfier());
        }
    }
}
