using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using RestSharp;
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
            if (!chatContext.Users.Any(x => x.UserName == form.UserName && x.Password == form.Password))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var currentUser = chatContext.Users.First(user => user.UserName == form.UserName);
            currentUser.FirebaseToken = form.FirebaseToken;
            chatContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/chats/send")]
        public void Send([FromBody]string message)
        {
            var fromUserName = Request.Headers.GetValues("user").First();
            var allOtherUsers = chatContext.Users.Where(x => x.UserName != fromUserName).ToList();
            allOtherUsers.ForEach(user => SendMessage(user, message));
        }

        [HttpDelete]
        [Route("api/chats/signout")]
        public IHttpActionResult LogOut([FromBody] string token)
        {
            var user = chatContext.Users.FirstOrDefault(x => x.FirebaseToken == token);
            if (user == null) 
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            user.FirebaseToken = null;
            return Ok();
        }

        private static void SendMessage(User user, string message)
        {
            const string rootUrl = "https://fcm.googleapis.com";
            var client = new RestClient(rootUrl);
            var request = new RestRequest("/fcm/send", Method.POST);
            request.AddHeader("Authorization",
                "key=AAAAUEcirzM:APA91bG5bgf1FZW31CHcrWscmkxLLbumln-oNLF1yhAGJ8kU5lORYd7dHceen7PzSt4DxO0D2mx3_xRUpKlgj0GGYgqOgOci4i22twpKPD6_xQ7Om5mzszmR1UvIuNsKSv2IDfW2JDZD");
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(new
            {
                to = user.FirebaseToken,
                notification = new
                {
                    body = message,
                    title = user.FullName,
                    content_available = true,
                    priority = "high"
                },
                data = new
                {
                    body = message,
                    title = user.FullName,
                    content_available = true,
                    priority = "high"
                }
            });
            client.Execute(request);
        }
    }
}