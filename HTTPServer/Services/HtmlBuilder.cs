using HTTPServer.Dtos;
using RazorEngine;
using RazorEngine.Templating;

namespace HTTPServer.Services;

public class HtmlBuilder
{
    public string BuildHtml(string filename, string filePath, string siteDir, int from, int to, string postText)
    {
        Console.WriteLine(filePath);
        string layoutPath = Path.Combine(siteDir, "layout.html");
        var razorService = Engine.Razor;

        if (!razorService.IsTemplateCached("layout", typeof(TestDto)))
            razorService.AddTemplate("layout", File.ReadAllText(layoutPath));

        if (!razorService.IsTemplateCached(filename, typeof(TestDto)))
        {
            razorService.AddTemplate(filename, File.ReadAllText(filePath));
            razorService.Compile(filename, typeof(TestDto));
        }

        var key = razorService.GetKey(filename);

        if (from < 1)
            from = 1;

        if (to > 5)
            to = 5;

        var html = razorService.Run(key.Name, typeof(TestDto), new TestDto
        {
            IndexTitle = "My Index Title",
            LayoutTitle = "Layout",
            Page1 = "My Page 1",
            Page2 = "My Page 2",
            Employees = "Employees",
            From = from,
            To = to,
            TextModel = new TextModel
            {
                Text = postText
            }
        });

        return html;
    }
}