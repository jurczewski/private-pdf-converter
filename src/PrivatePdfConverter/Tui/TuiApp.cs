using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace PrivatePdfConverter.Tui;

/// <summary>
/// Interactive text-based user interface (TUI) for Private PDF Converter, built with Terminal.Gui.
/// </summary>
public static partial class TuiApp
{
    /// <summary>
    /// Launches the interactive text-based user interface.
    /// </summary>
    public static void Run()
    {
        using var app = Application.Create();
        app.Init();

        using Window window = new();
        window.Title = "Private PDF Converter (Esc to quit)";

        AddMenuButton(window, "Convert directory of images to PDF", 1, () => RunDirToPdf(app));
        AddMenuButton(window, "Merge PDFs in a directory", 3, () => RunMergePdf(app));
        AddMenuButton(window, "Convert a single image to PDF", 5, () => RunImgToPdf(app));
        AddMenuButton(window, "Encrypt a PDF with a password", 7, () => RunEncryptPdf(app));
        AddMenuButton(window, "Add a logo/watermark to a PDF", 9, () => RunAddLogo(app));
        AddMenuButton(window, "List supported image extensions", 11, () => RunListExtensions(app));
        AddMenuButton(window, "Quit", 13, app.RequestStop);

        app.Run(window);
    }

    private static void AddMenuButton(Window window, string text, int y, Action action)
    {
        var button = new Button
        {
            X = Pos.Center(),
            Y = y,
            Text = text
        };
        button.Accepted += (_, _) => action();
        window.Add(button);
    }
}
