using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieWebApi.Data;
using MovieWebApi.Interfaces;
using MovieWebApi.Models;

namespace MovieWebApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MovieWebApiContext _context;

        public AccountRepository(MovieWebApiContext context)
        {
            _context = context;
        }

        public bool CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            return Save();
        }

        public bool Save()
        {
            var saveStatus = _context.SaveChanges();
            return saveStatus > 0 ? true : false;
        }

        public bool SameAccountExist(string username, string password)
        {
            var sameAccountDetected = _context.Accounts.Any(a => a.Username.Equals(username.ToLower()) && a.Password.Equals(password.ToLower()));
            return sameAccountDetected;
        }

        public bool ApiKeyVerified(string ApiKey)
        {
            // Check if the API Key exists.
            return _context.Accounts.Any(a => a.ApiKey.Equals(ApiKey));
        }
    }
}
