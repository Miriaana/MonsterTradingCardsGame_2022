using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.Server.HTTP
{
    public class HttpResponse
    {
        private StreamWriter writer;
        public int ResponseCode { get; set; }
        public string ResponseText { get; set; }

        public Dictionary<string, string> Headers = new();
        public string ResponseContent { get; set; }

        public HttpResponse(StreamWriter writer)
        {
            this.writer = writer;
            ResponseText = string.Empty;
            ResponseContent = string.Empty;
        }

        public void Process()
        {
            Console.WriteLine($"    {Thread.CurrentThread.ManagedThreadId}: HTTP/1.1 {ResponseCode} {ResponseText}");
            try
            {
                writer.WriteLine($"HTTP/1.1 {ResponseCode} {ResponseText}"); //handle exception if client closes down
                                                                             // headers... (skipped)
                writer.WriteLine();
                writer.WriteLine(ResponseContent);

                writer.Flush();
                writer.Close();
                Console.WriteLine("Writer managed to send response");
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Writer closed before sending response");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("RemoteHost forced connection closed (or sth like that)");
            }
        }

        //opt todo: implement func to automatically set code/text/content
    }
}
