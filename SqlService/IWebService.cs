using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SqlService
{
   
        // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
        [ServiceContract]
        public interface IWebService
        {
            [OperationContract]
            List<user> GetUsers();
            // TODO: Добавьте здесь операции служб
        }

    
}
