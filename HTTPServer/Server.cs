using System.Net;
using System.Text;
using System.Text.Json;
using HTTPServer.Dtos;
using HTTPServer.Services;

namespace HTTPServer;

public class Server
{
    private readonly Thread _serverThread;
    private readonly string _siteDirectory;
    private readonly HttpListener _listener;
    private readonly int _port;
    private readonly ContentTypeResolver _resolver;
    private HttpListenerContext _context;
    private readonly HtmlBuilder _htmlBuilder;
    private readonly FileManager _fileManager;

    public Server(string siteDirectory, HttpListener listener, 
        int port, ContentTypeResolver resolver, 
        HtmlBuilder htmlBuilder, FileManager fileManager)
    {
        _siteDirectory = siteDirectory;
        _listener = listener;
        _port = port;
        _resolver = resolver;
        _htmlBuilder = htmlBuilder;
        _fileManager = fileManager;
        _serverThread = new Thread(Listen);
    }

    public void Start()
    {
        _serverThread.Start();
        Console.WriteLine($"Сервер запущен на порту{_port}");
        Console.WriteLine($"Файлы лежат тут {_siteDirectory}");
    }

    private void Listen()
    {
        _listener.Prefixes.Add($"http://localhost:{_port}/");
        _listener.Start();
        while (true)
        {
            try
            {
                _context = _listener.GetContext();

                TextModel textModel = new TextModel();
                
                if (_context.Request.HttpMethod == HttpMethod.Post.ToString())
                {
                    Stream body = _context.Request.InputStream;
                    Encoding encoding = _context.Request.ContentEncoding;
                    StreamReader reader = new StreamReader(body, encoding);
                    string s = reader.ReadToEnd();
                    textModel = JsonSerializer.Deserialize<TextModel>(s);
                    Console.WriteLine(textModel.Text);
                    body.Close();
                    reader.Close();
                }
                
                var idFrom = _context.Request.QueryString["IdFrom"];
                var idTo = _context.Request.QueryString["IdTo"];
                string filename = _context.Request.Url.AbsolutePath[1..];
                string filePath = Path.Combine(_siteDirectory, filename);

                bool idFromParseResult = int.TryParse(idFrom, out var intFromResult);
                if (idFromParseResult == false)
                    intFromResult = 1;

                bool idToParseResult = int.TryParse(idTo, out var intToResult);
                if (idToParseResult == false)
                    intToResult = 5;
                
                var content = filename.Contains("html")
                    ? _htmlBuilder.BuildHtml(filename, filePath, _siteDirectory, intFromResult, intToResult, textModel.Text)
                    : _fileManager.GetHtmlContent(filePath);
                var handler = new RequestHandler(_context);
                handler.Handle(content, filename);
                Console.WriteLine("Request Handler начал работу");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                _context.Response.StatusDescription = "Not Found";
                _context.Response.OutputStream.Close();
            }
        }
    }
}