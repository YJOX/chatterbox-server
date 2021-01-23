using ChatterBoxServerNEW.Handlers;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatterBoxServerNEW
{
    class ClientObject
    {
        private string username = "";
        private string companionName = "";
        private int id = 0;
        TcpClient client;
        public NetworkStream ns;
        public static Thread loadChats;

        public ClientObject(TcpClient client)
        {
            this.client = client;
        }

        public string getUsername()
        {
            return this.username;
        }

        public void setUsername(string username)
        {
            this.username = username;
        }

        public string getcompanionName()
        {
            return this.companionName;
        }

        public void setcompanionName(string username)
        {
            this.companionName = username;
        }

        public int getID()
        {
            return this.id;
        }

        public void setID(int id)
        {
            this.id = id;
        }

        public void checkAnswersForClient()
        {
            byte[] msg = new byte[1024];
            string decodedString = "";
            int count = 0;
            while (true)
            {
                try
                {
                    ns = client.GetStream();
                    while (client.Connected)
                    {
                        count = ns.Read(msg, 0, msg.Length);
                        decodedString = Encoding.UTF8.GetString(msg, 1, count - 1);

                        if (msg[0] == 11) // Регистрация
                        {
                            try
                            {
                                string[] data = decodedString.Split(":");
                                RegistrationHandler.checkRegistrationHandler(data[0], data[1], data[2], this);
                            }
                            catch
                            {
                                // Ничего...
                            }
                        }
                        else if (msg[0] == 22) // Подтверждение регистрации
                        {
                            try
                            {
                                string[] data = decodedString.Split(":");
                                ConfirmationHandler.checkConfirmationHandler(data[0], data[1], data[2], this);
                            }
                            catch
                            {
                                Console.WriteLine("Ошибка создания пользователя в базе данных.");
                            }
                        }
                        else if (msg[0] == 33) // Авторизация
                        {

                            string[] data = decodedString.Split(":");
                            LoginHandler.checkLoginHandler(data[0], data[1], this);
                        }
                        else if (msg[0] == 44) // Загрузка чатов
                        {
                            setUsername(decodedString);
                            LoadChatsHandler.loadChatsHandler(this);
                        }
                        else if (msg[0] == 55) // Отправка сообщений
                        {

                        }
                        else if (msg[0] == 66) // Приём сообщений
                        {

                        }
                        else if (msg[0] == 77) // Создание нового чата
                        {
                            string[] data = decodedString.Split(":");
                            AddNewChatHandler.createNewChat(this, data[0], data[1]);
                        }

                    }
                    Thread.Sleep(5);
                }
                catch
                {
                    // Ничего...
                }
            }
        }
    }
}
