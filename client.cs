using System.IO;
using System.Net.Sockets;
using System;

namespace Rrjeta
{
    public class EchoClient
    {
        public void StartClient(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 8080);
                reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                string s = string.Empty;
                while (!s.Equals("Exit", StringComparison.Ordinal))
                {
                    Console.Write("Futni një string për ta dërguar te serveri: ");
                    s = Console.ReadLine();
                    Console.WriteLine();
                    writer.WriteLine(s);
                    writer.Flush();
                    string server_string = reader.ReadLine();
                    Console.WriteLine(server_string);
                }
                reader.Close();
                writer.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }
    }
}

