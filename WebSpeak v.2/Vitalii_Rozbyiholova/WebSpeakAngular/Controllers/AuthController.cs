using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebSpeakAngular.Models;

namespace WebSpeakAngular.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly AuthHelper _helper;

        public AuthController()
        {
            _usersRepository = new UsersRepository();
            _helper = new AuthHelper();
        }

        [HttpGet("UsersLogins")]
        public List<string> UsersLogins()
        {
            List<Users> users = _usersRepository.GetAll().ToList();
            List<string> logins = users.Select(user => user.Email).ToList();
            logins.AddRange(users.Select(user => user.UserName).ToList());

            return logins;
        }

        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
            {
                return InvalidClientRequest();
            }

            if (_helper.IsUser(user))
            {
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenOptions.SecretKey));
                SigningCredentials signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken tokenOptions = new JwtSecurityToken(
                    issuer: TokenOptions.ValidIssuer,
                    audience: TokenOptions.ValidAudience,
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(TokenOptions.MinutesExpire),
                    signingCredentials: signinCredentials);
                tokenOptions.Payload["userLogin"] = user.Login;

                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new {Token = tokenString});
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("Register")]
        public IActionResult Register([FromBody] Users user)
        {
            if (user == null)
            {
                return InvalidClientRequest();
            }

            LoginModel loginModel = new LoginModel
            {
                Login = user.Email,
                Password =user.PasswordHash
            };

            string userId = Guid.NewGuid().ToString("N");
            string hashedPassword = _helper.ComputeSha256Hash(user.PasswordHash);
            UserSettings userSettings = new UserSettings
            {
                LearningLanguageId = 0,
                NativeLanguageId = 0,
                UserId = userId
            };

            user.Id = userId;
            user.UserSettings = new List<UserSettings> {userSettings};
            user.PasswordHash = hashedPassword;

            using (ProductHouseContext db = new ProductHouseContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }


            return Login(loginModel);
        }

        [HttpGet, Route("User/{userLogin}"), Authorize]
        public IActionResult GetUser(string userLogin)
        {
            if (String.IsNullOrEmpty(userLogin)) { return InvalidClientRequest(); }

            string decodedLogin = Uri.UnescapeDataString(userLogin);
            Users user = _helper.GetUserByEmailOrName(decodedLogin);

            if (user == null) { return BadRequest("No such user"); }

            string json = JsonConvert.SerializeObject(new {user = user}, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Ok(json);
        }

        [Authorize, HttpGet, Route("Languages")]
        public IActionResult GetAllLanguages()
        {
            List<Languages> languages = new LanguagesRepository().GetAll().ToList();
            List<LanguageTranslations> languageTranslations = new LanguagesTranslationsRepository().GetAll().ToList();
            var result = (from language in languages
                join langTranslation in languageTranslations on language.Id equals langTranslation.LangId
                where langTranslation.NativeLangId == language.Id
                select new
                {
                    Translation = langTranslation.Translation,
                    Id = language.Id
                }).ToList();
            string json = JsonConvert.SerializeObject(result);

            return Ok(json);
        }

        [Authorize, HttpPost, Route("SetLanguages")]
        public IActionResult SetLanguages([FromBody] ChangeLanguageModel model)
        {
            bool changedSuccessfully = _helper.SetLanguages(model.Login, model.NativeLanguageId, model.LearningLanguageId);

            if (!changedSuccessfully)
            {
                return InvalidClientRequest();
            }

            return Ok();
        }

        private IActionResult InvalidClientRequest()
        {
            return BadRequest("Invalid client request");
        }
    }
}