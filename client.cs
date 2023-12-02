using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPClient
{
    private const int Port = 6000;
    private static string Host = "192.168.0.18"; // IP adresa e serverit

    static void Main(string[] args)
    {
        UdpClient client = new UdpClient();
        IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(Host), Port);

        Console.Write("Shtyp kredencialet (username:password): ");
        string credentials = Console.ReadLine();

        //

        client.Close();
    }
}
