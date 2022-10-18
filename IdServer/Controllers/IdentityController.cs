using IdServer.Api.Helpers;
using IdServer.Api.Models;
using IdServer.Core.CommandModel;
using IdServer.Core.Services;
using IdServer.Infraestructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityQueryRepository _identityQueryRepository;
        private readonly IEmailSender _emailSender;
        private readonly ISecurityTokenProvider _securityTokenProvider;

        public IdentityController(ILogger<IdentityController> logger, IPasswordHasher passwordHasher, IUserRepository userRepository, IEmailSender emailSender, ISecurityTokenProvider securityTokenProvider, IIdentityQueryRepository identityQueryRepository)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _securityTokenProvider = securityTokenProvider;
            _identityQueryRepository = identityQueryRepository;
        }

        [HttpPost("create", Name = "CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserApiRequest request)
        {
            var user = new User(request.Email, request.FirstName, request.LastName, request.Email, request.ClubCode, "UserTest");
            user = _userRepository.Add(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
            var code = _securityTokenProvider.GenerateToken(user.Id);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            await _emailSender.SendEmailConfirmationAsync(request.Email, callbackUrl);
            return Ok(user.Id);
        }

        [HttpPost("confirm", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailApiRequest request)
        {
            var user = await _userRepository.FindByPk(request.UserId);
            if (user == null || user.EmailConfirmed)
            {
                throw new ApplicationException($"Unable to confirm user.");
            }
            if (!_securityTokenProvider.ValidateToken(request.Token, user.Id))
            {
                throw new ApplicationException($"Unable to confirm user.");
            }
            var passwordHashed = _passwordHasher.HashPassword(request.Password, KeyDerivationPrf.HMACSHA256, 128 / 8, 100000, 256 / 8);
            user.ConfirmEmail(passwordHashed);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
            return Ok();
        }

        [HttpPost("login", Name = "Login")]
        public async Task<IActionResult> Login(LoginApiRequest request)
        {
            var user = await _identityQueryRepository.GetUserIdentity(request.ClientId, request.Email);
            if (user == null)
            {
                throw new ApplicationException($"Unable to login user.");
            }
            if (!_passwordHasher.VerifyHashedPassword(user.PasswordHash, request.Password, KeyDerivationPrf.HMACSHA256, 128 / 8, 100000, 256 / 8))
            {
                throw new ApplicationException($"Unable to login user.");
            }
            var token = _securityTokenProvider.GenerateAccessToken(user);

            return Ok(new { access_token = token});
        }

        [HttpGet("secret", Name = "Secret")]
        [Authorize]
        public IActionResult Secret()
        {
            return Ok(new { data = "This is a secret endpoint." });
        }
    }
}
