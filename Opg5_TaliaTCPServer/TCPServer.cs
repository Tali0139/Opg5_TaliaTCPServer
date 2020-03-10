using BogLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Opg5_TaliaTCPServer
{
    class TCPServer
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(IPAddress.Loopback.ToString());
            TcpListener serverSocket = new TcpListener(ip, 4646);
            serverSocket.Start();
            Console.WriteLine("Server started");
            do
            {
                Task.Run(() =>
                {
                    TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Connection established");
                    DoClient(connectionSocket);
                });
            } while (true);
        }

        public static readonly List<Bog> Bøger = new List<Bog>()
        {
            new Bog("TCP is the new Black", "Talia Damary", 267, "9999966666555"),
            new Bog("C# for Absolute Beginners", "O. P. Oswaldsen", 978, "0002233445567"),
            new Bog("Typescript for Typists", "Talia Damary", 547, "1122334455667"),
            new Bog("Computer Networking", "Peter Eriksen", 23, "1234567890123")
        };

        public static void DoClient(TcpClient socket)
        {
            Stream ns = socket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;

            string message;
            while (true)
            {
                message = sr.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    String[] splitStrings = message.Split(" ", 2);

                    if (splitStrings[0] == "GetAll")
                    {
                        if (Bøger.Count > 0)
                        {
                            foreach (var bog in Bøger)
                            {
                                string answer = JsonConvert.SerializeObject(bog);
                                sw.WriteLine(answer);
                            }
                        }
                        else
                        {
                            sw.WriteLine("Samlingen er tom. Brug Save [bog] for at tilfoeje en bog i samlingen.");
                        }
                    }

                    if (splitStrings[0] == "Get" && splitStrings.Length == 2)
                    {
                        string answer = null;
                        foreach (var bog in Bøger)
                        {
                            if (bog.Isbn == splitStrings[1])
                            {
                                answer = JsonConvert.SerializeObject(bog);
                                sw.WriteLine(answer);
                            }
                        }
                        if (answer == null)
                        {
                            sw.WriteLine("Der er ingen bog med dette Isbn i samlingen");
                        }
                    }

                    if (splitStrings[0] == "Save")
                    {
                        string ISBN = "Isbn";
                        string ObjectToSave = splitStrings[1];
                        bool isBook = ObjectToSave.Contains(ISBN);
                        if (isBook)
                        {
                            Bog b1 = JsonConvert.DeserializeObject<Bog>(splitStrings[1]);
                            Bøger.Add(b1);
                        }
                        else sw.WriteLine("Objektet du vil gemme er ikke en bog.");
                    }

                    if (splitStrings[0] != "GetAll" && splitStrings[0] != "Get" && splitStrings[0] != "Save")
                    {
                        sw.WriteLine("Forespoergslen kan ikke genkendes. Brug GetAll, Get[isbn] eller Save[bog]");
                    }
                }
                else sw.WriteLine("Brug GetAll, Get [isbn] eller Save [bog]");
            }
        }
    }
}




