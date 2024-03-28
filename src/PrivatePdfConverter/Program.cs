﻿using Cocona;
using PrivatePdfConverter;
using PrivatePdfConverter.Commands;

var app = CoconaLiteApp.Create();

app.AddCommand("dir", DirToPdf.ConvertDirectoryToOnePdf).WithDescription("Converts all images inside directory to single pdf file.");
app.AddCommand("merge", MergePdf.ConvertDirectoryToOnePdf).WithDescription("Merge pdf files from directory into one pdf file.");

Logger.Initialize();

app.Run();
