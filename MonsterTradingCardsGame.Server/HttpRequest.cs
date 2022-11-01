using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server
{
    internal class HttpRequest
    {
        private StreamReader reader;

        public string Method { get; private set; }
        public string Path { get; private set; }
        public string ProtocolVersion { get; private set; }

        public Dictionary<string, string> headers = new();

        public string Content { get; private set; }

        public HttpRequest(StreamReader reader)
        {
            this.reader = reader;
        }

        public void Parse()
        {
            // first line contains HTTP METHOD PATH and PROTOCOL
            string line = reader.ReadLine();
            Console.WriteLine(line);
            var firstLineParts = line.Split(" ");
            Method = firstLineParts[0];
            Path = firstLineParts[1];
            ProtocolVersion = firstLineParts[2];

            // headers
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (line.Length == 0)
                    break;

                var headerParts = line.Split(": ");
                headers[headerParts[0]] = headerParts[1];
            }

            // content...
        }
    }
}
