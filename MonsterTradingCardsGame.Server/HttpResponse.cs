using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server
{
    internal class HttpResponse
    {
        private StreamWriter writer;
        public int ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public string ResponseContent { get; set; }

        public HttpResponse(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void Process()
        {
            writer.WriteLine($"HTTP/1.1 {ResponseCode} {ResponseText}");
            // headers... (skipped)
            writer.WriteLine();
            writer.WriteLine(ResponseContent);
            writer.Flush();
            writer.Close();
        }
    }
}
