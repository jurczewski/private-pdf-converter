using Cocona;
using PrivatePdfConverter;
using PrivatePdfConverter.Commands;

var app = CoconaLiteApp.Create();

app.AddCommand("dir", DirToPdf.ConvertDirectoryToOnePdf).WithDescription("Converts directory content to pdf file.");

Logger.Initialize();

app.Run();
