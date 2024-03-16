namespace HTTPServer.Services;

public class FileManager
{
    public string GetHtmlContent(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException($"Файл {filename} не найден");

        Console.WriteLine(filename);
        return File.ReadAllText(filename);
    }
}