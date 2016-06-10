using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SqlService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService, IWebService
    {
        #region REST

        public List<user> GetUsers()
        {
            throw new NotImplementedException();
        } 

        #endregion

        #region Login
        public bool SignUp(account userInsert)
        {
            throw new NotImplementedException();

            //using (var authData = new AirportEntities())
            //{
            //    try
            //    {
            //        var newUser = userInsert;
            //        if (authData.Check_in.Any(c => c.Login == newUser.Login /* || c.UserName == newUser.UserName*/))
            //        {
            //            return false;
            //        }


            //        newUser.ManagerID = Guid.NewGuid().ToString("D");
            //        //TODO: Make guid check?..
            //        authData.Check_in.Add(newUser);
            //        //authData.SaveChanges();
            //        authData.SaveChanges();
            //    }
            //    catch
            //    {
            //        return false;
            //    }
            //}
            //return true;

        }


        public AccountInformation Login(string login, string passwordHash)
        {
            try
            {
                using (var context = new courseworkEntities())
                {
                    var account = context.account.SingleOrDefault(a => a.Username == login);
                    if (account != null)
                    {
                        if (account.PasswordHash == passwordHash)
                            return new AccountInformation()
                            {
                                AccountID = account.AccountID,
                                Email = account.Email,
                                EmailConfirmed = account.EmailConfirmed,
                                PasswordHash = account.PasswordHash,
                                RegistrationTime = account.RegistrationTime,
                                Username = account.Username,
                                UserId = account.user.FirstOrDefault(a=>a.AccountID==account.AccountID)?.UserID
                            };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.Login: " + ex.Message);
                return null;
            }
        }
        #endregion  //Login

        public string GetTitle(string user)
        {
            try
            {
                using (var context = new courseworkEntities())
                {
                    var t = context.user.FirstOrDefault(a => a.UserID == user);
                    if (t == null)
                        return "User id error";
                    return t.LastName + " " + t.FirstName + " " +t.MiddleName;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.GetTitle: " + ex.Message);
                return null;
            }
        }

        public byte[] GetPhoto(string user)
        {
            try
            {
                using (var context = new courseworkEntities())
                {
                    var t = context.user.First(a => a.UserID == user);
                    return t.ProfilePhoto;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.GetPhoto: " + ex.Message);
                return null;
            }
        }
        public bool SetPhoto(string user,byte [] photo)
        {
            try
            {
                using (var context = new courseworkEntities())
                {
                     context.user.First(a => a.UserID == user).ProfilePhoto = photo;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.GetPhoto: " + ex.Message);
                return false;
            }
        }

    }
}
