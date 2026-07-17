using PrivatePdfConverter.Commands;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace PrivatePdfConverter.Tui;

public static partial class TuiApp
{
    private static readonly string[] logoPositionLabels = ["Top-left", "Top-right", "Bottom-left", "Bottom-right"];
    private static readonly string[] logoPositionValues = ["top-left", "top-right", "bottom-left", "bottom-right"];

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
}
