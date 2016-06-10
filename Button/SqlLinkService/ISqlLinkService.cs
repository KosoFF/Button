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

    }
}
