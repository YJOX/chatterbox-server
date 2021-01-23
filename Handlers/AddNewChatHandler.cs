using ChatterBoxServer;
using System;
using System.Text;

namespace ChatterBoxServerNEW.Handlers
{
    class AddNewChatHandler
    {
        public static void createNewChat(ClientObject client, string usernameFrom, string usernameTo)
        {
            MySQLUtils.createNewChatServer(client, usernameFrom, usernameTo);
        }

        public static void sendNewChatPacket(ClientObject client, string username, string chat_id)
        {
            string createNewChat = "ALLOW" + ":" + username + ":" + chat_id;
            byte[] msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(createNewChat), 6);
            try
            {
                client.ns.Write(msg, 0, msg.Length);
            }
            catch
            {
                // Ничего...
            }
        }

        public static void sendNewChatPacketDeny(ClientObject client)
        {
            string createNewChat = "DENY";
            byte[] msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(createNewChat), 6);
            try
            {
                client.ns.Write(msg, 0, msg.Length);
            }
            catch
            {
                // Ничего...
            }
        }
    }
}
