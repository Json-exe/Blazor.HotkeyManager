using Microsoft.AspNetCore.Components;

namespace Json_exe.Blazor.HotkeyManager;

/// <summary>
/// The options for the HotkeyManager to configure its behaviour.
/// </summary>
public sealed record HotkeyManagerOptions
{
    /// <summary>
    /// An optional container where the HotkeyManager will listen for key events.
    /// </summary>
    public ElementReference? Container { get; init; } = null;

    /// <summary>
    /// The hotkeys you want the HotkeyManager to listen for.
    /// </summary>
    public IReadOnlyList<Hotkey> Hotkeys { get; init; } = [];
}

/// <summary>
/// Defines a hotkey that the Manager should look for.
/// </summary>
public sealed record Hotkey
{
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
    /// True if you want to prevent the default behaviour of the key.
    /// </summary>
    public bool PreventDefault { get; init; } = true;
}