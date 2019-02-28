using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace YunWeiPingTai.Custom
{
    public class MyUserValidator<TUser> : IUserValidator<TUser> where TUser : class
    {
        const string chinese = "{中}";

        public IdentityErrorDescriber Describer { get; private set; }
        public MyUserValidator(IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var errors = new List<IdentityError>();
            await ValidateUserName(manager, user, errors);
            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
        private async Task ValidateUserName(UserManager<TUser> manager, TUser user, ICollection<IdentityError> errors)
        {
            var userName = await manager.GetUserNameAsync(user);
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(Describer.InvalidUserName(userName));
            }
            var characters = manager.Options.User.AllowedUserNameCharacters;
            bool allowChinese = false;
            if (characters.Contains(chinese))
            {
                allowChinese = true;
                characters = characters.Remove(characters.IndexOf(chinese), chinese.Length);
            }
            if (ContainsChinese(userName) && !allowChinese)
            {
                errors.Add(Describer.InvalidUserName(userName));
            }
            var tempName = RemoveChinese(userName);
            if (!string.IsNullOrEmpty(characters) && tempName.Any(c => !characters.Contains(c)))
            {
                errors.Add(Describer.InvalidUserName(userName));
            }
            var owner = await manager.FindByNameAsync(userName);
            if (owner != null &&
                !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
            {
                errors.Add(Describer.DuplicateUserName(userName));
            }
        }
        //判断字符串是否包含汉字
        private bool ContainsChinese(string text)
        {
            return text.Any(c => c >= 0x4e00 && c <= 0x9fbb);
        }

        //移除字符串中的汉字
        private string RemoveChinese(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c >= 0x4e00 && c <= 0x9fbb)
                {
                    continue;
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
