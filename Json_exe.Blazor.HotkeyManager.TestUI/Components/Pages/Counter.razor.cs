using Microsoft.AspNetCore.Components;

namespace Json_exe.Blazor.HotkeyManager.TestUI.Components.Pages;

public partial class Counter : ComponentBase 
{
    private readonly HotkeyManagerOptions _hotkeyManagerOptions = new()
    {
        Hotkeys =
        [
            new Hotkey
            {
                Key = "U",
                CtrlKey = true,
                PreventDefault = true
            }
        ]
    };
}