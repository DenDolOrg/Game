using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace MyGame.Models
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(32);
            }
            byte[] dst = new byte[49];
            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer2, 0, dst, 17, 32);
            return Convert.ToBase64String(dst);
        }
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 49) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[16];
            Buffer.BlockCopy(src, 1, dst, 0, 16);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 17, buffer3, 0, 32);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 1000))
            {
                buffer4 = bytes.GetBytes(32);
            }
            return buffer3.SequenceEqual(buffer4);
        }
    }
}