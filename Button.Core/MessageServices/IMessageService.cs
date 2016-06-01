using System.Collections.Generic;
using System.Threading.Tasks;

namespace Button.Core.MessageServices
{
    public interface IMessageService
    {
        Task ShowAsync(string title, string message);

        Task ShowAsync(string title, string message, IEnumerable<DialogCommand> commands);
    }
}