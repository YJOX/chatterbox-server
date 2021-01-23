using ChatterBoxServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatterBoxServerNEW
{
    class Program
    {
        public static NetworkStream ns;
        public static TcpListener server;
        public static Thread checkAnswersThread;
        public static TcpClient client;

        static void Main(string[] args)
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 1337);
                server.Start();
                Console.WriteLine("Сервер запущен");
                MySQLUtils.createTable();

                checkAnswersThread = new Thread(new ThreadStart(Network.AnswerHandler.checkAnswers));
                checkAnswersThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
