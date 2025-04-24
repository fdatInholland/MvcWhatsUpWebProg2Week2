namespace MvcWhatsUp.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public User()
        {
                
        }

        public User(int userID, string userName, string mobileNumber, string email, string password)
        {
            UserID = userID;
            UserName = userName;
            MobileNumber = mobileNumber;
            EmailAddress = email;
            Password = password;
        }
    }
}
