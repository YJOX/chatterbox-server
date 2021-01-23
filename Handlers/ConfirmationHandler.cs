using ChatterBoxServer;
using System.Text;

namespace ChatterBoxServerNEW.Handlers
{
    class ConfirmationHandler
    {
        public static void checkConfirmationHandler(string username, string email, string password, ClientObject client)
        {
            bool isCreated = MySQLUtils.createNewUserServer(username, email, password);
            if (isCreated)
            {
                sendRegistrationPacketsAllow(client);
            }
            else
            {
                sendRegistrationPacketsDeny(client);
            }
        }

        private static void sendRegistrationPacketsAllow(ClientObject client)
        {
            string register = "ALLOW";
            byte[] msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(register), 1);
            try
            {
                client.ns.Write(msg, 0, msg.Length);
            }
            catch
            {
                // Ничего...
            }
        }

        private static void sendRegistrationPacketsDeny(ClientObject client)
        {
            string register = "DENY";
            byte[] msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(register), 1);
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