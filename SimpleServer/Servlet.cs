using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace SimpleServer
{
    class Servlet
    {
        public virtual void onGet(System.Net.HttpListenerRequest request, System.Net.HttpListenerResponse response) { }
        public virtual void onPost(System.Net.HttpListenerRequest request, System.Net.HttpListenerResponse response) { }

        public virtual void onCreate()
        {
        }
    }


    class Server : Servlet
    {
        public Server() {
        }

        public override void onCreate()
        {
            base.onCreate();
        }

        public override void onGet(HttpListenerRequest request, HttpListenerResponse response)
        {
            Console.WriteLine("GET:" + request.Url);
            byte[] buffer = Encoding.UTF8.GetBytes("OK");

            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        public override void onPost(HttpListenerRequest request, HttpListenerResponse response)
        {
            Console.WriteLine("POST:" + request.Url);
            byte[] res = Encoding.UTF8.GetBytes("OK");
            response.OutputStream.Write(res, 0, res.Length);
        }


        private string port = "8090";
        public void start() {
            var url = string.Format("http://localhost:{0}/", port);
            Console.WriteLine("Start Listenner " + url);

            HttpListener httpListenner;
            httpListenner = new HttpListener();
            httpListenner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListenner.Prefixes.Add(url);
            httpListenner.Start();

            new Thread(new ThreadStart(delegate {
                try
                {
                    loop(httpListenner);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    httpListenner.Stop();
                }
            })).Start();
        }

        private void loop(HttpListener httpListenner)
        {
            while (true)
            {
                HttpListenerContext context = httpListenner.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                Servlet servlet = new Server();
                servlet.onCreate();
                if (request.HttpMethod == "POST")
                {
                    servlet.onPost(request, response);
                }
                else if (request.HttpMethod == "GET")
                {
                    servlet.onGet(request, response);
                }
                response.Close();
            }
        }
    }
}
