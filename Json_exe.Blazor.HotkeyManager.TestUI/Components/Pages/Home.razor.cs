using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Json_exe.Blazor.HotkeyManager.TestUI.Components.Pages;

public partial class Home : ComponentBase, IAsyncDisposable
{
    [Inject] private HotkeyManager HotkeyManager { get; set; } = null!;
    private string? _hotkeyPressed;
    private bool _ctrlKey;
    private bool _shiftKey;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await HotkeyManager.Initialize(new HotkeyManagerOptions
            {
                Hotkeys =
                [
                    new Hotkey
                    {
                        Key = "S",
                        CtrlKey = true,
                        PreventDefault = true
                    },
                    new Hotkey
                    {
                        Key = "F",
                        CtrlKey = true,
                        PreventDefault = true
                    },
                    new Hotkey
                    {
                        Key = "S",
                        ShiftKey = true,
                        PreventDefault = true
                    }
                ]
            });
            HotkeyManager.OnHotkeyPressed += HotkeyManagerOnOnHotkeyPressed;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private Task HotkeyManagerOnOnHotkeyPressed(KeyboardEventArgs e)
    {
        _hotkeyPressed = e.Key;
        _ctrlKey = e.CtrlKey;
        _shiftKey = e.ShiftKey;
        return InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        HotkeyManager.OnHotkeyPressed -= HotkeyManagerOnOnHotkeyPressed;
        await HotkeyManager.DisposeAsync();
    }
}