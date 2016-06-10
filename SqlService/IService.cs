using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SqlService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService
    {

        #region Login
        [OperationContract]
        bool SignUp(account manager);

        [OperationContract]
        string GetTitle(string user);

        [OperationContract]
        bool SetPhoto(string user, byte[] photo);

        [OperationContract]
        byte [] GetPhoto(string user);

        [OperationContract]
        AccountInformation Login(string login, string passwordHash);
        #endregion //login
    }


    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.
    [DataContract]
    public class AccountInformation
    {
        [DataMember]
        public string AccountID { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string PasswordHash { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Nullable<bool> EmailConfirmed { get; set; }
        [DataMember]
        public Nullable<System.DateTime> RegistrationTime { get; set; }

        [DataMember]
        public string UserId { get; set; }
    }
}
