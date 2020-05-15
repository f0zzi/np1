using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using server.DataModel;
using System.Data.Entity;

namespace server
{
    class Server
    {
        static PersonsContext context;
        static void Main(string[] args)
        {
            using (context = new PersonsContext())
            {
                if (context.Persons.Count() <= 0)
                {
                    context.Persons.Add(new Person("test1", "1"));
                    context.Persons.Add(new Person("test2", "2"));
                    context.Persons.Add(new Person("test3", "3"));
                    context.SaveChanges();
                }
                context.Persons.Load();
            }
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            const int PORT = 2020;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPEndPoint ep = new IPEndPoint(ip, PORT);
                socket.Bind(ep);
                socket.Listen(10);//max queue 10 clients
                while (true)
                {
                    const int SIZE = 1024;
                    Socket client = socket.Accept();
                    byte[] buffer = new byte[SIZE];
                    int countBytes = 0;
                    do
                    {
                        countBytes = client.Receive(buffer);
                    }
                    while (client.Available > 0);
                    string data = Encoding.UTF8.GetString(buffer, 0, countBytes);

                    Person[] tmp = context.Persons.Where(x=>x.Name.Contains(data)).ToArray();
                    byte[] response = Formatter.Serialize(tmp);
                    client.Send(response);
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            };
        }
    }
}