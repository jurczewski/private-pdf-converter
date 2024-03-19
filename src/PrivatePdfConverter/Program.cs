using Cocona;
using PrivatePdfConverter;
using PrivatePdfConverter.Commands;

var app = CoconaLiteApp.Create();

app.AddCommand("pdf", DirToPdf.ConvertDirectoryToOnePdf);

Logger.Initialize();

app.Run();
