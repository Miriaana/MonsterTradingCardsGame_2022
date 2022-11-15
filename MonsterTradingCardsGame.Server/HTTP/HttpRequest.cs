using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server.HTTP
{
    public class HttpRequest
    {
        private StreamReader reader;

        public HttpMethod Method { get; private set; }
        public string Path { get; private set; } //string[]

        public Dictionary<string, string> QueryParams = new();

        public string ProtocolVersion { get; private set; }

        public Dictionary<string, string> headers = new();

        public string Content { get; private set; }

        public HttpRequest(StreamReader reader)
        {
            this.reader = reader;
        }

        public void Parse()
        {
            //todo: rewrite that whole mess
            // first line contains HTTP METHOD PATH and PROTOCOL
            string line = reader.ReadLine();
            Console.WriteLine($"        {line}");
            var firstLineParts = line.Split(" ");   //error handling: make sure you actually got a line
            //check, are there actually 3 parts
            Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), firstLineParts[0]); //error handling
            Path = firstLineParts[1];
            var pathParts = Path.Split('?');
            if (pathParts.Length == 2)
            {
                var queryParams = pathParts[1].Split("&");
                foreach (string queryParam in queryParams)
                {
                    var queryParamParts = queryParam.Split("=");
                    if (queryParamParts.Length == 2)
                    {
                        QueryParams.Add(queryParamParts[0], queryParamParts[1]);
                    }
                    else
                    {
                        //??
                    }
                }
            }

            ProtocolVersion = firstLineParts[2];

            // headers
            //todo: rewrite, how work?!?
            while ((line = reader.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                Console.WriteLine($"        {line}");
                if (line.Length == 0)
                    break;

                var headerParts = line.Split(": ");
                headers[headerParts[0]] = headerParts[1]; //to´do, replace with sth that makes sense
            }

            // content...
            Content = "";
            var data = new StringBuilder();
            int contentLength = int.Parse(headers["Content-Length"]);
            if (contentLength > 0)
            {
                char[] buffer = new char[1024];
                int totalBytesRead = 0;
                while (totalBytesRead < contentLength)
                {
                    var bytesRead = reader.Read(buffer, 0, 1024);
                    if (bytesRead == 0)
                        break;
                    totalBytesRead += bytesRead;
                    data.Append(buffer, 0, bytesRead);
                }
            }
            Content = data.ToString();
        }
    }
}
