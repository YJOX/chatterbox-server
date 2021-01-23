using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using ChatterBoxServerNEW.Handlers;
using System.Net.Sockets;
using ChatterBoxServerNEW;

namespace ChatterBoxServer
{
    class MySQLUtils
    {
        private static MySqlCommand cmd = null;
        private static MySqlConnection connection = null;
        public static List<int> userIDS = new List<int>(); // Список юзеров (id юзеров), которые есть в чате
        private static string name = ""; // Временная переменная для проверки никнейма
        public static int chat_id = 0; // ID чата при создании нового

        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            string port = "3306";
            string database = "chatterbox";
            string username = "chatterbox";
            string password = "y42hqizxOt9GgHqX";

            return GetDBConnection(host, port, database, username, password);
        }

        public static MySqlConnection GetDBConnection(string host, string port, string database, string username, string password)
        {
            string connString = "server=" + host + ";user=" + username + ";database=" + database + ";port=" + port + ";password=" + password + ";Allow Zero Datetime=true";

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }

        public static void createTable()
        {
            try
            {
                connection = GetDBConnection();
                connection.Open();

                string mysqlCommand = "CREATE TABLE IF NOT EXISTS users" +  // Таблица юзеров
                "(id INT NOT NULL primary key AUTO_INCREMENT," +
                "username VARCHAR(128) NOT NULL," +
                "email VARCHAR(128) NOT NULL," +
                "password VARCHAR(128) NOT NULL," +
                "activated BIT DEFAULT 0" +
                " )";
                cmd = new MySqlCommand(mysqlCommand, connection);
                cmd.ExecuteNonQuery();

                mysqlCommand = "CREATE TABLE IF NOT EXISTS chat" + // Таблица чатов
                "(chat_id INT NOT NULL," +
                "user_id INT NOT NULL" +
                " )";
                cmd = new MySqlCommand(mysqlCommand, connection);
                cmd.ExecuteNonQuery();

                mysqlCommand = "CREATE TABLE IF NOT EXISTS messages" + // Таблица сообщений
                "(message_id INT NOT NULL primary key AUTO_INCREMENT," +
                "message_text VARCHAR(1024) NOT NULL," +
                "chat_id INT NOT NULL," +
                "user_id INT NOT NULL," +
                "date_create TIMESTAMP DEFAULT CURRENT_TIMESTAMP" +
                " )";
                cmd = new MySqlCommand(mysqlCommand, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool createNewUserServer(string username, string email, string password)
        {
            try
            {
                connection = GetDBConnection();
                connection.Open();
                string mysqlCommand = "INSERT INTO users (username, email, password, activated) VALUES (" + "'" + username + "'" + ", " + "'" + email + "'" + ", " + "'" + password + "'" + ", 1)";
                cmd = new MySqlCommand(mysqlCommand, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static bool checkLogin(string username, string password)
        {
            try
            {
                connection = GetDBConnection();
                connection.Open();
                string mysqlCommand = "SELECT password FROM users WHERE username = " + "'" + username + "'";
                cmd = new MySqlCommand(mysqlCommand, connection);
                if (checkRecord(mysqlCommand))
                {
                    string passFromDB = cmd.ExecuteScalar().ToString();
                    if (passFromDB != null)
                    {
                        if (password == passFromDB)
                        {
                            connection.Close();
                            return true;
                        }
                    }
                }
                connection.Close();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return false;
        }

        public static bool checkRecord(string mysqlCommand)
        {
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return false;
        }

        public static void createNewChatServer(ClientObject client, string usernameFrom, string usernameTo)
        {
            try
            {
                connection = GetDBConnection();
                connection.Open();

                int IDFrom = getIDFromName(usernameFrom);
                int IDTo = getIDFromName(usernameTo);

                if (IDFrom == 0 || IDTo == 0)
                {
                    connection.Close();
                    AddNewChatHandler.sendNewChatPacketDeny(client);
                    return;
                }

                int chatID = getMaxChatID() + 1;
                chat_id = chatID;

                string mysqlCommand = "INSERT INTO chat (chat_id, user_id) VALUES (" + chatID + ", " + IDFrom + ")";
                cmd = new MySqlCommand(mysqlCommand, connection);
                cmd.ExecuteNonQuery();

                string mysqlCommand2 = "INSERT INTO chat (chat_id, user_id) VALUES (" + chatID + ", " + IDTo + ")";
                cmd = new MySqlCommand(mysqlCommand2, connection);
                cmd.ExecuteNonQuery();

                connection.Close();

                AddNewChatHandler.sendNewChatPacket(client, usernameTo, chat_id.ToString());
                Console.WriteLine(usernameTo + " " + chat_id.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static int getIDFromName(string username)
        {
            try
            {
                string mysqlCommand = "SELECT id FROM users WHERE username = " + "'" + username + "'";
                cmd = new MySqlCommand(mysqlCommand, connection);
                if (checkRecord(mysqlCommand))
                {
                    int IDFromDB = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    if (IDFromDB != 0)
                    {
                        return IDFromDB;
                    }
                }
            } catch
            {
                return 0;
            }
            return 0;
        }

        public static string getNameFromID(int ID)
        {
            try
            {
                string mysqlCommand = "SELECT username FROM users WHERE id = " + ID;
                cmd = new MySqlCommand(mysqlCommand, connection);
                if (checkRecord(mysqlCommand))
                {
                    string username = cmd.ExecuteScalar().ToString();
                    if (username != null)
                    {
                        return username;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public static int getMaxChatID()
        {
            try
            {
                string mysqlCommand = "SELECT max(chat_id) FROM chat";
                cmd = new MySqlCommand(mysqlCommand, connection);
                if (checkRecord(mysqlCommand))
                {
                    int IDFromDB = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    if (IDFromDB != 0)
                    {
                        return IDFromDB;
                    }
                }
            }
            catch
            {
                return 0;
            }
            return 0;
        }

        public static int getChatCountServer(string username)
        {
            try
            {
                connection = GetDBConnection();
                connection.Open();

                int userID = getIDFromName(username);
                string mysqlCommand = "SELECT chat_id FROM chat WHERE user_id = " + userID;
                cmd = new MySqlCommand(mysqlCommand, connection);

                int count = 0;
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count += 1;
                    }
                }
                reader.Close();
                connection.Close();
                return count;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public static void getChatUsers(ClientObject client)
        {
            try
            {
                string mysqlCommand = "";
                for (int i = 0; i < LoadChatsHandler.chatIDS.Count; i++)
                {
                    mysqlCommand = "SELECT user_id FROM chat WHERE chat_id = " + LoadChatsHandler.chatIDS[i];
                    cmd = new MySqlCommand(mysqlCommand, connection);

                    MySqlDataReader reader;
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userIDS.Add(reader.GetInt32("user_id"));
                        }
                    }
                    reader.Close();
                }

                for (int i = 0; i < userIDS.Count; i++)
                {
                    string username = getNameFromID(userIDS[i]);

                    if (username != name)
                    {
                        LoadChatsHandler.chatUsernames.Add(username);
                    }
                }
                connection.Close();
                LoadChatsHandler.sendChatsToClient(client);
            }
            catch
            {
                return;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return;
        }

        public static void getChats(ClientObject client)
        {
            try
            {
                LoadChatsHandler.chatIDS.Clear();
                userIDS.Clear();
                name = client.getUsername();
                connection = GetDBConnection();
                connection.Open();

                int userID = getIDFromName(name);
                string mysqlCommand = "SELECT chat_id FROM chat WHERE user_id = " + userID;
                cmd = new MySqlCommand(mysqlCommand, connection);

                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LoadChatsHandler.chatIDS.Add(reader.GetInt32("chat_id"));
                    }
                }
                reader.Close();
                getChatUsers(client);
            }
            catch
            {
                return;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return;
        }

        public static void saveMessage(string message, DateTime time, int chat_id, int user_id)
        {

        }
    }
}