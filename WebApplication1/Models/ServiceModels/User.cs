using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ServiceModels
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public string FirebaseToken { get; set; }
    }
}