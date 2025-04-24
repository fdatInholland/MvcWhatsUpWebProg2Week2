namespace MvcWhatsUp.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string MessageText { get; set; }
        public DateTime SendAt { get; set; }

        public Message()
        {

        }

        public Message(int messageId, int senderUserId, int receiverUserId, string messageText, DateTime sendAt)
        {
            MessageId = messageId;
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            MessageText = messageText;
            SendAt = sendAt;
        }
    }
}
