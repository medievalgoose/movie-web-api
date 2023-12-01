using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieWebApi.Interfaces;
using MovieWebApi.Models;
using MovieWebApi.Utils;

namespace MovieWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;

        public AccountsController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account newAccount)
        {
            if (newAccount == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_accountRepo.SameAccountExist(newAccount.Username, newAccount.Password))
            {
                return BadRequest();
            }

            var generatedApiKey = ApiKeyGenerator.GenerateSHA256Hash(newAccount.Username + newAccount.Password);
            newAccount.ApiKey = generatedApiKey;

            if (!_accountRepo.CreateAccount(newAccount))
            {
                return BadRequest();
            }

            return Ok($"New account added to the list \n Your API Key: {generatedApiKey}"); 
        }
    }
}
