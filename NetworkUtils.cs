using System;
using System.Text;
using System.Threading;

namespace ChatterBoxServerNEW
{
    class NetworkUtils
    {
        public static void sendPacketClientConfirm(int n)
        {
            string str = "[CONFIRMATION]:" + n;
            byte[] msg = Encoding.UTF8.GetBytes(str);
            Program.ns.Write(msg, 0, msg.Length);
        }

        public static void sendPacketClientNull()
        {
            string str = "[ACCOUNTNULL]";
            byte[] msg = Encoding.UTF8.GetBytes(str);
            Program.ns.Write(msg, 0, msg.Length);
        }

        public static void sendLoginBoolean(string username)
        {
            string str = "[LOGINANSWER]:ALLOW:" + username;
            byte[] msg = Encoding.UTF8.GetBytes(str);
            Program.ns.Write(msg, 0, msg.Length);
        }

        public static void sendNewChatBoolean(string answer, int chat_id)
        {
            string str = "[ADDNEWCHATANSWER]:" + answer + ":" + chat_id;
            byte[] msg = Encoding.UTF8.GetBytes(str);
            Program.ns.Write(msg, 0, msg.Length);
        }

        public static void sendUserChatCount(int count)
        {
            string str = "[USERCHATCOUNT]:" + count;
            byte[] msg = Encoding.UTF8.GetBytes(str);
            Program.ns.Write(msg, 0, msg.Length);
        }

        public static void sendChat(string username, int chat_id)
        {
            string str = "[GETCHATSFROMSERVER]:" + username + ":" + chat_id;
            byte[] msg = Encoding.UTF8.GetBytes(str);
            Program.ns.Write(msg, 0, msg.Length);
        }

        public static void sendMessageByteCount(string message)
        {
            string str = "[GETMESSAGEBYTECOUNT]:" + message;
            byte[] msg = Encoding.UTF8.GetBytes(str);
            int count = msg.Length;
            string str2 = "[GETMESSAGEBYTECOUNT]:" + count;
            Program.ns.Write(msg, 0, msg.Length);
            Thread.Sleep(250);
        }
    }
}
