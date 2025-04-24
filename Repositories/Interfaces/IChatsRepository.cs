using MvcWhatsUp.Models;

namespace MvcWhatsUp.Repositories.Interfaces
{
    public interface IChatsRepository
    {
        void AddMessage(Message message);
        List<Message> GetMessages(int senderUserId, int receiverUserId);
    }
}
