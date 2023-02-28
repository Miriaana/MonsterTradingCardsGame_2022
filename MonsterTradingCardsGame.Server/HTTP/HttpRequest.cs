using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MTCGame.Server.HTTP
{
    public class HttpRequest
    {
        private readonly StreamReader reader;

        public EHttpMethod Method { get; private set; }
        public List<string> Path { get; private set; } //string[]

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
            // first line contains HTTP-METHOD, PATH and PROTOCOL
            string line = reader.ReadLine();
            if(line == null)
            {
                Console.WriteLine("No line received");
                throw new Exception("server didn't receive a readable request");
            }
            Console.WriteLine($"\t{line}");
            var firstLineParts = line.Split(" ");
            if(firstLineParts.Length != 3)
            {
                Console.WriteLine("Not able to correctly parse first line");
                throw new Exception("400: Invalid Http Request");
            }
            Method = (EHttpMethod)Enum.Parse(typeof(EHttpMethod), firstLineParts[0]); //error handling
            ProtocolVersion = firstLineParts[2];

            var fullPath = firstLineParts[1];
            
            var pathParts = fullPath.Split('?');    //split path and optional parameters

            Path = pathParts[0].Split("/", StringSplitOptions.RemoveEmptyEntries).ToList();//save path

            if (pathParts.Length == 2)              //parse optional params
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
                        Console.WriteLine("Invalid QueryParams");
                        throw new Exception("User sent invalid request");
                    }
                }
            }

            //Console.WriteLine($"Path: /{string.Join("/", Path.ToArray())} \tParams: {string.Join(",", QueryParams)}");

            // headers (Host, User - Agent, Accept, Content-Type, Content-Length)
            while ((line = reader.ReadLine()) != null) // continues reading after first line until it reaches an empty line (end of headers)
            {
                //Console.WriteLine(line);
                Console.WriteLine($"        {line}");
                if (line.Length == 0)
                    break;

                var headerParts = line.Split(": ");
                headers[headerParts[0]] = headerParts[1];
            }

            // content
            Content = "";
            var data = new StringBuilder();

            int contentLength = 0;
            if (headers.ContainsKey("Content-Length"))
            {
                contentLength = int.Parse(headers["Content-Length"]);
            }
            if (contentLength > 0) //if there is content to parse
            {
                char[] buffer = new char[1024];
                int totalBytesRead = 0;
                while (totalBytesRead < contentLength)
                {
                    var bytesRead = reader.Read(buffer, 0, 1024);
                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Expected Content not received");
                        break;
                    }
                    totalBytesRead += bytesRead;
                    data.Append(buffer, 0, bytesRead);
                }
                Content = data.ToString();
                Console.WriteLine(Content);
            }
        }

        public string GetToken()
        {
            if (headers.ContainsKey("Authorization"))
            {
                return (headers["Authorization"].Split(" "))[1];
            }
            else
            {
                return "";
            }
        }
    }
}
