using LoggerAppAnalyser;
using LoggerAppAnalyser.Interfaces;
using LoggerAppAnalyser.Models;
using LoggerAppAnalyser.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

var host = CreateHostBuilder(args).Build();
LoggerAppAnalyserService app = host.Services.GetRequiredService<LoggerAppAnalyserService>();

app.StartAnalyser("028A033D3983D61F");

//foreach (var filaEv in filaEventos)
//{
//    Console.WriteLine(filaEv.);
//}



static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) => services
                .AddSingleton<LoggerAppAnalyserService>()
                .AddSingleton<IFilaEventosService, FilaEventosService>()
                .AddSingleton<IAgenteEventoService, AgenteEventoService>()
                .AddSingleton<IFileFolderManagerService, FileFolderManagerService>());
}
