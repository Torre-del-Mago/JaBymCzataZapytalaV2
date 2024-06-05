using Login.Database.Entity;
using MongoDB.Driver;

namespace Login.Service.LoginService
{
    public class LoginService : ILoginService
    {

        const string connectionUri = "mongodb://root:student@student-swarm01.maas:27017/";

        private MongoClient _client { get; set; }
        private IMongoDatabase _database { get; set; }
        public LoginService()
        {
            _client = new MongoClient(connectionUri);
            _database = _client.GetDatabase("rsww_184543_login");
        }

        public bool IsUsernameCorrect(string username)
        {
            var userCollection = _database.GetCollection<User>("users").AsQueryable();

            var user = userCollection.Where(u => u.Login == username).FirstOrDefault();

            return user != null;
        }
    }
}
