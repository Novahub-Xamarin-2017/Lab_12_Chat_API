using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Models.DTOModels;
using WebApplication1.Models.ServiceModels;

namespace WebApplication1.Controllers
{
    public class ChatController: ApiController
    {
        [HttpPost]
        [Route("api/chats/signup")]
        public void SignUp([FromBody]SignUpForm form)
        {
            var user = new User
            {
                FullName = form.FullName,
                Password = form.Password,
                UserName = form.UserName
            };

            var context = new ChatContext();
            context.Users.Add(user);
            context.SaveChanges();
        }

        [HttpPost]
        [Route("api/chats/signin")]
        public void SignIn([FromBody]SignInForm form)
        {
            
        }

        [HttpPost]
        [Route("api/chats/send")]
        public void Send([FromBody]string message)
        {
            var fromUserName = Request.Headers.GetValues("user").First();

            var context = new ChatContext();
            var allOtherUsers = context.Users.Where(x => x.UserName != fromUserName).ToList();
            allOtherUsers.ForEach(x=> SendMessage(x, message));
        }

        private void SendMessage(User user, string message)
        {
            //
        }
    }
}