﻿using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using ContosoCrafts.WebSite.Pages;
using System;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc;


namespace UnitTests.Pages.Users
{
    /// <summary>
    /// RegisterTest class for Register
    /// </summary>
    class RegisterTest
    {
        #region TestSetup
        // Register Model static field/attribute
        public static RegisterModel PageModel;

        /// <summary>
        ///  Test Initialization for RegisterPage
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // logging attribute created
            var MockLoggerDirect = Mock.Of<ILogger<RegisterModel>>();

            // Register Model instance created with logging attribute passed in constructor
            PageModel = new RegisterModel(MockLoggerDirect, TestHelper.UserService)
            {
                // Set the Page Context
                PageContext = TestHelper.PageContext
            };
        }
        #endregion TestSetup

        #region OnPost

        /// <summary>
        /// Test for OnPost Valid UserModel
        /// </summary>
        [Test]
        public void OnPost_Valid_UserModel_Should_Add_New_Record()
        {
            // Arrange
            // oldCount variable for total numbers of Users
            var oldCount = TestHelper.UserService.GetUsers().Count();

            // Random object instance creation
            Random rnd = new Random();

            // userID variable with random number
            int userID = rnd.Next(1, 999999);

            // UserModel instance created
            PageModel.BindUser = new UserModel()
            {
                //user id
                UserID = userID,

                //username
                Username = "TestValidName",

                //password
                Password = "TestValidPassword",

                //email
                Email = "TestValidEmail@gmail.com",

                //location 
                Location = "TestValidLocation"
            };

            //Act
            // Fetch result from OnPost()
            var result = PageModel.OnPost() as RedirectToPageResult;

            //Reset

            //Assert

            Assert.AreEqual(true, PageModel.ModelState.IsValid);

            // Confirm that record was created
            Assert.AreEqual(oldCount + 1, TestHelper.UserService.GetUsers().Count());
        }

        /// <summary>
        /// Test for Duplicate Registration 
        /// </summary>
        [Test]
        public void OnPost_ValidExisting_UserModel_Should_Return_Page()
        {
            // Arrange

            // Create Bind User Object
            PageModel.BindUser = new UserModel()
            {
                // A User with this Username Already Exists in User Database/Dataset
                //username
                Username = "edwin",

                //password
                Password = "DuplicatePassword",

                //email
                Email = "email@gmail.com",

                //location 
                Location = "USA"
            };

            // Act

            // Fetch result from OnPost()
            var getResult = PageModel.OnPost() as RedirectToPageResult;
            var message = PageModel.DuplicateUserPrompt;
            // Reset

            // Assert
            //page model is valid
            Assert.AreEqual(true, PageModel.ModelState.IsValid);

            //message is correct 
            Assert.AreEqual("Username Already Exist, Log in with your username?", message);           
        }


        /// <summary>
        /// Test for OnPost InValid UserModel
        /// </summary>
        [Test]
        public void OnPost_InValid_UserModel_Name_Should_Not_Add_New_Record()
        {
            //Arrange
            // oldCount variable for total numbers of Users
            var oldCount = TestHelper.UserService.GetUsers().Count();

            // Random object instance creation
            Random rnd = new Random();

            // userID variable with random number
            int userID = rnd.Next(1, 999999);

            // UserModel instance created
            PageModel.BindUser = new UserModel()
            {
                //user id
                UserID = userID,

                //username
                Username = "User@@@", //should only contain numbers and letter

                //password
                Password = "TestValidpassword", //password is less than 6

                //email
                Email = "TestvalidEmail@gmail.com",

                //location
                Location = "TestvalidLocation"
            };


            //Act
            // Fetch result from OnPost()
            var result = PageModel.OnPost() as RedirectToPageResult;

            //Reset

            //Assert
            // Confirm that no new recorded was added
            Assert.AreEqual(oldCount, TestHelper.UserService.GetUsers().Count());
        }

