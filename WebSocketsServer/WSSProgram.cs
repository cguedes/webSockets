using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace WebSocketsServer
{

    public class WebSocketServer
    {
        private int port;

        public WebSocketServer(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            IPAddress localAddress = IPAddress.Loopback;
            TcpListener server = new TcpListener(localAddress, port);
            server.Start();
            Console.WriteLine(String.Format("WebSocketServer listening on {0} port of the {1} IP", port, localAddress));

            while (true)
            {
                Console.WriteLine("Waiting for a connection... ");

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected");

                NetworkStream clientStream = client.GetStream();
                StreamReader sr = new StreamReader(clientStream);
                
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("REQUEST");
                Console.WriteLine("---------------------------------------------");
                // status line: GET / HTTP/1.1
                Console.WriteLine(sr.ReadLine()); 
                // headers
                String header = null;
                while ((header = sr.ReadLine()).Length != 0)
                {
                    Console.WriteLine(header);
                }
                // other content (8byte)
                //int firstByte = clientStream.ReadByte();
                //Console.WriteLine("firstByte = " + firstByte);

                //byte[] challenge = new byte[8];
                //clientStream.Read(challenge, 0, 4);
                //Console.Write(challenge);

                Console.WriteLine("---------------------------------------------");


                Console.WriteLine("SEND HTTP RESPONSE --------------------------");
                // SEND RESPONSE
                StreamWriter sw = new StreamWriter(clientStream);

                sw.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");                sw.WriteLine("Upgrade: WebSocket");                sw.WriteLine("Connection: Upgrade");                sw.WriteLine("WebSocket-Origin: http://localhost:8080");
                sw.WriteLine("WebSocket-Location: ws://localhost:8181/websession");
                sw.WriteLine("");
                
                //sw.Write("HTTP/1.1 101 WebSockets Protocol Handshake\r\n");
                //sw.Write("Upgrade: WebSocket\r\n");
                //sw.Write("Connection: Upgrade\r\n");
                //sw.Write("WebSocket-Origin: http://localhost:14195\r\n");
                //sw.Write("WebSocket-Location: ws://localhost:8081\r\n");

                Console.WriteLine("---------------------------------------------");

                sw.Flush();

                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("REQUEST (SEND)");
                Console.WriteLine("---------------------------------------------");
                // status line: GET / HTTP/1.1
                Console.WriteLine(sr.ReadLine());
                // headers
                while ((header = sr.ReadLine()).Length != 0)
                {
                    Console.WriteLine(header);
                }


           }
        }

    }

    public class WSSProgram
    {
        static void Mainx(string[] args)
        {

            WebSocketServer wss = new WebSocketServer(8181);
            wss.listen();

        }
    }
}
