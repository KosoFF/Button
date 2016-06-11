using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;
using Button.SqlReference;

namespace Button.SqlLinkService
{
    public class LinkService : ISqlLinkService
    {
        readonly ServiceClient _sqlService = new ServiceClient();
        public async Task<AccountInformation> LoginAsync(string login, string password)
        {
            return await _sqlService.LoginAsync(login, password);
        }

        public async Task<bool> SetPhoto(string user, byte[] photo)
        {
            return await _sqlService.SetPhotoAsync(user, photo);
        }

        public async Task<byte[]> GetPhoto(string user)
        {
            return await _sqlService.GetPhotoAsync(user);
        }

        public async Task<string> GetTitle(string user)
        {
            return await _sqlService.GetTitleAsync(user);
        }

        public async Task<Dictionary<string, string>> GetUsersDictionary()
        {
            return await _sqlService.GetUsersDictionaryAsync();
        }

        public async Task<Dictionary<string, string>> GetDrawbacks()
        {
            return await _sqlService.GetNegativeRepliesAsync();
        }

        public async Task<Dictionary<string, string>> GetBenefits()
        {
            return await _sqlService.GetPositiveRepliesAsync();
        }

        public async Task<bool> AddReplies(string sender, string reciever, bool isBad, Dictionary<string, string> replies)
        {
            return await _sqlService.AddRepliesAsync(sender, reciever, isBad, replies);
        }

        public async Task<Dictionary<string, string>> GetRepliesCount(string recepient)
        {
            return await _sqlService.GetRepliesCountAsync(recepient);
        }
        public async Task<string> GetEmail(string userid)
        {
            return await _sqlService.GetEmailAsync(userid);
        }
        public async Task<string> GetMobilePhone(string userid)
        {
            return await _sqlService.GetMobilePhoneAsync(userid);
        }

        public async Task SetMobilePhone(string userid, string mobilephone)
        {
             await _sqlService.SetMobilePhoneAsync(userid, mobilephone);
        }
        public async Task SetEmail(string userid, string email)
        {
            await _sqlService.SetEmailAsync(userid, email);
        }

        public async Task<Dictionary<string, string>> GetFreeUsersDictionary()
        {
            return await _sqlService.GetFreeUsersDictionaryAsync();
        }

        public async Task<bool> SignUp(AccountInformation account)
        {
            return await _sqlService.SignUpAsync(account);
        }
    }
}
