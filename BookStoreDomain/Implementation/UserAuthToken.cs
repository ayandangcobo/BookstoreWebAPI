using BookStoreDomain.Contracts;
using BookStoreDomain.Models;
using System;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Security.Principal;
using System.Security.Claims;

namespace BookStoreDomain.Implementation
{
    public class UserAuthToken : IUserAuthToken
    {
        public string GenerateAuthToken(UserModel user, List<string> roles)
        {
            var response = new RetVal<string>();

            int expiry = 30; //default
            int.TryParse(ConfigurationManager.AppSettings["jwt_expiry"], out expiry);
            string issuer = ConfigurationManager.AppSettings["jwt_bookStoreKey"];
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["jwt_bookStoreKey"]); //Secret key which will be used later during validation       

            //Create a List of Claims    
            var claims = new List<Claim>();
            claims.Add(new Claim("userid", user.UserId.ToString()));
            claims.Add(new Claim("name", user.AuthorPseudonym));

            if (roles.Any())
            {
                roles.ForEach(r =>
                {
                    claims.Add(new Claim("role", r));
                });
            }

            //Create Security Token object by giving required parameters  
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiry),
                //Issuer = issuer,
                //Audience = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetClaimValue(IPrincipal user, string claimName)
        {
            var claims = GetCurrentUserClaims(user);
            return claims.Where(x => x.Type == claimName).FirstOrDefault()?.Value;
        }

        private IEnumerable<Claim> GetCurrentUserClaims(IPrincipal user)
        {
            IEnumerable<Claim> claims = new List<Claim>();
            var identity = user.Identity as ClaimsIdentity;
            if (identity != null)
            {
                claims = identity.Claims;
            }
            return claims;
        }
    }

}
