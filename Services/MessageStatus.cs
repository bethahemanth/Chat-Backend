using ChatAppBackend.Models;
using ChatApplication.Repositories.Service_Contracts;
using ChatApplication.Services.Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApplication.Services
{
    public class MessageStatusService : IMessageStatus
    {
        private readonly IRepo _repo;

        public MessageStatusService(IRepo repo)
        {
            _repo = repo;
        }

        public string AddMessageStatus(MessageStatus messageStatus)
        {
            var query = new Query()
            {
                TableName = "message_status",
                Values = new List<List<string>>()
                {
                    new List<string>()
                    {
                        $"message_id='{messageStatus.MessageId}'",
                        $"status='{messageStatus.Status}'",
                        $"status_time='{messageStatus.StatusTime:yyyy-MM-dd HH:mm:ss}'"
                    }
                }
            };

            return _repo.InsertRecord(query);
        }

        public string UpdateMessageStatus(int messageStatusId, string status)
        {
            var query = new Query()
            {
                TableName = "message_status",
                Values = new List<List<string>>()
                {
                    new List<string>()
                    {
                        $"status='{status}'",
                        $"status_time='{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}'"
                    }
                },
                Conditions = new List<string> { $"message_status_id={messageStatusId}" }
            };

            return _repo.UpdateRecord(query);
        }

        public string DeleteMessageStatus(int messageStatusId)
        {
            var query = new Query()
            {
                TableName = "message_status",
                Conditions = new List<string> { $"message_status_id={messageStatusId}" }
            };

            return _repo.DeleteRecord(query);
        }

        public MessageStatus GetMessageStatus(int messageStatusId)
        {
            var query = new Query()
            {
                TableName = "message_status",
                Conditions = new List<string> { $"message_status_id={messageStatusId}" }
            };

            return _repo.GetRecords<MessageStatus>(query).FirstOrDefault();
        }

        public List<MessageStatus> GetAllMessageStatuses()
        {
            var query = new Query()
            {
                TableName = "message_status"
            };

            return _repo.GetRecords<MessageStatus>(query);
        }
    }
}
