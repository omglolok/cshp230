using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningCenter.Repository;

namespace LearningCenter.Business
{
    public interface IUserManager
    {
        UserModel LogIn(string email, string password);
        UserModel Register(string email, string password);
    }
    class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;
        public UserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public UserModel LogIn(string email, string password)
        {
            var user = userRepository.LogIn(email, password);
            if (user == null)
            {
                return null;
            }
            return new UserModel { Id = user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Username = user.Username };
        }
        public UserModel Register(string email, string password)
        {
            var user = userRepository.Register(email, password);
            if (user == null)
            {
                return null;
            }
            return new UserModel { Id = user.Id, Email = user.Email };
        }
    }
}