        /// <summary>
        /// Test for OnPost Invalid UserModel password
        /// </summary>
        [Test]
        public void OnPost_UserModel_Password_With_Invalid_Characters_Should_Not_Add_New_Record()
        {
            //Arrange

            var oldCount = TestHelper.UserService.GetUsers().Count();

            // Random object instance creation
            Random rnd = new Random();

            int userID = rnd.Next(1, 999999);

            // UserModel instance created
            PageModel.BindUser = new UserModel()
            {
                // id 
                UserID = userID,

                // username 
                Username = "TestValidUsername",

                // password to fail 
                // Valid passwords are more than 6 and all English characters
                Password = "~!@#@#$#%%$^&$#%%^$%#$%^$#$",

                // email 
                Email = "TestvalidEmail@gmail.com",

                // location 
                Location = "TestvalidLocation"
            };

            // Act
            // Fetch result from OnPost
            var result = PageModel.OnPost() as RedirectToPageResult;

            //Reset

            //Assert
            // Confirm that no new recorded was added
            var currentCount = TestHelper.UserService.GetUsers().Count();
            //check old count to new count 
            Assert.AreEqual(oldCount, currentCount);
        }

        /// <summary>
        /// Test for OnPost Invalid UserModel password
        /// </summary>
        [Test]
        public void OnPost__Length_Less_Than_6_Password_Should_Not_Add_New_Record()
        {
            //Arrange
            //count number of users in user database
            var oldCount = TestHelper.UserService.GetUsers().Count();

            // Random object instance creation
            Random rnd = new Random();

            int userID = rnd.Next(1, 999999);

            // UserModel instance created
            PageModel.BindUser = new UserModel()
            {
                // id 
                UserID = userID,

                // username 
                Username = "TestValidUsername",

                // password to fail 
                // Valid passwords are more than 6 and all English characters
                Password = "abc",

                // email 
                Email = "TestvalidEmail@gmail.com",

                // location 
                Location = "TestvalidLocation"
            };

            // Act
            // Fetch result from OnPost
            var result = PageModel.OnPost() as RedirectToPageResult;

            //Reset

            //Assert
            // Confirm that no new recorded was added
            var currentCount = TestHelper.UserService.GetUsers().Count();

            //check old count and new count are equal 
            Assert.AreEqual(oldCount, currentCount);
        }

        /// <summary>
        /// Test for OnPost InValid UserModel Email
        /// </summary>
        [Test]
        public void OnPost_InValid_UserModel_Email_Should_Not_Add_New_Record()
        {
            //Arrange
            var oldCount = TestHelper.UserService.GetUsers().Count();

            // Random object instance creation
            Random rnd = new Random();

            int userID = rnd.Next(1, 999999);

            // UserModel instance created
            PageModel.BindUser = new UserModel()
            {
                //user id 
                UserID = userID,

                //username 
                Username = "TestValidUsername",

                //password 
                Password = "TestValidpassword",

                //email to fail 
                Email = "TestvalidEmail@gmail", //email should be in valid format

                //location 
                Location = "TestvalidLocation"
            };

            //Act
            // Fetch result from OnPost
            var result = PageModel.OnPost() as RedirectToPageResult;

            //Reset

            //Assert
            // Confirm that no new recorded was added
            Assert.AreEqual(oldCount, TestHelper.UserService.GetUsers().Count());
        }

        /// <summary>
        /// Test for OnPost Invalid UserModel Location
        /// </summary>
        [Test]
        public void OnPost_InValid_Location_Should_Not_Add_New_Record()
        {
            //Arrange
            var oldCount = TestHelper.UserService.GetUsers().Count();

            // Random object instance creation
            Random rnd = new Random();

            int userID = rnd.Next(1, 999999);

            // UserModel instance created
            PageModel.BindUser = new UserModel()
            {
                //user id 
                UserID = userID,

                //username 
                Username = "TestValidUsername",

                //password 
                Password = "TestValidpassword",

                //email
                Email = "TestvalidEmail@gmail.com",

                //location
                Location = "TestInvalidLocation!!!!" //should only contain letters
            };

            // Act
            // Fetch result from OnPost
            var result = PageModel.OnPost() as RedirectToPageResult;

            //Reset

            //Assert
            // Confirm that no new recorded was added
            Assert.AreEqual(oldCount, TestHelper.UserService.GetUsers().Count());
        }
        #endregion OnPost
    }
}