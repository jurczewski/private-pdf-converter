using System.Reflection;
using ConsoleAppFramework;
using PrivatePdfConverter;
using PrivatePdfConverter.Commands;

var versionString = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0";

Logger.Initialize();
Logger.LogStart(versionString);

var app = ConsoleApp.Create();
RegisterCommands(app);

await app.RunAsync(args);

static void RegisterCommands(ConsoleApp.ConsoleAppBuilder app)
{
    app.Add("dir", DirToPdf.ConvertDirectoryToOnePdf);
    app.Add("merge", MergePdf.ConvertDirectoryToOnePdf);
    app.Add("ext", ListValidExt.ListValidExtensions);
    app.Add("img", ImgToPdf.ConvertImageToOnePdf);
    app.Add("encrypt", EncryptPdf.EncryptPdfWithPassword);
    app.Add("logo", AddLogoToPdf.Run);
}
