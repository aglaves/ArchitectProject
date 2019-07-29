using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ArchitectProject.Security
{
    public static class JwtSecurityKey
    {
        public static Boolean UseDynamicToken{ get; set; } = true;
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
