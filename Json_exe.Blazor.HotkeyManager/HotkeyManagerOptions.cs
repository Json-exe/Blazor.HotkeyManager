using Microsoft.AspNetCore.Components;

namespace Json_exe.Blazor.HotkeyManager;

internal record HotkeyManagerOptions
{
    public RenderFragment? Container { get; init; } = null;
};