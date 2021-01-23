using ChatterBoxServer;
using System;
using System.Net.Sockets;
using System.Text;

namespace ChatterBoxServerNEW.Handlers
{
    class LoginHandler
    {
        public static void checkLoginHandler(string username, string password, ClientObject client)
        {
            bool checkLogin = MySQLUtils.checkLogin(username, password);
            if (checkLogin)
            {
                sendLoginPacketsAllow(client);
            }
            else
            {
                sendLoginPacketsDeny(client);
            }
        }

        public static void sendLoginPacketsAllow(ClientObject client)
        {
            string register = "ALLOW";
            byte[] msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(register), 2);
            try
            {
                client.ns.Write(msg, 0, msg.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void sendLoginPacketsDeny(ClientObject client)
        {
            string register = "DENY";
            byte[] msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(register), 2);
            try
            {
                client.ns.Write(msg, 0, msg.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
