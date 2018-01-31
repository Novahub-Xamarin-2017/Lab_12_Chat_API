using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models.DTOModels;
using WebApplication1.Models.ServiceModels;

namespace WebApplication1.Controllers
{
    public class ChatController: ApiController
    {
        private readonly ChatContext chatContext = new ChatContext();
        [HttpPost]
        [Route("api/chats/signup")]
        public IHttpActionResult SignUp([FromBody]SignUpForm form)
        {
            if (chatContext.Users.Any(x => x.UserName == form.UserName) || form.Password.Length < 6)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            chatContext.Users.Add(new User
            {
                FullName = form.FullName,
                Password = form.Password,
                UserName = form.UserName
            });
            chatContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/chats/signin")]
        public IHttpActionResult SignIn([FromBody]SignInForm form)
        {
            if (chatContext.Users.Any(x => x.UserName == form.UserName && x.Password == form.Password))
                return Ok();
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        [Route("api/chats/send")]
        public void Send([FromBody]string message)
        {
            var fromUserName = Request.Headers.GetValues("user").First();
            var allOtherUsers = chatContext.Users.Where(x => x.UserName != fromUserName).ToList();
            allOtherUsers.ForEach(user => SendMessage(user, message));
        }

        private void SendMessage(User user, string message)
        {
            //
        }
    }
}