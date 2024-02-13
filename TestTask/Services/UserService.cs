using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext dbContext;
        public UserService(ApplicationDbContext applicationDb)
        {
            dbContext = applicationDb;
        }
        public async Task<User> GetUser()
        {
            //var result = dbContext.Users.MaxBy(x => x.Orders.Count()); // Здравствйте!
            //return result;                                             // MaxBy() можно было использовать?         

            var result = from user in dbContext.Users
                         where user.Orders.Count == (from or in dbContext.Orders
                                                     group or.UserId by or.UserId into g
                                                     select new { Name = g.Key, Count = g.Count() })
                                                     .Max( u => u.Count)
                         select user;           
            
            return await result.FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await dbContext.Users.Where(s => s.Status == UserStatus.Inactive).ToListAsync();
        }
    }
}
