namespace Services
{
    using Dtos;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public static class Security
    {

        #region Propiedades
        private const string UserValue = "User";
        #endregion
        #region Metodos y funciones
        /// <summary>
        /// Genera un token que expira al finalizar el dia
        /// </summary>
        /// <param name="login"></param>
        /// <param name="apiAuth"></param>
        /// <returns></returns>
        public static JwtSecurityToken GenerateToken(UserDto login, string[] apiAuth, TokenExpiresEnum tokenExpires)
        {
            string ValidIssuer = apiAuth[0];
            string ValidAudience = apiAuth[1];
            SymmetricSecurityKey IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiAuth[2]));

            DateTime dtDateExpirationToken = new DateTime();
            DateTime now = DateTime.Now;
            switch (tokenExpires)
            {
                case TokenExpiresEnum.login:
                    //La fecha de expiracion sera el mismo dia a las 12 de la noche
                    dtDateExpirationToken = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);
                    break;
                case TokenExpiresEnum.confirmation:
                    dtDateExpirationToken = now.AddMinutes(25);
                    break;
                default:
                    break;
            }
            Claim[] claims = new[]
            {
                new Claim(UserValue, login.Username)
            };
            return new JwtSecurityToken
            (
                issuer: ValidIssuer,
                audience: ValidAudience,
                claims: claims,
                notBefore: now,
                expires: dtDateExpirationToken,
                signingCredentials: new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        /// <summary>
        /// Retorna true si el token aun es valido, de lo contrario retorna false
        /// </summary>
        /// <param name="token"></param>
        /// <param name="apiAuth"></param>
        /// <returns></returns>
        public static bool ValidateCurrentToken(string token, string[] apiAuth)
        {
            string ValidIssuer = apiAuth[0];
            string ValidAudience = apiAuth[1];
            SymmetricSecurityKey IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiAuth[2]));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = ValidIssuer,
                    ValidAudience = ValidAudience,
                    IssuerSigningKey = IssuerSigningKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Encripta en base 64 un texto plano
        /// </summary>
        /// <param name="clearText"></param>
        /// <param name="EncryptionKey"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
                {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
            return clearText;
        }

        /// <summary>
        /// Desencripta de base 64 un texto plano
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="EncryptionKey"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string EncryptionKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
                {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
            return cipherText;
        }
        #endregion
    }
}
