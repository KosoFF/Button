using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
