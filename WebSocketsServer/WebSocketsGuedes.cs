using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class WebSocketsGuedes
{

    public static void Run()
    {
        TcpListener server = new TcpListener(IPAddress.Loopback, 8181);
        server.Start();
        using (TcpClient client = server.AcceptTcpClient())
        using (NetworkStream clientStream = client.GetStream())
        using (StreamReader sr = new StreamReader(clientStream))
        using (StreamWriter sw = new StreamWriter(clientStream))
        {
            Console.WriteLine(sr.ReadLine());
            Console.WriteLine(sr.ReadLine());
            Console.WriteLine(sr.ReadLine());
            Console.WriteLine(sr.ReadLine());
            Console.WriteLine(sr.ReadLine());
            Console.WriteLine(sr.ReadLine());
            
            sw.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");
            sw.WriteLine("Upgrade: WebSocket");
            sw.WriteLine("Connection: Upgrade");
            sw.WriteLine("WebSocket-Origin: http://localhost:8080");
            sw.WriteLine("WebSocket-Location: ws://localhost:8181/websession");
            sw.WriteLine("");

            sw.Flush();

            // Accept send
            while (true)
            {
                //  read type byte
                byte type = (byte)clientStream.ReadByte();
                if (type != 0x00) { Console.Error.WriteLine("Erro no protocolo: The type byte was not 0x00"); break; }
                byte[] sendData = new byte[1024];
                byte sendDataByte; int sendDataIdx = 0;
                while ((sendDataByte = (byte)clientStream.ReadByte()) != 0xFF)
                {
                    sendData[sendDataIdx++] = sendDataByte;
                }

                String text = System.Text.ASCIIEncoding.Default.GetString(sendData, 0, sendDataIdx);
                Console.WriteLine("LINE: " + text);

                // Write down message ECHO 
                clientStream.WriteByte(0x00);
                sw.Write(text.ToUpper());
                sw.Flush();
                clientStream.WriteByte(0xFF);

            }

        }
        server.Stop();
    }
}

