using System.Diagnostics;

using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Moq;

using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Privacy
{
    /// <summary>
    /// PrivacyTests Class For Privacy Page 
    /// </summary>
    public class PrivacyTests
    {
        #region TestSetup

        // Privacy Model static field/attribute
        public static PrivacyModel PageModel;

        /// <summary>
        ///  Test Initialization for Privacy Page
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // logging attribute created
            var MockLoggerDirect = Mock.Of<ILogger<PrivacyModel>>();

            // Privacy Model instance created with logging attribute passed in constructor
            PageModel = new PrivacyModel(MockLoggerDirect)
            {
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test for OnGet 
        /// </summary>
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {
            // ----------------- Arrange -----------------

            // Act
            PageModel.OnGet();

            // ----------------- Reset -----------------

            // ----------------- Assert -----------------
            Assert.AreEqual(true, PageModel.ModelState.IsValid);
        }

        #endregion OnGet
    }
}