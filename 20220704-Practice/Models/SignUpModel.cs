using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _20220704_Practice.Models
{
    public class SignUpModel //資料庫的物件
    {
        [Key]
        [Required]
        public string Username { get; set; }
        /*
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        */

        [Required]
        public string Password { get; set; }
        /*
        [Required]
        [Compare("ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        */
    }
}
