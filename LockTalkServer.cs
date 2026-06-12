using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static Socket client1;
        static Socket client2;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("===== CHAT SERVER =====");

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 9999);

            server.Bind(ep);
            server.Listen(2);

            Console.WriteLine("Đang chờ Client 1 kết nối...");
            client1 = server.Accept();
            Gui(client1, "Bạn là Client 1");
            Console.WriteLine("Client 1 đã kết nối");

            Console.WriteLine("Đang chờ Client 2 kết nối...");
            client2 = server.Accept();
            Gui(client2, "Bạn là Client 2");
            Console.WriteLine("Client 2 đã kết nối");

            Gui(client1, "Client 2 đã vào. Bắt đầu chat!");
            Gui(client2, "Client 1 đang online. Bắt đầu chat!");

            Thread t1 = new Thread(() => ChuyenTinNhan(client1, client2, "Client 1"));
            Thread t2 = new Thread(() => ChuyenTinNhan(client2, client1, "Client 2"));

            t1.Start();
            t2.Start();
        }

                static void ChuyenTinNhan(Socket nguoiGui, Socket nguoiNhan, string tenNguoiGui)
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[4096];
        
                    int size = nguoiGui.Receive(data);
        
                    if (size <= 0)
                        break;
        
                    string duLieuMaHoa = Encoding.UTF8.GetString(data, 0, size);
        
                    Console.WriteLine(tenNguoiGui + " đã gửi 1 tin nhắn mã hóa:");
                    Console.WriteLine(duLieuMaHoa);
        
                    Gui(nguoiNhan, duLieuMaHoa);
                }
            }
            catch
            {
                Console.WriteLine(tenNguoiGui + " đã ngắt kết nối.");
            }
        }

        static void Gui(Socket client, string noiDung)
        {
            byte[] data = Encoding.UTF8.GetBytes(noiDung);
            client.Send(data);
        }
    }
}
