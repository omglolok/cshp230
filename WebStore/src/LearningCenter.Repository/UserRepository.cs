using System.Linq;

namespace LearningCenter.Repository
{
    public interface IUserRepository
    {
        UserModel LogIn(string email, string password);
        UserModel Register(string email, string password);
    }
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    class UserRepository : IUserRepository
    {
        public UserModel LogIn(string username, string password)
        {
            var user = DatabaseAccessor.Instance.Users
                .FirstOrDefault(t => t.Username.ToLower() == username.ToLower()
                && t.Password == password);
            if (user == null)
            {
                return null;
            }
            return new UserModel { Id = user.UserId, Username = user.Username, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName };

        }
        public UserModel Register(string email, string password)
        {
            var user = DatabaseAccessor.Instance.Users
            .FirstOrDefault(t => t.Username.ToLower() == email.ToLower());
            if (user == null)
            {
                user = DatabaseAccessor.Instance.Users
                        // only require email and password in account creation, email and username are identical on registration
                        .Add(new ProductDatabase.User { Username = email, Email = email, Password = password, FirstName = string.Empty, LastName = string.Empty });

                DatabaseAccessor.Instance.SaveChanges();

                return new UserModel { Id = user.UserId, Email = user.Email };
            }
            return null;
        }
    }
}
