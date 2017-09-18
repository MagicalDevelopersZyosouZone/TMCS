using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace TMCS_Test
{
    public class WebSocketHandler
    {
        public WebSocket WebSocket;
        public string Uid;
        public string Token;
        public UserStatus UserStatus;
        public Thread handleThread;
        public WebSocketHandler(WebSocket ws)
        {
            this.WebSocket = ws;
        }
        public async Task<string> Receive()
        {
            WebSocketReceiveResult result;
            var msg = new List<byte>();
            do
            {
                var buffer = new byte[4096];
                result = await WebSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None);
                var segment = new ArraySegment<byte>(buffer, 0, result.Count);
                msg.AddRange(segment);
                //await webSocket.SendAsync(
                //    new ArraySegment<byte>(
                //        segment.Reverse().ToArray(), 0, result.Count),
                //    result.MessageType,
                //    result.EndOfMessage,
                //    CancellationToken.None);
            } while (!result.EndOfMessage);
            var textData = Encoding.UTF8.GetString(msg.ToArray());
            return textData;
        }
        public async Task StartReceiev()
        {
            if (WebSocket != null)
            {
                if(this.Uid==null || this.Uid=="")
                {
                    await WaitHandShake();
                }
                var msgToSend = new TMCSTest.Message[] {
                                    new TMCSTest.Message("SardineFish", Uid, TMCSTest.MessageType.Text, "Welcom to TMCS!"),
                                    new TMCSTest.Message("SardineFish", Uid, TMCSTest.MessageType.Text, "TMCS is now developing."),
                                    new TMCSTest.Message("SardineFish", Uid, TMCSTest.MessageType.Text, "It is just a demo now.")
                                };

                await SendTextAsync(await TMCSTest.JSONStringifyAsync(msgToSend));
                while (WebSocket.State== WebSocketState.Open)
                {
                    try
                    {
                        var textData = await Receive();
                        JObject rcvObj = JObject.Parse(textData);
                        var receiver = rcvObj["receiverId"].ToString();
                        if (receiver == "TMCS")
                        {
                            var data = rcvObj["data"].ToString();
                            //var data = TMCSTest.RSADecrypt(Convert.FromBase64String(dataEnc), TMCSTest.PRIVATE_KEY);
                            //var dataText = Encoding.UTF8.GetString(data);
                            if (rcvObj["type"].ToString() == "Signal")
                            {
                                await HandleSignal(JObject.Parse(data));
                            }
                        }
                        else
                        {
                            await HandleMessage(rcvObj);
                        }

                            
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public async Task SendTextAsync(string text)
        {
            await WebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(text)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task WaitHandShake()
        {
            if (WebSocket.State == WebSocketState.Open)
            {
                // Wait for handshake
                var handshakeData = JObject.Parse(await Receive());
                if (handshakeData["receiverId"] == null ||
                    handshakeData["receiverId"].ToString() != "TMCS" ||
                    handshakeData["type"] == null ||
                    handshakeData["type"].ToString() != "Signal" ||
                    handshakeData["data"]["signal"] == null ||
                    handshakeData["data"]["signal"].ToString() != "HandShake"
                    )
                {
                    await SendTextAsync(await TMCSTest.JSONStringifyAsync(new
                    {
                        type = "Signal",
                        senderId = "TMCS",
                        receiverId = "[user]",
                        data = new
                        {
                            signal = "Error",
                            data = -100
                        }
                    }));
                    return;
                }
                await HandShake(handshakeData["data"]["data"]);

                // Wait for start signal
                var readyData = JObject.Parse(await Receive());
                if (readyData["receiverId"] == null ||
                    readyData["receiverId"].ToString() != "TMCS" ||
                    readyData["type"] == null ||
                    readyData["type"].ToString() != "Signal" ||
                    readyData["data"]["signal"] == null ||
                    readyData["data"]["signal"].ToString() != "Ready"
                    )
                {
                    await SendTextAsync(await TMCSTest.JSONStringifyAsync(new
                    {
                        type = "Signal",
                        senderId = "TMCS",
                        receiverId = "[user]",
                        data = new
                        {
                            signal = "Error",
                            data = -100
                        }
                    }));
                    return;
                }
            }
        }

        public async Task HandleMessage(JObject msgData)
        {
            var receiver=msgData["receiverId"].ToString();
            if (TMCSTest.HandlerList.ContainsKey(receiver))
            {
                var send = new object[] {
                    new {
                        type = msgData["type"].ToString(),
                        senderId = Uid,
                        data =  msgData["data"].ToString()
                    }
                };

                var msgToSendJson = await TMCSTest.JSONStringifyAsync(send);
                await TMCSTest.HandlerList[receiver].SendTextAsync(msgToSendJson);
            }
            else
            {
                var sender = "";
                if (TMCSTest.rand.NextDouble() < 0.5)
                {
                    sender = receiver;
                }
                else
                    sender = TMCSTest.RandomUid();
                var send = new object[] {
                    new {
                        type = msgData["type"].ToString(),
                        senderId = sender,
                        data =  msgData["data"].ToString()
                    }
                };

                var msgToSendJson = await TMCSTest.JSONStringifyAsync(send);
                await SendTextAsync(msgToSendJson);
            }
        }

        public async Task HandleSignal(JObject signalData)
        {
            switch (signalData["signal"].ToString())
            {
                case "HandShake":
                    await HandShake(signalData["data"]);
                    break;
            }
        }

        public async Task HandShake(JToken data)
        {
            var uid = data["uid"].ToString();
            var token = data["token"].ToString();
            this.Uid = uid;
            this.Token = token;
            await SendTextAsync(await TMCSTest.JSONStringifyAsync(new
            {
                type = "Signal",
                senderId = "TMCS",
                data = new
                {
                    signal = "HandShake",
                    data = new
                    {
                        serverName = "TMCS-Test",
                        owner = "MDZZ.studio",
                        version = "0.1.0-alpha",
                        pubKey = TMCSTest.PUBLIC_KEY
                    }
                }
            }));
        }
    }
}
