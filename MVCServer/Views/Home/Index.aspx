﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script type="text/javascript">
        var socket = null;
        
        
        function connect()
        {
            if(socket != null) { alert("already connected"); return; }

            //var server = "ws://websockets.org";
            var server = 'ws://localhost:8282/websession';
            socket = new WebSocket(server);

            socket.onopen = function() {  alert('handshake successfully established. May send data now...'); };
            socket.onclose = function() { alert('connection closed');  socket = null; };
            socket.onmessage = onSocketMessage;
        }

        function send() 
        {
            if(socket == null) { alert("connect before send the message"); return; }

            var text = txtSendText.value;
            var sendResult = socket.send(text);
            alert("sendResult = " + sendResult);
        }
        
        // Socket message arrived
        function onSocketMessage(msg)
        {
            alert("Message: " + msg.data);
        }

    </script>
</head>
<body >

    <input id="btnConnect" type="button" value="Connect" onclick="connect();" /><br />
    
    <input id="txtSendText" type="text" value="Hello Web" /> <input type="button" value="Send" onclick="send();" />
</body>
</html>