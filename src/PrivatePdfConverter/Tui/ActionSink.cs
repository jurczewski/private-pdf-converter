using Serilog.Core;
using Serilog.Events;

namespace PrivatePdfConverter.Tui;

/// <summary>
/// A minimal Serilog sink that forwards rendered log messages to a callback instead of writing to the console.
/// Used to capture command output so it can be displayed inside the TUI.
/// </summary>
internal sealed class ActionSink(Action<string> onMessage) : ILogEventSink
{
    public void Emit(LogEvent logEvent)
        => onMessage($"[{logEvent.Level}] {logEvent.RenderMessage()}");
}
