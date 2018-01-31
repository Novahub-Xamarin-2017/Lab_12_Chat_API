using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApplication1.Models.DTOModels
{
    [DataContract]
    public class SignInForm
    {
        [DataMember(Name = "user_name")]
        public string UserName { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }    

        [DataMember(Name = "firebase_token")]
        public string FirebaseToken { get; set; }
    }
}