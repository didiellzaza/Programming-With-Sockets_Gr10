using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPClient
{
    private const int Port = 6000;
    private static string Host = "172.20.10.2";

    static void Main(string[] args)
    {
        UdpClient client = new UdpClient();
        IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(Host), Port);

        ///
        client.Close();
    }
}
