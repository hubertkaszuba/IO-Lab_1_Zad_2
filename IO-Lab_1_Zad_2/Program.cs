using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace IO_Lab_1_Zad_2
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc_server, new object[] { 1000 });
            ThreadPool.QueueUserWorkItem(ThreadProc_client, new object[] { 1 });
            ThreadPool.QueueUserWorkItem(ThreadProc_client, new object[] { 2 });
            Thread.Sleep(5000);
        }

        static void ThreadProc_client(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            var nr = ((object[])stateInfo)[0];
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc od watku nr: " + nr);
            
            client.GetStream().Write(message, 0, message.Length);
            client.Close();
        }

        static void ThreadProc_server(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
        
            server.Start();
            while (true)
            {
                TcpClient  client = server.AcceptTcpClient();
                byte[] buffer = new byte[1024];
                int size = client.GetStream().Read(buffer, 0, 1024);
                client.GetStream().Read(buffer, 0, 1024);
                Console.WriteLine("Polaczono");
                Console.WriteLine(new ASCIIEncoding().GetString(buffer, 0, size));
                client.GetStream().Write(buffer, 0, buffer.Length);
                client.Close();
            }
        }

    }
}
