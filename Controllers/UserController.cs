using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MovinderAPI.Models;
using MovinderAPI.Dtos;
using MovinderAPI.Helpers;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using AutoMapper;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MovinderAPI.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly MovinderContext _context;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public UserController(
            MovinderContext context, 
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            var usersDtos = _mapper.Map<IList<UserDto>>(users);

            return Ok(new {
                success = true,
                users = usersDtos
            });
        }

        [HttpPost("get")]
        public IActionResult GetById([FromBody]UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userDto.id);

            if (user == null)
            {
                return Ok(new {
                    success = false,
                    error = "User not found"
                });
            }

            return Ok( new {
                success = true,
                user = _mapper.Map<UserDto>(user)
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.name) || string.IsNullOrEmpty(userDto.password))
                return Ok(new {
                    success = false,
                    error = "Name or password empty."
                });
 
            var user = _context.Users.SingleOrDefault(x => x.name == userDto.name);
 
            if (user == null)
                return Ok(new {
                    success = false,
                    error = "No user with this name."
                });

            if (!VerifyPasswordHash(userDto.password, user.passwordHash, user.passwordSalt))
                return Ok(new {
                    success = false,
                    error = "Wrong password."
                });
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
 
            // return basic user info (without password) and token to store client side
            return Ok(new {
                id = user.Id,
                name = user.name,
                city = user.city,
                email = user.email,
                token = tokenString
            });
        }

        [HttpPost("new")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.password))
                return Ok(new {
                    success = false,
                    error = "Email requered."
                });

            var user = _mapper.Map<User>(userDto);
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(userDto.email);
            
            if(!match.Success) {
                return Ok(new {
                    success = false,
                    error = "Bad email fromat"
                });
            }
            
            try
            {
                if (string.IsNullOrWhiteSpace(userDto.password))
                    return Ok(new {
                        success = false,
                        error = "Bad password"
                    });
 
                if (_context.Users.Any(x => x.name == user.name))
                    return Ok(new {
                        success = false,
                        error = "Username " + user.name + " is already taken"
                    });
    
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(userDto.password, out passwordHash, out passwordSalt);

                user.passwordHash = passwordHash;
                user.passwordSalt = passwordSalt;

                _context.Users.Add(user);

                _context.SaveChanges();

                return Ok(new {
                    success = true,
                    name = user.name,
                    city = user.city,
                    email = user.email
                });
            } 
            catch(SystemException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
 
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
 
            return true;
        }
    }
}