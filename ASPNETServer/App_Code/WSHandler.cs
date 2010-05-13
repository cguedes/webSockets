using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WSHandler
/// </summary>
public class WSHandler : IHttpHandler
{
    #region IHttpHandler Members

    bool IHttpHandler.IsReusable
    {
        get { return true; }
    }

    void IHttpHandler.ProcessRequest(HttpContext context)
    {

        String origin = context.Request.Headers["Origin"];
        String location = context.Request.Url.ToString().Replace("http://", "ws://");


        context.Response.StatusCode = 101;
        context.Response.StatusDescription = "Web Socket Protocol Handshake";
        context.Response.AppendHeader("Upgrade", "WebSocket");
        context.Response.AppendHeader("Connection", "Upgrade");
        //context.Response.AppendHeader("WebSocket-Origin", origin);
        //context.Response.AppendHeader("WebSocket-Location", location);

        byte type = (byte)context.Request.InputStream.ReadByte();
        byte[] sendData = new byte[1024];
        byte sendDataByte; int sendDataIdx = 0;
        while ((sendDataByte = (byte)context.Request.InputStream.ReadByte()) != 0xFF)
        {
            sendData[sendDataIdx++] = sendDataByte;
        }

        String text = System.Text.Encoding.Default.GetString(sendData, 0, sendDataIdx);
        Console.WriteLine("LINE: " + text);

        // Write down message ECHO 
        context.Response.OutputStream.WriteByte(0x00);
        context.Response.Output.Write(text.ToUpper());
        context.Response.Output.Flush();
        context.Response.OutputStream.WriteByte(0xFF);

        //int x = 10;
        //while (x-- > 0)
        //{
        //    context.Response.Write("Hello -> ");
        //    context.Response.Flush();
        //    System.Threading.Thread.Sleep(1000);
        //}

    }

    #endregion
}