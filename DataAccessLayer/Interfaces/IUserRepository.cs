using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        public User? GetUser(Func<User,bool> action);
        public Task<User?> GetUserAsync(int id);
        public Task<User?> GetUserAsync(string email);

        public Task<User> StoreAsync(User user);
        public Task<User?> GetUserByIdAsync(int id);

    }
}
