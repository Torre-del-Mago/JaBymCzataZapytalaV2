using Login.Database.Entity;
using MongoDB.Driver;

namespace Login.Service.LoginService
{
    public class LoginService : ILoginService
    {

        const string connectionUri = "mongodb://mongo:27017";

        private MongoClient _client { get; set; }
        private IMongoDatabase _database { get; set; }
        public LoginService()
        {
            _client = new MongoClient(connectionUri);
            _database = _client.GetDatabase("login");
        }

        public bool isUsernameCorrect(string username)
        {
            var userCollection = _database.GetCollection<User>("login").AsQueryable();

            var user = userCollection.Where(u => u.Login == username).FirstOrDefault();

            return user != null;
        }
    }
}
