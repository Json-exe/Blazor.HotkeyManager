using Microsoft.AspNetCore.Components;

namespace Json_exe.Blazor.HotkeyManager.TestUI.Components.Pages;

public partial class Home : ComponentBase, IAsyncDisposable
{
    [Inject] private HotkeyManager HotkeyManager { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await HotkeyManager.Initialize(new HotkeyManagerOptions
            {
                Hotkeys = new []
                {
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
                    }
                }
            });
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public async ValueTask DisposeAsync()
    {
        await HotkeyManager.DisposeAsync();
    }
}