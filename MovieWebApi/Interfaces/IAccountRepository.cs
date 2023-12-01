using AutoMapper.Configuration.Conventions;
using MovieWebApi.Models;

namespace MovieWebApi.Interfaces
{
    public interface IAccountRepository
    {
        public bool CreateAccount(Account account);

        public bool Save();

        public bool SameAccountExist(string username, string password);

        public bool ApiKeyVerified(string ApiKey);
    }
}
