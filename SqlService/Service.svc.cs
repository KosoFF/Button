using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
        public bool SignUp(AccountInformation acc)
        {

            try
            {
                using (var context = new courseworkEntities())
                {
                    if (acc == null)
                        return true;
                    var user = context.user.First(a => a.UserID == acc.UserId);
                    if (user != null)
                    {
                        var account = new account()
                        {
                            AccountID = Guid.NewGuid().ToString("D"),
                            Email = acc.Email,
                            PasswordHash = acc.PasswordHash,
                            RegistrationTime = DateTime.Now,
                            EmailConfirmed = false,
                            Username = acc.Username
                        };
                        user.AccountID = account.AccountID;
                        context.account.AddOrUpdate(account);
                        context.user.AddOrUpdate(user);
                        context.SaveChanges();
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.Login: " + ex.Message);
                return true;
            }
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
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.GetPhoto: " + ex.Message);
                return true;
            }
        }

        public Dictionary<string, string> GetUsersDictionary()
        {
            try
            {
                using (var c = new courseworkEntities())
                {

                    return c.user.ToDictionary(user => user.UserID,(user) => (user.IsStudent ? "Студент:" : "Преподаватель:") +
                                    user.LastName + " " + user.FirstName + " " + user.MiddleName);
                    
                }
            }
            catch (Exception e)
            {
                Debug.Print("GetUsersDictionaryError:" + e.Message);
                return null;
            }
        }

        public Dictionary<string, string> GetFreeUsersDictionary()
        {
            try
            {
                using (var c = new courseworkEntities())
                {

                    return c.user.Where(a=>a.AccountID==null).ToDictionary(user => user.UserID, (user) => (user.IsStudent ? "Студент:" : "Преподаватель:") +
                                     user.LastName + " " + user.FirstName + " " + user.MiddleName);

                }
            }
            catch (Exception e)
            {
                Debug.Print("GetUsersDictionaryError:" + e.Message);
                return null;
            }
        }

        public Dictionary<string, string> GetPositiveReplies()
        {

            try
            {
                using (var c = new courseworkEntities())
                {

                    return c.replyCollection.Where(t=>t.IsBad==false).ToDictionary(t=>t.ReplyCollectionID, t=>t.Text);

                }
            }
            catch (Exception e)
            {
                Debug.Print("RepliesError:" + e.Message);
                return null;
            }
        }
        public Dictionary<string, string> GetNegativeReplies()
        {

            try
            {
                using (var c = new courseworkEntities())
                {

                    return c.replyCollection.Where(t => t.IsBad).ToDictionary(t => t.ReplyCollectionID, t => t.Text);

                }
            }
            catch (Exception e)
            {
                Debug.Print("RepliesError:" + e.Message);
                return null;
            }
        }

        public bool AddReplies(string sender, string reciever, bool isBad, Dictionary<string, string> replies)
        {
            try
            {
                using (var context = new courseworkEntities())
                {
                    context.reply.RemoveRange(context.reply.Where(a => a.Recipient == reciever && a.Sender == sender && a.replyCollection.IsBad==isBad));
                    context.reply.AddRange(replies.Select(a=>new reply()
                    {
                        ReplyID = Guid.NewGuid().ToString("D"),
                        ReplyCollectionID = a.Key,
                        Recipient = reciever,
                        Sender = sender
                    }));
                    context.SaveChanges();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.GetPhoto: " + ex.Message);
                return true;
            }
        }

        public Dictionary<string, string> GetRepliesCount(string recepient)
        {
            try
            {
                using (var c = new courseworkEntities())
                {

                   var t = c.reply.Where(reply => reply.Recipient == recepient).GroupBy(reply => reply.ReplyCollectionID)
                        .Select(group => new {
                            Key = c.replyCollection.FirstOrDefault(a=>a.ReplyCollectionID==group.Key).Text,
                            Value = group.Count().ToString()
                            }).ToDictionary(temp=>temp.Key, temp=>temp.Value);
                    return t;

                }
            }
            catch (Exception e)
            {
                Debug.Print("RepliesError:" + e.Message);
                return null;
            }
        }

        public string GetMobilePhone(string userid)
        {
            try
            {
                using (var c = new courseworkEntities())
                {

                    return c.user.FirstOrDefault(u => u.UserID == userid)?.details.PhoneNumber;
                }
            }
            catch (Exception e)
            {
                Debug.Print("RepliesError:" + e.Message);
                return null;
            }
        }


        public string GetEmail(string userid)
        {
            try
            {
                using (var c = new courseworkEntities())
                {

                    return c.user.FirstOrDefault(u => u.UserID == userid)?.details.ConnectionEmail;
                }
            }
            catch (Exception e)
            {
                Debug.Print("RepliesError:" + e.Message);
                return null;
            }
        }


        public void SetMobilePhone(string userid, string mobilePhone)
        {
            try
            {
                using (var c = new courseworkEntities())
                {
                   var t =  c.user.FirstOrDefault(u => u.UserID == userid)?.details;
                    t.PhoneNumber = mobilePhone;
                    c.details.AddOrUpdate(t);
                    c.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.Print("RepliesError:" + e.Message);
                
            }
        }


        public void SetEmail(string userid, string email)
        {
            try
            {
                using (var c = new courseworkEntities())
                {
                    var t = c.user.FirstOrDefault(u => u.UserID == userid)?.details;
                    t.ConnectionEmail = email;
                    c.details.AddOrUpdate(t);
                    c.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.Print("RepliesError:" + e.Message);

            }
        }
        

    }
}
