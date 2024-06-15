using System.Reflection;
using Cocona;
using PrivatePdfConverter;
using PrivatePdfConverter.Commands;

var versionString = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0";

Logger.Initialize();
Logger.LogStart(versionString);

var app = CoconaLiteApp.Create();

app.AddCommand("dir", DirToPdf.ConvertDirectoryToOnePdf).WithDescription("Convert all images inside directory to single pdf file.");
app.AddCommand("merge", MergePdf.ConvertDirectoryToOnePdf).WithDescription("Merge pdf files from directory into one pdf file.");
app.AddCommand("ext", ListValidExt.ListValidExtensions).WithDescription("List all valid image extensions.");
app.AddCommand("img", ImgToPdf.ConvertImageToOnePdf).WithDescription("Convert single image to single pdf file.");
app.AddCommand("encrypt", EncryptPdf.EncryptPdfWithPassword).WithDescription("Encrypt pdf file with a password.");
app.AddCommand("logo", AddLogoToPdf.Run).WithDescription("Add an image/logo/watermark to a every page of pdf file.");

await app.RunAsync();
