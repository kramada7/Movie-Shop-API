using MovieShop.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string email, string password);
        Task<User> CreateUserAsync(string email, string password, string firstName, string lastName);
        Task<IEnumerable<Purchase>> GetPurchases(int userId);
    }
}
