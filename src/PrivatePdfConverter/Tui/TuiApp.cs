using PrivatePdfConverter.Commands;
using Serilog;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace PrivatePdfConverter.Tui;

/// <summary>
/// Interactive text-based user interface (TUI) for Private PDF Converter, built with Terminal.Gui.
/// </summary>
public static class TuiApp
{
    /// <summary>
    /// Launches the interactive text-based user interface.
    /// </summary>
    public static void Run()
    {
        using IApplication app = Application.Create();
        app.Init();

        using Window window = new() { Title = "Private PDF Converter (Esc to quit)" };

        AddMenuButton(window, app, "Convert directory of images to PDF", 1, () => RunDirToPdf(app));
        AddMenuButton(window, app, "Merge PDFs in a directory", 3, () => RunMergePdf(app));
        AddMenuButton(window, app, "Convert a single image to PDF", 5, () => RunImgToPdf(app));
        AddMenuButton(window, app, "Encrypt a PDF with a password", 7, () => RunEncryptPdf(app));
        AddMenuButton(window, app, "Add a logo/watermark to a PDF", 9, () => RunAddLogo(app));
        AddMenuButton(window, app, "List supported image extensions", 11, () => RunListExtensions(app));
        AddMenuButton(window, app, "Quit", 13, app.RequestStop);

        app.Run(window);
    }

    private static void AddMenuButton(Window window, IApplication app, string text, int y, Action action)
    {
        var button = new Button
        {
            X = Pos.Center(),
            Y = y,
            Text = text,
        };
        button.Accepted += (_, _) => action();
        window.Add(button);
    }

    private static void RunDirToPdf(IApplication app)
    {
        var values = PromptForFields(
            app,
            "Convert directory of images to PDF",
            ("Directory path:", false, ""),
            ("Output file name (optional):", false, ""));

        if (values is null)
        {
            return;
        }

        var path = values[0];
        var output = string.IsNullOrWhiteSpace(values[1]) ? null : values[1];

        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A directory path is required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Convert directory of images to PDF", () => DirToPdf.ConvertDirectoryToOnePdf(path, output));
    }

    private static void RunMergePdf(IApplication app)
    {
        var values = PromptForFields(
            app,
            "Merge PDFs in a directory",
            ("Directory path:", false, ""),
            ("Output file name (optional):", false, ""));

        if (values is null)
        {
            return;
        }

        var path = values[0];
        var output = string.IsNullOrWhiteSpace(values[1]) ? null : values[1];

        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A directory path is required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Merge PDFs in a directory", () => MergePdf.ConvertDirectoryToOnePdf(path, output));
    }

    private static void RunImgToPdf(IApplication app)
    {
        var values = PromptForFields(
            app,
            "Convert a single image to PDF",
            ("Image path:", false, ""),
            ("Output file name (optional):", false, ""));

        if (values is null)
        {
            return;
        }

        var path = values[0];
        var output = string.IsNullOrWhiteSpace(values[1]) ? null : values[1];

        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.ErrorQuery(app, "Missing input", "An image path is required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Convert a single image to PDF", () => ImgToPdf.ConvertImageToOnePdf(path, output));
    }

    private static void RunEncryptPdf(IApplication app)
    {
        var values = PromptForFields(
            app,
            "Encrypt a PDF with a password",
            ("PDF path:", false, ""),
            ("Password:", true, ""),
            ("Output file name (optional):", false, ""));

        if (values is null)
        {
            return;
        }

        var path = values[0];
        var password = values[1];
        var output = string.IsNullOrWhiteSpace(values[2]) ? null : values[2];

        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A PDF path and password are required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Encrypt a PDF with a password", () => EncryptPdf.EncryptPdfWithPassword(path, password, output));
    }

    private static void RunAddLogo(IApplication app)
    {
        var values = PromptForFields(
            app,
            "Add a logo/watermark to a PDF",
            ("PDF path:", false, ""),
            ("Logo image path:", false, ""),
            ("Position (top-left, top-right, bottom-left, bottom-right):", false, "bottom-right"),
            ("Scale % (optional):", false, ""),
            ("Opacity % (optional):", false, ""),
            ("Output file name (optional):", false, ""));

        if (values is null)
        {
            return;
        }

        var path = values[0];
        var logoPath = values[1];
        var position = values[2];
        var scale = ParseOptionalInt(values[3]);
        var opacity = ParseOptionalInt(values[4]);
        var output = string.IsNullOrWhiteSpace(values[5]) ? null : values[5];

        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(logoPath) || string.IsNullOrWhiteSpace(position))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A PDF path, logo path, and position are required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Add a logo/watermark to a PDF", () => AddLogoToPdf.Run(path, logoPath, position, scale, opacity, output));
    }

    private static void RunListExtensions(IApplication app)
        => ExecuteAndShowResult(app, "Supported image extensions", ListValidExt.ListValidExtensions);

    private static int? ParseOptionalInt(string value)
        => int.TryParse(value, out var result) ? result : null;

    /// <summary>
    /// Shows a modal dialog with one text field per requested field and OK/Cancel buttons.
    /// Returns the entered values in order, or <see langword="null"/> if the user cancelled.
    /// </summary>
    private static string[]? PromptForFields(IApplication app, string title, params (string Label, bool Secret, string Default)[] fields)
    {
        using Dialog dialog = new()
        {
            Title = title,
            Width = Dim.Percent(80),
            Height = fields.Length * 2 + 4,
        };
        dialog.App = app;

        var textFields = new TextField[fields.Length];
        for (var i = 0; i < fields.Length; i++)
        {
            var (label, secret, defaultValue) = fields[i];
            var fieldLabel = new Label { X = 1, Y = i * 2, Text = label };
            var textField = new TextField
            {
                X = 1,
                Y = (i * 2) + 1,
                Width = Dim.Fill(1),
                Secret = secret,
                Text = defaultValue,
            };
            textFields[i] = textField;
            dialog.Add(fieldLabel, textField);
        }

        dialog.AddButton(new Button { Text = "_Cancel" });
        dialog.AddButton(new Button { Text = "_OK", IsDefault = true });

        textFields[0].SetFocus();

        app.Run(dialog);

        if (dialog.Canceled)
        {
            return null;
        }

        return textFields.Select(t => t.Text ?? string.Empty).ToArray();
    }

    /// <summary>
    /// Runs the given action while temporarily capturing Serilog output, then shows the captured
    /// messages (or any thrown exception) in a modal message box instead of writing to the console.
    /// </summary>
    private static void ExecuteAndShowResult(IApplication app, string title, Action action)
    {
        var messages = new List<string>();
        var previousLogger = Log.Logger;
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Sink(new ActionSink(messages.Add))
            .CreateLogger();

        try
        {
            action();
        }
        catch (Exception ex)
        {
            messages.Add($"Error: {ex.Message}");
        }
        finally
        {
            Log.Logger = previousLogger;
        }

        var message = messages.Count > 0 ? string.Join(Environment.NewLine, messages) : "Done.";
        MessageBox.Query(app, title, message, "OK");
    }
}
