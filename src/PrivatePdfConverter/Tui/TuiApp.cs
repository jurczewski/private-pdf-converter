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
    private static readonly string[] logoPositionLabels = ["Top-left", "Top-right", "Bottom-left", "Bottom-right"];
    private static readonly string[] logoPositionValues = ["top-left", "top-right", "bottom-left", "bottom-right"];

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

    private static void RunDirToPdf(IApplication app)
    {
        using var dialog = CreateDialog(app, "Convert directory of images to PDF");
        var y = 0;
        var pathField = AddRequiredTextField(dialog, ref y, "Directory path:");
        var (outputToggle, outputField) = AddOptionalTextField(dialog, ref y, "Output file name (optional):");

        if (!ShowDialog(app, dialog, y, pathField))
        {
            return;
        }

        var path = pathField.Text;
        var output = GetOptionalText(outputToggle, outputField);

        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A directory path is required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Convert directory of images to PDF", () => DirToPdf.ConvertDirectoryToOnePdf(path, output));
    }

    private static void RunMergePdf(IApplication app)
    {
        using var dialog = CreateDialog(app, "Merge PDFs in a directory");
        var y = 0;
        var pathField = AddRequiredTextField(dialog, ref y, "Directory path:");
        var (outputToggle, outputField) = AddOptionalTextField(dialog, ref y, "Output file name (optional):");

        if (!ShowDialog(app, dialog, y, pathField))
        {
            return;
        }

        var path = pathField.Text;
        var output = GetOptionalText(outputToggle, outputField);

        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A directory path is required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Merge PDFs in a directory", () => MergePdf.ConvertDirectoryToOnePdf(path, output));
    }

    private static void RunImgToPdf(IApplication app)
    {
        using var dialog = CreateDialog(app, "Convert a single image to PDF");
        var y = 0;
        var pathField = AddRequiredTextField(dialog, ref y, "Image path:");
        var (outputToggle, outputField) = AddOptionalTextField(dialog, ref y, "Output file name (optional):");

        if (!ShowDialog(app, dialog, y, pathField))
        {
            return;
        }

        var path = pathField.Text;
        var output = GetOptionalText(outputToggle, outputField);

        if (string.IsNullOrWhiteSpace(path))
        {
            MessageBox.ErrorQuery(app, "Missing input", "An image path is required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Convert a single image to PDF", () => ImgToPdf.ConvertImageToOnePdf(path, output));
    }

    private static void RunEncryptPdf(IApplication app)
    {
        using var dialog = CreateDialog(app, "Encrypt a PDF with a password");
        var y = 0;
        var pathField = AddRequiredTextField(dialog, ref y, "PDF path:");
        var passwordField = AddRequiredTextField(dialog, ref y, "Password:", secret: true);
        var (outputToggle, outputField) = AddOptionalTextField(dialog, ref y, "Output file name (optional):");

        if (!ShowDialog(app, dialog, y, pathField))
        {
            return;
        }

        var path = pathField.Text;
        var password = passwordField.Text;
        var output = GetOptionalText(outputToggle, outputField);

        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A PDF path and password are required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Encrypt a PDF with a password", () => EncryptPdf.EncryptPdfWithPassword(path, password, output));
    }

    private static void RunAddLogo(IApplication app)
    {
        using var dialog = CreateDialog(app, "Add a logo/watermark to a PDF");
        var y = 0;
        var pathField = AddRequiredTextField(dialog, ref y, "PDF path:");
        var logoPathField = AddRequiredTextField(dialog, ref y, "Logo image path:");
        var positionSelector = AddPositionField(dialog, ref y, "Position:", logoPositionLabels, defaultIndex: 3);
        var (scaleToggle, scaleUpDown) = AddOptionalIntField(dialog, ref y, "Scale % (optional):");
        var (opacityToggle, opacityUpDown) = AddOptionalIntField(dialog, ref y, "Opacity % (optional):");
        var (outputToggle, outputField) = AddOptionalTextField(dialog, ref y, "Output file name (optional):");

        if (!ShowDialog(app, dialog, y, pathField))
        {
            return;
        }

        var path = pathField.Text;
        var logoPath = logoPathField.Text;
        var position = logoPositionValues[positionSelector.Value ?? 3];
        var scale = scaleToggle.Value == CheckState.Checked ? scaleUpDown.Value : (int?) null;
        var opacity = opacityToggle.Value == CheckState.Checked ? opacityUpDown.Value : (int?) null;
        var output = GetOptionalText(outputToggle, outputField);

        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(logoPath))
        {
            MessageBox.ErrorQuery(app, "Missing input", "A PDF path and logo path are required.", "OK");
            return;
        }

        ExecuteAndShowResult(app, "Add a logo/watermark to a PDF", () => AddLogoToPdf.Run(path, logoPath, position, scale, opacity, output));
    }

    private static void RunListExtensions(IApplication app)
        => ExecuteAndShowResult(app, "Supported image extensions", ListValidExt.ListValidExtensions);

    private static string? GetOptionalText(CheckBox toggle, TextField field)
        => toggle.Value == CheckState.Checked && !string.IsNullOrWhiteSpace(field.Text) ? field.Text : null;

    /// <summary>
    /// Creates an empty modal dialog ready to receive fields via the various AddXField helpers.
    /// Call <see cref="ShowDialog"/> once all fields have been added to finalize its size and run it.
    /// </summary>
    private static Dialog CreateDialog(IApplication app, string title)
    {
        Dialog dialog = new() { Title = title, Width = Dim.Percent(80) };
        dialog.App = app;
        return dialog;
    }

    /// <summary>
    /// Adds a required single-line text field (optionally masked) with a label above it.
    /// </summary>
    private static TextField AddRequiredTextField(Dialog dialog, ref int y, string label, bool secret = false)
    {
        dialog.Add(new Label { X = 1, Y = y, Text = label });
        var field = new TextField { X = 1, Y = y + 1, Width = Dim.Fill(1), Secret = secret };
        dialog.Add(field);
        y += 2;
        return field;
    }

    /// <summary>
    /// Adds an optional text field guarded by a checkbox: the field stays disabled until the checkbox
    /// is checked, making it obvious that leaving it unchecked falls back to the command's default behavior.
    /// </summary>
    private static (CheckBox Toggle, TextField Field) AddOptionalTextField(Dialog dialog, ref int y, string label)
    {
        dialog.Add(new Label { X = 1, Y = y, Text = label });
        var toggle = new CheckBox { X = 1, Y = y + 1, Text = "Custom:" };
        var field = new TextField { X = Pos.Right(toggle) + 1, Y = y + 1, Width = Dim.Fill(1), Enabled = false };
        toggle.ValueChanged += (_, _) => field.Enabled = toggle.Value == CheckState.Checked;
        dialog.Add(toggle, field);
        y += 2;
        return (toggle, field);
    }

    /// <summary>
    /// Adds an optional numeric up/down field guarded by a checkbox, for optional integer parameters
    /// such as scale or opacity percentages.
    /// </summary>
    private static (CheckBox Toggle, NumericUpDown<int> UpDown) AddOptionalIntField(Dialog dialog, ref int y, string label)
    {
        dialog.Add(new Label { X = 1, Y = y, Text = label });
        var toggle = new CheckBox { X = 1, Y = y + 1, Text = "Set:" };
        var upDown = new NumericUpDown<int> { X = Pos.Right(toggle) + 1, Y = y + 1, Increment = 5, Enabled = false };
        toggle.ValueChanged += (_, _) => upDown.Enabled = toggle.Value == CheckState.Checked;
        dialog.Add(toggle, upDown);
        y += 2;
        return (toggle, upDown);
    }

    /// <summary>
    /// Adds a single-choice picker (radio-style option selector) for a fixed set of labeled choices.
    /// </summary>
    private static OptionSelector AddPositionField(Dialog dialog, ref int y, string label, string[] labels, int defaultIndex)
    {
        dialog.Add(new Label { X = 1, Y = y, Text = label });
        var selector = new OptionSelector { X = 1, Y = y + 1, Labels = labels, Value = defaultIndex };
        dialog.Add(selector);
        y += 1 + labels.Length;
        return selector;
    }

    /// <summary>
    /// Finalizes the dialog's size, adds OK/Cancel buttons, focuses the given field and runs it modally.
    /// Returns <see langword="true"/> if the user confirmed with OK, or <see langword="false"/> if canceled.
    /// </summary>
    /// <remarks>
    /// The dialog's height is auto-computed from its content (<see cref="Dim.Auto(DimAutoStyle,Dim,Dim)"/>) rather than a
    /// hardcoded row count, so every field added via the AddXField helpers is guaranteed to be visible -
    /// a fixed "rows + N" formula previously clipped the last field(s) out of view.
    /// </remarks>
    private static bool ShowDialog(IApplication app, Dialog dialog, int contentRows, View focusField)
    {
        _ = contentRows;
        dialog.Height = Dim.Auto();
        dialog.AddButton(new Button { Text = "_Cancel" });
        dialog.AddButton(new Button { Text = "_OK", IsDefault = true });

        focusField.SetFocus();
        app.Run(dialog);

        return !dialog.Canceled;
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
