using System.Net;
using System.Text;

namespace HTTPServer;

public class RequestHandler
{
    private readonly HttpListenerContext _context;
    private readonly ContentTypeResolver _resolver;
    
   public RequestHandler(HttpListenerContext context)
   {
       _context = context;
       _resolver = new ContentTypeResolver();
   }

   public void Handle(string content, string filename)
   {
       try
       {
           byte[] htmlBytes = Encoding.UTF8.GetBytes(content);

           using Stream stream = new MemoryStream(htmlBytes);

           _context.Response.ContentType = _resolver.GetContentType(filename);
           _context.Response.ContentLength64 = stream.Length;

           byte[] buffer = new byte[64 * 1024];
           int bytesRead;
           while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
           {
               _context.Response.OutputStream.Write(buffer, 0, bytesRead);
           }
       }
       catch (Exception ex)
       {
           _context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
           _context.Response.StatusDescription = "Internal Server Error";
           Console.WriteLine(ex.Message);
       }
       finally
       {
           _context.Response.OutputStream.Close();
       }
   }
}






