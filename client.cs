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

           
        }
    }
}

