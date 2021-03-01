using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Table("Authentication")]
    public class Authentication
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
