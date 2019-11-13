using MovieShop.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MovieShop.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Purchase>> GetUserPurchasedMovies(int userId)
        {
            var userMovies = await _dbContext.Purchases.Where(p => p.UserId == userId).Include(p => p.Movie).ToListAsync();
            return userMovies;
        }
    }
}
