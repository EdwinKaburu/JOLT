using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    public class UserModel
    {
        //This class contains data validation above each attribute
        public int userID { get; set; }


        [Required(ErrorMessage = "* This field is required")]
        public string username { get; set; }


        [Required(ErrorMessage = "* This field is required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "* Must be at least 6 characters long")]
        public string password { get; set; }

       /* [Required(ErrorMessage = "* This field is required")]
        [Compare("password")]
        public string confirmPassword { get; set; }*/


        [EmailAddress(ErrorMessage = "* Enter a valid email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "* Must be an email address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "* This field is required")]
        public string email { get; set; }


        [Required(ErrorMessage = "* This field is required")]
        public string location { get; set; }

        //public override string ToString() => JsonSerializer.Serialize<UserModel>(this);

 
    }
}