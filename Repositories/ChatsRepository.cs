using Microsoft.Data.SqlClient;
using MvcWhatsUp.Models;
using MvcWhatsUp.Repositories.Interfaces;

namespace MvcWhatsUp.Repositories
{
    public class ChatsRepository : IChatsRepository
    {
        private readonly string _connectionString;

        public ChatsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SomerenDBConnection");
        }

        public void AddMessage(Message message)
        {
            string query = "INSERT INTO Messages (SenderUserId, ReceiverUserId, MessageText, SendAt) " +
                              "VALUES (@SenderUserId, @ReceiverUserId, @MessageText, @SendAt)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SenderUserId", message.SenderUserId);
                    command.Parameters.AddWithValue("@ReceiverUserId", message.ReceiverUserId);
                    command.Parameters.AddWithValue("@MessageText", message.MessageText);
                    command.Parameters.AddWithValue("@SendAt", message.SendAt);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {

                        throw new Exception("Something went wrong");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Something went wrong");
                    }
                }
            }
        }

        public List<Message> GetMessages(int senderUserId, int receiverUserId)
        {
            List<Message> messages = new List<Message>();

            string query = @"SELECT MessageId, SenderUserId, ReceiverUserId, MessageText, SendAt 
            FROM Messages 
            WHERE 
                    (SenderUserId = @UserId1 AND ReceiverUserId = @UserId2) 
                OR 
                    (SenderUserId = @UserId2 AND ReceiverUserId = @UserId1) 

            ORDER BY SendAt ASC";

            //TODO - move to separate method
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId1", senderUserId);
                command.Parameters.AddWithValue("@UserId2", receiverUserId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(new Message
                        {
                            MessageId = Convert.ToInt32(reader["MessageId"]),
                            SenderUserId = Convert.ToInt32(reader["SenderUserId"]),
                            ReceiverUserId = Convert.ToInt32(reader["ReceiverUserId"]),
                            MessageText = reader["MessageText"].ToString(),
                            SendAt = Convert.ToDateTime(reader["SendAt"])
                        });
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Something went wrong");
                }
                catch (Exception ex)
                {
                    throw new Exception("Something went wrong");
                }
                return messages;
            }
        }
    }
}
