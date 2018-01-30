using WebApplication1.Models.ServiceModels;

namespace WebApplication1
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ChatContext : DbContext
    {
        // Your context has been configured to use a 'ChatContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'WebApplication1.ChatContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ChatContext' 
        // connection string in the application configuration file.
        public ChatContext()
            : base("name=ChatContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<User> Users { get; set; }
    }
}