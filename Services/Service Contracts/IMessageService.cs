using ChatAppBackend.Models;

namespace ChatApplication.Services.Service_Contracts
{
    public interface IMessageService
    {
        // 1️⃣ Send Message
        string SendMessage(Message message);

        // 2️⃣ Get All Messages Between Two Users
        List<Message> GetAllMessages(int senderId, int receiverId);

        // 3️⃣ Update Message Status (Sent, Delivered, Read)
        string UpdateMessageStatus(int messageId, string status);

        // 4️⃣ Get Unread Message Count
        int GetUnreadMessageCount(int userId);

        // 5️⃣ Delete Message
        string DeleteMessage(int messageId);

        // 6️⃣ Edit Message message
        string EditMessage(int messageId, string newContent);

        // 7️⃣ Soft Delete Message (Mark as deleted)
        string SoftDeleteMessage(int messageId);

        // 8️⃣ Search Messages by Keyword
        List<Message> SearchMessages(int userId, string searchQuery);

        List<Message> GetMessages(int id);

        List<Message> GetInverseMessages(int id);

        List<Message> GetGroupMessage(int id);

    }
}
