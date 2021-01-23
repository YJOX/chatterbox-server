using System;
using System.Text;
using System.Threading;

namespace ChatterBoxServerNEW.Network
{
    class AnswerHandler
    {
        public static Thread clientThread;
        public static void checkAnswers()
        {
            try
            {
                while (true)
                {
                    Program.client = Program.server.AcceptTcpClient();

                    ClientObject client = new ClientObject(Program.client);
                    HandlersHelper.addClient(client);

                    clientThread = new Thread(new ThreadStart(client.checkAnswersForClient));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
