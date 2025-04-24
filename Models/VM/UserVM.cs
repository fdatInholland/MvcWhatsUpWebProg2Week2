namespace MvcWhatsUp.Models.VM
{
    public class UserVM
    {
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }


        public UserVM(string userName, string mobileNumber, string emailAddress)
        {
            UserName = userName;
            MobileNumber = mobileNumber;
            EmailAddress = emailAddress;
        }
    }
}
