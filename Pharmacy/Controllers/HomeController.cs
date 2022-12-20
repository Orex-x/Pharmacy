using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using static System.String;

namespace Pharmacy.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationContext _db;
    private readonly string _set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";

    public HomeController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _logger = logger;
        _db = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult EasyData()
    {
        return View();
    } 
    
    public ViewResult Backup()
    {  
        var userName = User.Identity!.Name;
        if(userName.ToLower() == "admin")
            return View(true);
        return View(false);
    }

    public async Task Dump()
    {
        await PSqlDump(
            @"C:\Program Files\PostgreSQL\14\bin\", 
            "333", 
            "postgres", 
            "Pharmacy", 
            "db");
        RedirectToAction("Backup", "Home");
    } 
    
    public async Task Restore()
    {
        await PSqlRestore(
            @"C:\Program Files\PostgreSQL\14\bin\", 
            "333", 
            "postgres", 
            "Pharmacy", 
            "db");
        RedirectToAction("Backup", "Home");
    } 
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    private async Task PSqlDump(string pathToExecutableFile, string password, string login, string database, string outputFile)
    {
        string[] commands =
        {
            $"cd {pathToExecutableFile}", 
            $"{_set} PGPASSWORD={password}", 
            $"pg_dump.exe -U {login} {database} > {outputFile}.sql"
        };
        await RunCommands(commands);
    }
    
    private async Task PSqlRestore(string pathToExecutableFile, string password, string login, string database, string inputFile)
    {
        string[] commands =
        {
            $"cd {pathToExecutableFile}", 
            $"{_set} PGPASSWORD={password}", 
            $"psql -U {login} -d {database} -c \"select pg_terminate_backend(pid) from pg_stat_activity where datname = '{database}'",
            $"dropdb -U {login} {database}",
            $"createdb -U {login} {database}",
            $"psql -U {login} -d {database} -f {inputFile}.sql",
        };
        await RunCommands(commands);
    }

    private static async Task RunCommands(string[] commands)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false

            }
        };
        process.Start();
        await using var pWriter = process.StandardInput;
        if (pWriter.BaseStream.CanWrite)
        {
            foreach (var line in commands)
                await pWriter.WriteLineAsync(line);
        }
    }
}