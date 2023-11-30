using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;

public class MultiThreadedEchoServer
{
    private static void ProcessClientRequests(object argument)
    {
        TcpClient client = (TcpClient)argument;
        try
        {
            StreamReader reader = new(client.GetStream());
            StreamWriter writer = new(client.GetStream());
            string s = String.Empty;
            while (!(s = reader.ReadLine()).Equals("Exit", StringComparison.Ordinal) || s == null)
            {
                Console.WriteLine("Nga Klienti =>> " + s);
                writer.WriteLine("Nga Serveri =>> " + s);
                writer.Flush();
            }
            reader.Close();
            writer.Close();
            client.Close();
            Console.WriteLine("Duke mbyllur lidhjen e klientit...");
        }
        catch (IOException)
        {
            Console.WriteLine("Problem komunikimi me klientin. Exiting thread.");
        }
        finally
        {
            client?.Close();
        }
    }



}

