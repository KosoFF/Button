using System;

namespace Button.Account
{
    public class Account
    {
        public string AccountId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? RegistrationTime { get; set; }
        
        public string WatchUserId { get; set; }
    }
}