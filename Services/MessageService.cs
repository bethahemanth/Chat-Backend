﻿using ChatAppBackend.Models;
using ChatApplication.Repositories.Service_Contracts;
using ChatApplication.Services.Service_Contracts;
using System;
using System.Collections.Generic;

namespace ChatApplication.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepo _repo;

        public MessageService(IRepo repo)
        {
            _repo = repo;
        }

        // 1️⃣ Send Message
        public string SendMessage(Message message)
        {
            try
            {
                Query query = new Query()
                {
                    TableName = "messages",
                    Values = new List<List<string>>()
            {
                new List<string>()
                {
                    $"sender_id={message.sender_id}",
                    $"receiver_id={message.receiver_id}",
                    $"message='{message.message.Replace("'", "''")}'", // Escape single quotes in message
                    $"created_at='{DateTime.Now:yyyy-MM-dd HH:mm:ss}'",
                    $"destroy_at='{DateTime.Now.AddHours(1):yyyy-MM-dd HH:mm:ss}'"
                }
            }
                };
                return _repo.InsertRecord(query);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }



        // 2️⃣ Fetch All Messages Between Two Users
        public List<Message> GetAllMessages(int sender_id, int receiver_id)
        {
            try
            {
                var query = new Query()
                {
                    TableName = "messages",
                    Conditions = new List<string>()
                    {
                        $"(sender_id={sender_id} AND receiver_id={receiver_id}) OR (sender_id={sender_id} AND receiver_id={receiver_id})"
                    }
                };
                return _repo.GetRecords<Message>(query);
            }
            catch (Exception ex)
            {
                return new List<Message> { new Message { message = $"Error: {ex.Message}" } };
            }
        }

        // 3️⃣ Update Message Status (Sent, Delivered, Read)
        public string UpdateMessageStatus(int messageId, string status)
        {
            try
            {
                var query = new Query()
                {
                    TableName = "MessageStatus",
                    Values = new List<List<string>>()
                    {
                        new List<string>()
                        {
                            $"status='{status}'",
                            $"status_time='{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}'"
                        }
                    },
                    Conditions = new List<string>() { $"message_id={messageId}" }
                };
                return _repo.UpdateRecord(query);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // 4️⃣ Get Unread Message Count
        public int GetUnreadMessageCount(int userId)
        {
            try
            {
                var query = new Query()
                {
                    TableName = "messages",
                    Conditions = new List<string> { $"receiver_id={userId} AND status='sent'" }
                };
                var messages = _repo.GetRecords<Message>(query);
                return messages.Count;
            }
            catch (Exception ex)
            {
                return -1; // Return -1 for errors
            }
        }

        // 5️⃣ Delete Message (Soft Delete)
        public string DeleteMessage(int messageId)
        {
            try
            {
                var query = new Query()
                {
                    TableName = "messages",
                    Conditions = new List<string> { $"message_id={messageId}" }
                };
                return _repo.DeleteRecord(query);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // 6️⃣ Edit Message message
        public string EditMessage(int messageId, string newContent)
        {
            try
            {
                var query = new Query()
                {
                    TableName = "messages",
                    Values = new List<List<string>>()
                    {
                        new List<string>() { $"message='{newContent}'" }
                    },
                    Conditions = new List<string>() { $"message_id={messageId}" }
                };
                return _repo.UpdateRecord(query);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // 7️⃣ Soft Delete Message
        public string SoftDeleteMessage(int messageId)
        {
            try
            {
                var query = new Query()
                {
                    TableName = "messages",
                    Values = new List<List<string>>()
                    {
                        new List<string>() { $"deleted=true" }
                    },
                    Conditions = new List<string>() { $"message_id={messageId}" }
                };
                return _repo.UpdateRecord(query);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // 8️⃣ Search Messages
        public List<Message> SearchMessages(int userId, string searchQuery)
        {
            try
            {
                var query = new Query()
                {
                    TableName = "messages",
                    Conditions = new List<string>()
                    {
                        $"(sender_id={userId} OR receiver_id={userId}) AND message LIKE '%{searchQuery}%'"
                    }
                };
                return _repo.GetRecords<Message>(query);
            }
            catch (Exception ex)
            {
                return new List<Message> { new Message { message = $"Error: {ex.Message}" } };
            }
        }

        public List<Message> GetMessages(int id)
        {
            Query query = new Query()
            {
                TableName = "messages",
                Conditions = new List<string>()
            };
            query.Conditions.Add($"sender_id={id}");
            return _repo.GetRecords<Message>(query);
        }

        public List<Message> GetInverseMessages(int id)
        {
            Query query = new Query()
            {
                TableName = "messages",
                Conditions = new List<string>()
            };
            query.Conditions.Add($"receiver_id={id}");
            return _repo.GetRecords<Message>(query);
        }

        }
    }
