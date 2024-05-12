using ChatAppServer.Database;
using ChatAppServer.Database.Models;
using ChatAppServer.Database.Repositories;
using System.Net;

namespace ChatAppServer
{
    internal class Program
    {
        private static Server? Server;
        private static DbContext dbContext;
        static async Task Main(string[] args)
        {
            
            dbContext = new DbContext();
            dbContext.ConnectToDatabase();
            
            //UsersRepository usersRepository = new UsersRepository(dbContext.Connection);
            //usersRepository.InsertUser(user);

            Server = new Server(IPAddress.Parse("127.0.0.1"), 55400);
            await Server.StartServer();


        }
    }
}
