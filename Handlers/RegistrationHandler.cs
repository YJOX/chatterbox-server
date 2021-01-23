using System.Text;

namespace ChatterBoxServerNEW.Handlers
{
    class RegistrationHandler
    {
        public static void checkRegistrationHandler(string username, string email, string password, ClientObject client)
        {
            EmailUtils.send(username, email, client);
        }

        public static void sendRegistrationPackets(int n, ClientObject client)
        {
            string register = "ALLOW:" + n;
            byte[] msg = Network.HandlersHelper.toByteArray(Encoding.UTF8.GetBytes(register), 0);
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
