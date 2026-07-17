using Serilog;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
// ReSharper disable AccessToDisposedClosure

namespace PrivatePdfConverter.Tui;

public static partial class TuiApp
{
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
