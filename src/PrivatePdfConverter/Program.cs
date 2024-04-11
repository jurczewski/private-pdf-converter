﻿using Cocona;
using PrivatePdfConverter;
using PrivatePdfConverter.Commands;

var app = CoconaLiteApp.Create();

app.AddCommand("dir", DirToPdf.ConvertDirectoryToOnePdf).WithDescription("Converts all images inside directory to single pdf file.");
app.AddCommand("merge", MergePdf.ConvertDirectoryToOnePdf).WithDescription("Merge pdf files from directory into one pdf file.");
app.AddCommand("ext", ListValidExt.ListValidExtensions).WithDescription("List all valid image extensions.");
app.AddCommand("img", ImgToPdf.ConvertImageToOnePdf).WithDescription("Converts single image to single pdf file.");

Logger.Initialize();

app.Run();
