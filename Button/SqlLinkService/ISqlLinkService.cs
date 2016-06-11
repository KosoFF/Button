using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Button.SqlReference;

namespace Button.SqlLinkService
{
    public interface ISqlLinkService
    {
        Task<AccountInformation> LoginAsync(string login, string password);
        Task<bool> SetPhoto(string user, byte[] photo);

        Task<byte[]> GetPhoto(string user);


        Task<string> GetTitle(string user);
        Task<Dictionary<string, string>> GetUsersDictionary();

        Task<bool> AddReplies(string sender, string reciever, bool isBad, Dictionary<string, string> replies);
        Task<Dictionary<string, string>> GetDrawbacks();
        Task<Dictionary<string, string>> GetBenefits();

        Task<Dictionary<string, string>> GetRepliesCount(string recepient);

        Task<string> GetEmail(string userid);

        Task<string> GetMobilePhone(string userid);

        Task SetMobilePhone(string userid, string mobilephone);

        Task SetEmail(string userid, string email);
        Task<Dictionary<string, string>> GetFreeUsersDictionary();

        Task<bool> SignUp(AccountInformation account);


    }
}
