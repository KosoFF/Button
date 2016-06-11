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
        bool SignUp(AccountInformation account);

        [OperationContract]
        string GetTitle(string user);

        [OperationContract]
        bool SetPhoto(string user, byte[] photo);

        [OperationContract]
        byte [] GetPhoto(string user);

        [OperationContract]
        Dictionary<string, string> GetUsersDictionary();
        [OperationContract]
        AccountInformation Login(string login, string passwordHash);
        [OperationContract]
        Dictionary<string, string> GetPositiveReplies();
        [OperationContract]
        Dictionary<string, string> GetNegativeReplies();

        [OperationContract]
        bool AddReplies(string sender, string reciever, bool isBad, Dictionary<string, string> replies);

        [OperationContract]
        Dictionary<string, string> GetRepliesCount(string recepient);

        [OperationContract]
        string GetMobilePhone(string userid);

        [OperationContract]
        string GetEmail(string userid);

        [OperationContract]
        void SetMobilePhone(string userid, string mobilePhone);

        [OperationContract]

        void SetEmail(string userid, string email);

        [OperationContract]
        Dictionary<string, string> GetFreeUsersDictionary();

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
