using ChatterBoxServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChatterBoxServerNEW.Handlers
{
    class LoadChatsHandler
    {
        public static List<string> chatUsernames = new List<string>();
        public static List<int> chatIDS = new List<int>();
        public static void loadChatsHandler(ClientObject client)
        {
            MySQLUtils.getChats(client);
        }

        public static void sendChatsToClient(ClientObject client)
        {
            string chat = "";
            byte[] msg = { };
            for (int i = 0; i < chatUsernames.Count; i++)
            {
                chat = chatUsernames[i] + ":" + chatIDS[i];
                msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(chat), 3);
                try
                {
                    client.ns.Write(msg, 0, msg.Length);
                    Thread.Sleep(500);
                }
                catch
                {
                    // Ничего...
                }
            }
        }
    }
}
