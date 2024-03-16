using System.Text.Json;

namespace HTTPServer.Dtos;

public class TestDto
{
    public string IndexTitle;
    public string Page1;
    public string Page2;
    public string Employees;
    public string LayoutTitle;
    public EmployeeJsonModel EmployeesList = Deserialize();
    public int From;
    public int To;
    public TextModel TextModel;

    private static EmployeeJsonModel? Deserialize()
    {
        var deserialize = File.ReadAllText("../../../site/Employees.json");
        var employeeJsonModel = JsonSerializer.Deserialize<EmployeeJsonModel>(deserialize);
        
        return employeeJsonModel;
    } 
}