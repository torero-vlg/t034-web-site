using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace T034.Core.Entity.Administration
{
    public class User : Entity
    {
        public User()
        {
        }

        public User(string email, string name, string password)
        {
            Email = email;
            Name = name;

            Salt = GenerateSalt();
            Password = HashPassword(password);
        }

        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Salt { get; set; }
        public virtual ICollection<Role> UserRoles { get; set; }

        public virtual bool InRoles(string roles)
        {
            if (string.IsNullOrEmpty(roles))
            {
                return false;
            }

            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesArray)
            {
                var hasRole = UserRoles.Any(p => string.Compare(p.Code, role, true) == 0);
                if (hasRole)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяем пароль пользователя
        /// </summary>
        /// <param name="password">проверяемый пароль</param>
        /// <returns></returns>
        public virtual bool IsValidPassword(string password)
        {
            var hash = HashPassword(password);
            return hash == Password;
        }

        /// <summary>
        /// Сменить пароль
        /// </summary>
        /// <param name="newPassword">Новый пароль</param>
        public virtual void ChangePassword(string newPassword)
        {
            Salt = GenerateSalt();
            Password = HashPassword(newPassword);
        }

        public override string ToString()
        {
            return $"{Name} [{Email}]";
        }

        /// <summary>
        /// Генерация соли
        /// </summary>
        /// <returns>Соль</returns>
        /// <remarks> по информации с http://stackoverflow.com/questions/4181198/how-to-hash-a-password </remarks>
        private static string GenerateSalt()
        {
            var salt = new byte[16];

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Поиск хэша пароля
        /// </summary>
        /// <param name="password">пароль</param>
        /// <returns>хэш</returns>
        /// <remarks> по информации с http://stackoverflow.com/questions/4181198/how-to-hash-a-password </remarks>
        private string HashPassword(string password)
        {
            var salt = Convert.FromBase64String(Salt);

            var r = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = r.GetBytes(32);
            return Convert.ToBase64String(hash);
        }
    }
}
