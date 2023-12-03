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

        Console.Write("Enter credentials (username:password): ");
        string credentials = Console.ReadLine();

        if (credentials != null)
        {
            byte[] credentialsBytes = Encoding.ASCII.GetBytes(credentials);
            client.Send(credentialsBytes, credentialsBytes.Length, serverEndpoint);

            byte[] authResponseBytes = client.Receive(ref serverEndpoint);
            string authResponse = Encoding.ASCII.GetString(authResponseBytes);
            Console.WriteLine("Server response: " + authResponse);

            if (authResponse == "Authentication successful. Access granted.")
            {
                bool isReadOnly = credentials == "dina:pirana";

                while (true)
                {
                    if (isReadOnly)
                    {
                        Console.Write("You are in read-only mode. Enter 'read <file_name>' or type 'exit' to end: ");
                    }
                    else
                    {
                        Console.Write("Enter a command ('read <file_name>', 'write <string> <file_name>', 'delete <file_name>' or type 'exit' to end): ");
                    }

                    string command = Console.ReadLine();

                    if (command == "exit")
                    {
                        const string exitMessage = "Client disconnected.";
                        byte[] exitMessageBytes = Encoding.ASCII.GetBytes(exitMessage);
                        client.Send(exitMessageBytes, exitMessageBytes.Length, serverEndpoint);
                        break;
                    }

                    byte[] commandBytes = Encoding.ASCII.GetBytes(command);
                    client.Send(commandBytes, commandBytes.Length, serverEndpoint);

                    byte[] responseBytes = client.Receive(ref serverEndpoint);
                    string response = Encoding.ASCII.GetString(responseBytes);
                    Console.WriteLine("Server Response: " + response);

///
                }
            }
        }
        client.Close();
    }
}
