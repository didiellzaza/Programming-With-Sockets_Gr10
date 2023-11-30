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

    public static void Main()
    {
        TcpListener? listener = null;
        try
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            listener.Start();
            Console.WriteLine("MultiThreadedEchoServer ka filluar...");
            while (true)
            {
                Console.WriteLine("Në pritje të lidhjeve hyrëse të klientit...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("U pranua lidhja e re e klientit!");
                Thread t = new(ProcessClientRequests);
                t.Start(client);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener?.Stop();
        }

    }

}

