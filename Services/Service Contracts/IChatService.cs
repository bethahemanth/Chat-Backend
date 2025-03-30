using ChatAppBackend.Models;
namespace ChatApplication.Services.Service_Contracts
{
    public interface IChatService
    {
        string AddChat(Chat chat); // Create a new chat
        string DeleteChat(int chatId); // Delete a chat by its ID
        List<Chat> GetChatsByUser(int userId); // Get all chats for a specific user (either sender or receiver)
        Chat GetChat(int chatId); // Get a specific chat by its ID
    }
}
