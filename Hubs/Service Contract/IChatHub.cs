namespace ChatApplication.Hubs.Service_Contract
{
    public interface IChatHub
    {
        Task SendMessage(int senderId, int receiverId, string message);
        Task NotifyChatDeleted(int senderId, int receiverId, int chatId);
    }
}
