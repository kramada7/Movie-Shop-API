using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieShop.Data;
using MovieShop.Entities;

namespace MovieShop.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        public UserService(IUserRepository userRepository, ICryptoService cryptoService)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
        }
        public async Task<User> CreateUserAsync(string email, string password, string firstName, string lastName)
        {
            var dbUser = await _userRepository.GetUserByEmail(email);
            if (dbUser != null)
            {
                return null;
            }
            var salt = _cryptoService.CreateSalt();
            var hashedPassword = _cryptoService.HashPassword(password, salt);
            var user = new User
            {
                Email = email,
                HashedPassword = hashedPassword,
                FirstName = firstName,
                LastName = lastName,
                Salt = salt
            };

            var createdUser = await _userRepository.AddAsync(user);

            return createdUser;
        }

        public async Task<IEnumerable<Purchase>> GetPurchases(int userId)
        {
            return await _userRepository.GetUserPurchasedMovies(userId);
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var dbUser = await _userRepository.GetUserByEmail(email);
            if (dbUser != null)
            {
                var dbSalt = dbUser.Salt;
                var dbHashedPassword = dbUser.HashedPassword;
                var userHashedPassword = _cryptoService.HashPassword(password, dbSalt);

                if (userHashedPassword == dbHashedPassword)
                {
                    return dbUser;
                }
            }
            return null;
            
        }
    }
}
