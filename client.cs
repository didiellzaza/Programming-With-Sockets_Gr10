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

        if (credentials != null)
        {
            byte[] credentialsBytes = Encoding.ASCII.GetBytes(credentials);
            client.Send(credentialsBytes, credentialsBytes.Length, serverEndpoint);

            byte[] authResponseBytes = client.Receive(ref serverEndpoint);
            string authResponse = Encoding.ASCII.GetString(authResponseBytes);
            Console.WriteLine("Pergigjja e serverit: " + authResponse);

            if (authResponse == "Autentikimi i suksesshem. Lidhja u mundesua.")
            {
                while (true)
                {
                    Console.Write("Shtyp nje komande ('read <emri_i_fajllit>', 'write <permbajtja> <emri_i_fajllit>', 'execute <komanda>' ose shtyp 'exit' per te perfunduar): ");
                    string command = Console.ReadLine();

                    if (command == "exit")
                    {
                        const string exitMessage = "Klienti u diskonektua.";
                        byte[] exitMessageBytes = Encoding.ASCII.GetBytes(exitMessage);
                        client.Send(exitMessageBytes, exitMessageBytes.Length, serverEndpoint);
                        break;
                    }

                    byte[] commandBytes = Encoding.ASCII.GetBytes(command);
                    client.Send(commandBytes, commandBytes.Length, serverEndpoint);

                    byte[] responseBytes = client.Receive(ref serverEndpoint);
                    string response = Encoding.ASCII.GetString(responseBytes);
                    Console.WriteLine("Pergjigjja e serverit: " + response);

                    if (response == "U mbrri limiti i klienteve!")
                    {
                        Console.WriteLine("Duke diskonektuar klientin...");
                        break;
                    }
                }
            }
        }

        client.Close();
    }
}
