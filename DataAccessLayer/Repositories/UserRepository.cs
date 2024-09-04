using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public User? GetUser(Func<User, bool> condition)
        {
            return appDbContext.Users.Where(condition).FirstOrDefault();
        }

        public Task<User> GetUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> StoreAsync(User user)
        {
            await appDbContext.Users.AddAsync(user);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await appDbContext.Users
                 .Include(u => u.Roles)
                 .Include(u => u.Teacher)
                 .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
