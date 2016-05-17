using System;
using System.Net;
using System.Threading;

namespace SimpleServer
{
    class Program
    {
        static void Main()
        {
            var server = new Server();
            server.start();
        }
    }
}

