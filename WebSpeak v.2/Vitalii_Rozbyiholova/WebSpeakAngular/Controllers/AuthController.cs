using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebSpeakAngular.Models;

namespace WebSpeakAngular.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
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

                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new {Token = tokenString});
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}