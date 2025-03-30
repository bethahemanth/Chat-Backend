using System;
using System.Collections.Generic;
using System.Linq;
using ChatAppBackend.Models;
using ChatApplication.Repositories.Service_Contracts;
using ChatApplication.Services.Service_Contracts;

namespace ChatApplication.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepo _repo;

        public ChatService(IRepo repo)
        {
            _repo = repo;
        }

        // 1️⃣ Add a new chat record
        public string AddChat(Chat chat)
        {
            var query = new Query()
            {
                TableName = "chat",
                Values = new List<List<string>>()
                {
                    new List<string>()
                    {
                        $"sender_id='{chat.SenderId}'",
                        $"receiver_id='{chat.receiver_id}'",
                        $"message_id='{chat.MessageId}'"
                    }
                }
            };

            return _repo.InsertRecord(query);
        }

        // 2️⃣ Delete a chat record by its ID
        public string DeleteChat(int chatId)
        {
            var query = new Query()
            {
                TableName = "chat",
                Conditions = new List<string> { $"chat_id='{chatId}'" }
            };

            return _repo.DeleteRecord(query);
        }

        // 3️⃣ Get all chats for a specific user (either sender or receiver)
        public List<Chat> GetChatsByUser(int userId)
        {
            var query = new Query()
            {
                TableName = "chat",
                Conditions = new List<string> { $"sender_id='{userId}' OR receiver_id='{userId}'" }
            };

            return _repo.GetRecords<Chat>(query);
        }

        // 4️⃣ Get a specific chat by its ID
        public Chat GetChat(int chatId)
        {
            var query = new Query()
            {
                TableName = "chat",
                Conditions = new List<string> { $"chat_id='{chatId}'" }
            };

            return _repo.GetRecords<Chat>(query).FirstOrDefault();
        }
    }
}
