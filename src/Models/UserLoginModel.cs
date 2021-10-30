﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Model for handling user login and user database user.json
    /// </summary>
    public class  UserLoginModel
    {
        /// <summary>
        /// get and set username method for username in userservices
        /// required field for login 
        /// </summary>
        [Required(ErrorMessage = "* This field is required")]
        public string username { get; set; }

        /// <summary>
        /// get and set method for password in userservices 
        /// required to min 6 characters and field is required for login 
        /// </summary>
        [Required(ErrorMessage = "* This field is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "* Must be at least 6 characters long")]
        public string password { get; set; }
    }
}
