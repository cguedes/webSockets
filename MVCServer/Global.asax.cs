using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCServer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }

        public void Application_BeginRequest()
        {
            if (Request.Headers["Connection"].Equals("Upgrade") &&
                Request.Headers["Upgrade"].Equals("WebSocket"))
            {

                String origin = Request.Headers["Origin"];
                String location = Request.Url.ToString().Replace("http://", "ws://");

                
                Response.StatusCode = 101;
                Response.BufferOutput = false;
                Response.Buffer = false;
                Response.StatusDescription = "Web Socket Protocol Handshake";
                Response.AppendHeader("Upgrade", "WebSocket");
                Response.AppendHeader("Connection", "Upgrade");
                Response.AppendHeader("WebSocket-Origin", origin);
                Response.AppendHeader("WebSocket-Location", location);
                //Response.Flush();

                //Response.Output.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");
                //Response.Output.WriteLine("Upgrade: WebSocket");
                //Response.Output.WriteLine("Connection: Upgrade");
                //Response.Output.WriteLine(String.Format("WebSocket-Origin: {0}", origin));
                //Response.Output.WriteLine(String.Format("WebSocket-Location: {0}", location));
                //Response.Output.WriteLine("");
                //Response.Output.Flush();
                

                //while (true) { }

                //  read type byte
                byte type = (byte)Request.InputStream.ReadByte();
                byte[] sendData = new byte[1024];
                byte sendDataByte; int sendDataIdx = 0;
                while ((sendDataByte = (byte)Request.InputStream.ReadByte()) != 0xFF) {
                    sendData[sendDataIdx++] = sendDataByte;
                }

                String text = System.Text.ASCIIEncoding.Default.GetString(sendData, 0, sendDataIdx);
                Console.WriteLine("LINE: " + text);

                // Write down message ECHO 
                Response.OutputStream.WriteByte(0x00);
                Response.Output.Write(text.ToUpper());
                Response.Output.Flush();
                Response.OutputStream.WriteByte(0xFF);
            }
        }
    }
}