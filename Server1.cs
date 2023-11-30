
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SServer
{
    public class Server
    {
        public static void StartServer()
        {
            Console.WriteLine("Server është duke pritur për lidhje...");
            UdpClient client = new(8080);

            IPEndPoint remoteip = new IPEndPoint(IPAddress.Any, 8080);

            byte[] receivedbytes = client.Receive(ref remoteip);

            if (receivedbytes != null)
            {
                string message = Encoding.ASCII.GetString(receivedbytes);
                Console.WriteLine("mesazhi i pranuar " + message);
            }
            else
            {
                Console.WriteLine("Mesazhi i pranuar është i zbrazët!");
            }

            Console.ReadLine();
        }
    }
}
