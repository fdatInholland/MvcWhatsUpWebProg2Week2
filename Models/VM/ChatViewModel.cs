
namespace MvcWhatsUp.Models.VM
{
    public class ChatViewModel
    {
        public List<Message> messages { get; set; }
        public User SendingUser { get; set; }
        public User ReceivingUser { get; set; }

        public ChatViewModel(List<Message> messages, User sendingUser, User receivingUser)
        {
            this.messages = messages;
            SendingUser = sendingUser;
            ReceivingUser = receivingUser;
        }
    }
}
