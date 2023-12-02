using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class UDPServer
{
    private const int PORT = 6000;
    private const string IP_ADDRESS = "192.168.0.18";

    private static int clientCount = 0;

    private static bool Authenticate(string credentials, out bool readOnly)
    {
        readOnly = false;

        if (credentials == "admin:grupi10")
        {
            readOnly = false;
            return true;
        }
        else if (credentials == "readonly:fiek2023")
        {
            readOnly = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private static string ExecuteCommand(string command)
    {
        try
        {
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = procStartInfo;
            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return result;
        }
        catch (Exception ex)
        {
            return "Error gjate ekzekutimit te komandes: " + ex.Message;
        }
    }

   //

    static void Main()
    {
        UdpClient server = new UdpClient(new IPEndPoint(IPAddress.Parse(IP_ADDRESS), PORT));

        try
        {
            Console.WriteLine($"UDP Serveri po degjon ne: {IP_ADDRESS}:{PORT}");

            List<Thread> clientThreads = new List<Thread>();

            while (true)
            {
                IPEndPoint clientAddress = new IPEndPoint(IPAddress.Any, 0);

                if (clientThreads.Count < 2)
                {
                    Thread newClientThread = new Thread(() => HandleClient(server, clientAddress));
                    newClientThread.Start();
                    clientThreads.Add(newClientThread);
                }

                clientThreads = clientThreads.Where(t => t.IsAlive).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            server.Close();
        }
    }
}
