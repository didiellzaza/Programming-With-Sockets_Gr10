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

  private static void HandleClient(UdpClient server, IPEndPoint clientAddress)
{
    try
    {
        while (true)
        {
            byte[] receivedBytes = server.Receive(ref clientAddress);
            string request = Encoding.ASCII.GetString(receivedBytes);

            bool readOnly;
            if (Authenticate(request, out readOnly))
            {
                const string authSuccessMessage = "Autentikim i suksesshem. Lidhja u mundesua.";
                server.Send(Encoding.ASCII.GetBytes(authSuccessMessage), authSuccessMessage.Length, clientAddress);

                clientCount++;

                if (clientCount > 4)
                {
                    const string noSpaceMessage = "U mbrri limiti i klienteve!";
                    server.Send(Encoding.ASCII.GetBytes(noSpaceMessage), noSpaceMessage.Length, clientAddress);
                }

                while (true)
                {
                    receivedBytes = server.Receive(ref clientAddress);
                    string command = Encoding.ASCII.GetString(receivedBytes);
                    string[] commandParts = command.Split(' ');

                    if (commandParts.Length > 0)
                    {
                        string action = commandParts[0];

                        if (action == "read")
                        {
                            if (commandParts.Length > 1)
                            {
                                string fileName = commandParts[1];
                                try
                                {
                                    string fileContent = File.ReadAllText(fileName);
                                    server.Send(Encoding.ASCII.GetBytes(fileContent), fileContent.Length, clientAddress);
                                }
                                catch (FileNotFoundException)
                                {
                                    const string errorMessage = "Error: File nuk u gjet.";
                                    server.Send(Encoding.ASCII.GetBytes(errorMessage), errorMessage.Length, clientAddress);
                                }
                            }
                        }
                        else if (action == "write" && !readOnly)
                        {
                            if (commandParts.Length > 2)
                            {
                                string content = commandParts[1];
                                string fileName = commandParts[2];
                                try
                                {
                                    File.WriteAllText(fileName, content);
                                    const string successMessage = "File u mbishkrua me sukses.";
                                    server.Send(Encoding.ASCII.GetBytes(successMessage), successMessage.Length, clientAddress);
                                }
                                catch (Exception)
                                {
                                    const string errorMessage = "Error: Pamundesi per te mbishkruar file.";
                                    server.Send(Encoding.ASCII.GetBytes(errorMessage), errorMessage.Length, clientAddress);
                                }
                            }
                        }

                        else if (action == "delete" && !readOnly)
                        {
                            if (commandParts.Length > 1)
                            {
                                string fileName = commandParts[1];
                                try
                                {
                                    File.Delete(fileName);
                                    const string successMessage = "File u fshi.";
                                    server.Send(Encoding.ASCII.GetBytes(successMessage), successMessage.Length, clientAddress);
                                }
                                catch (Exception)
                                {
                                    const string errorMessage = "Error: Pamundesi per te fshire file.";
                                    server.Send(Encoding.ASCII.GetBytes(errorMessage), errorMessage.Length, clientAddress);
                                }
                            }
                        }
                        else if (action == "execute" && !readOnly)
                        {
                            string commandToExecute = command.Substring(action.Length).Trim();
                            string result = ExecuteCommand(commandToExecute);
                            server.Send(Encoding.ASCII.GetBytes(result), result.Length, clientAddress);
                        }


                    
                        else
                        {
                            const string errorMessage = "Error: Komande e pa-autorizuar.";
                            server.Send(Encoding.ASCII.GetBytes(errorMessage), errorMessage.Length, clientAddress);
                        }
                    }
                }
            }
            else
            {
                const string authFailureMessage = "Autentikimi deshtoi. Lidhja nuk u realizua.";
                server.Send(Encoding.ASCII.GetBytes(authFailureMessage), authFailureMessage.Length, clientAddress);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error handling client: {ex.Message}");
    }
}

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
