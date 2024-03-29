﻿using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Users
{
    /// <summary>
    /// ReadProfilePage class for Read
    /// </summary>
    class ReadProfilePage
    {

        #region TestSetup
        /// <summary>
        /// Read - ProfilePage Model static field/attribute
        /// </summary>
        public static ProfilePageModel PageModel;

        /// <summary>
        /// Test Initialization for ProfilePage
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // logging attribute created
            var MockLoggerDirect = Mock.Of<ILogger<ProfilePageModel>>();

            // Read Profile Model instance created with logging attribute passed in constructor
            PageModel = new ProfilePageModel(MockLoggerDirect, TestHelper.UserService)
            {
                // Set the Page Context
                PageContext = TestHelper.PageContext
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test for OnGet() UserModel
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_UserModel()
        {
            // Arrange

            // Act

            PageModel.OnGet();

            // Reset

            // Assert

            //check model state is valid 
            Assert.AreEqual(true, PageModel.ModelState.IsValid);

        }

        #endregion OnGet


    }
}