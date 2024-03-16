namespace HTTPServer;

public class ContentTypeResolver
{
    public string? GetContentType(string filename)
    {
        var dictionary = new Dictionary<string, string>
        {
            { ".css", "text/css" },
            { ".html", "text/html" },
            { ".ico", "image/x-icon" },
            { ".js", "application/x-javascript" },
            { ".json", "application/json" },
            { ".png", "image/png" }
        };

        string fileExtension = Path.GetExtension(filename);
        dictionary.TryGetValue(fileExtension, out var contentType);
        return contentType;
    }
}
