using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace OfferWebSystem.Models
{
    public class LogIn
    {

        public long ID { get; set; }
        [Required(ErrorMessage = "Enter UserID")]
        public string USERID { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string USERPASSWORD { get; set; }
    }


    public class RegisterUser
    {

        public long ID { get; set; }
        [Required(ErrorMessage = "Enter UserID")]
        [Remote("IsUserIDExists", "SignIn", HttpMethod = "POST", ErrorMessage = "User name already exists.")]
        public string USERID { get; set; }

        [Required(ErrorMessage = "Enter Email Address")]
        [DataType(DataType.EmailAddress)]
        public string USEREMAIL { get; set; }

        [Required(ErrorMessage = "Enter Company Name")]
        public string USERCOMPANY { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string USERPASSWORD { get; set; }

        [System.Web.Mvc.Compare("USERPASSWORD",ErrorMessage ="Password doesn't Match!!!")]
        [DataType(DataType.Password)]
        public string CONFIRMPASSWORD { get; set; }
    }

    public class ForgetPassword
    {

        
        [Required(ErrorMessage = "Enter UserID")]
        public string USERID { get; set; }

        [Required(ErrorMessage = "Enter Email Address")]
        [DataType(DataType.EmailAddress)]
        public string USEREMAIL { get; set; }
        
    }
}