using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Json_exe.Blazor.HotkeyManager.TestUI.Components.Pages;

public partial class Home : ComponentBase, IAsyncDisposable
{
    [Inject] private HotkeyManager HotkeyManager { get; set; } = null!;
    private string _hotkeyPressed = string.Empty;
    private bool _ctrlKey;
    private bool _shiftKey;
    private string _hotkeyMessage = string.Empty;

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
                        PreventDefault = true,
                        OnHotkeyTriggered = OnHotkeySCTRLTriggered
                    },
                    new Hotkey
                    {
                        Key = "F",
                        CtrlKey = true,
                        PreventDefault = true,
                        OnHotkeyTriggeredAsync = OnHotkeyTriggeredAsync
                    },
                    new Hotkey
                    {
                        Key = "S",
                        ShiftKey = true,
                        PreventDefault = true
                    },
                    new Hotkey
                    {
                        Key = "S",
                        AltKey = true,
                        PreventDefault = true
                    }
                ]
            });
            HotkeyManager.OnHotkeyPressed += HotkeyManagerOnOnHotkeyPressed;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private Task OnHotkeyTriggeredAsync()
    {
        _hotkeyMessage = "Hello from the F + CTRL hotkey! This was run ASYNC!";
        return InvokeAsync(StateHasChanged);
    }

    private void OnHotkeySCTRLTriggered()
    {
        _hotkeyMessage = "Hello from the S + CTRL hotkey!";
        InvokeAsync(StateHasChanged);
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