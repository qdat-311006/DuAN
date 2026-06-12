using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatClient
{
    class Program
    {
        static Socket client;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("===== CHAT CLIENT =====");

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.Write("Nhập IP Server: ");
            string ip = Console.ReadLine();

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), 9999);

            client.Connect(ep);

            Console.WriteLine("Kết nối Server thành công!");

            Thread threadNhan = new Thread(NhanTinNhan);
            threadNhan.Start();

            while (true)
            {
                Console.Write("Bạn: ");
                string tinNhan = Console.ReadLine();

                if (tinNhan.ToLower() == "exit")
                    break;

                byte[] data = Encoding.UTF8.GetBytes(tinNhan);
                client.Send(data);
            }

            client.Close();
        }

        static void NhanTinNhan()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[4096];

                    int size = client.Receive(data);

                    if (size <= 0)
                        break;

                    string tinNhan = Encoding.UTF8.GetString(data, 0, size);

                    Console.WriteLine("\n" + tinNhan);
                    Console.Write("Bạn: ");
                }
            }
            catch
            {
                Console.WriteLine("\nMất kết nối Server.");
            }
        }
    }
}
