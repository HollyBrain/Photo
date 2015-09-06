using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Photo.Models
{
    public class UserModel
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name="Email address: ")]
        public string Email { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "FirstName: ")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "LastName: ")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(200, MinimumLength=6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        public string UserFace { get; set; }

    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Email ddress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}