using System.Net;
using System.Text;
using HTTPServer;
using HTTPServer.Services;

class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        string currentDir = Directory.GetCurrentDirectory();
        string site = currentDir + "/site";
        Server server = new Server(site, new HttpListener(), 8000, 
            new ContentTypeResolver(), new HtmlBuilder(), new FileManager());
        server.Start();
    }
}