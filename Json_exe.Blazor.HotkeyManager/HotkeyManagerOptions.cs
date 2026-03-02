using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace Json_exe.Blazor.HotkeyManager;

/// <summary>
/// The options for the HotkeyManager to configure its behavior.
/// </summary>
public sealed record HotkeyManagerOptions
{
    /// <summary>
    /// An optional container where the HotkeyManager will listen for key events.
    /// </summary>
    public ElementReference? Container { get; init; }

    /// <summary>
    /// The hotkeys you want the HotkeyManager to listen for.
    /// </summary>
    public HashSet<Hotkey> Hotkeys { get; init; } = [];

    public HotkeyManagerOptions(HotkeyManagerOptions options)
    {
        Container = options.Container;
        Hotkeys = new HashSet<Hotkey>(options.Hotkeys);
    }
}

/// <summary>
/// Defines a hotkey that the Manager should look for.
/// </summary>
public sealed record Hotkey
{
    [JsonInclude] internal readonly Guid Id = Guid.NewGuid();

    /// <summary>
    /// The key you want to listen for.
    /// </summary>
    public required string Key { get; init; } = string.Empty;

    /// <summary>
    /// True if you want to listen for Ctrl + Key
    /// </summary>
    public bool CtrlKey { get; init; }

    /// <summary>
    /// True if you want to listen for Shift + Key
    /// </summary>
    public bool ShiftKey { get; init; }

    /// <summary>
    /// True if you want to listen for Alt + Key
    /// </summary>
    public bool AltKey { get; init; }

    /// <summary>
    /// True if you want to prevent the default behavior of the key from the browser.
    /// <remarks>Defaults to true.</remarks>
    /// </summary>
    public bool PreventDefault { get; init; } = true;

    /// <summary>
    /// An action invoked when the associated hotkey is triggered.
    /// </summary>
    [JsonIgnore]
    public Action? OnHotkeyTriggered { get; init; }

    /// <summary>
    /// An asynchronous function that is invoked when the associated hotkey is triggered.
    /// </summary>
    [JsonIgnore]
    public Func<Task>? OnHotkeyTriggeredAsync { get; init; }

    internal Task TriggerHotkeyEvent()
    {
        OnHotkeyTriggered?.Invoke();
        return OnHotkeyTriggeredAsync?.Invoke() ?? Task.CompletedTask;
    }

    /// <inheritdoc />
    public bool Equals(Hotkey? other)
    {
        if (other is null) return false;
        return other.Key.Equals(Key, StringComparison.OrdinalIgnoreCase)
               && other.AltKey == AltKey
               && other.CtrlKey == CtrlKey
               && other.ShiftKey == ShiftKey
               && other.PreventDefault == PreventDefault;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var normalizedKeyHash = StringComparer.OrdinalIgnoreCase.GetHashCode(Key);
        return HashCode.Combine(normalizedKeyHash, CtrlKey, ShiftKey, AltKey, PreventDefault);
    }
}