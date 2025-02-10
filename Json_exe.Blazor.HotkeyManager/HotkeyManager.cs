using System.Diagnostics;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Json_exe.Blazor.HotkeyManager;

public delegate Task OnHotkeyPressed(KeyboardEventArgs e);

public class HotkeyManager : IAsyncDisposable
{
    private IJSObjectReference? _module;
    private readonly IJSRuntime _jsRuntime;
    private readonly DotNetObjectReference<HotkeyManager> _objectReference;
    public event OnHotkeyPressed? OnHotkeyPressed;

    public HotkeyManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _objectReference = DotNetObjectReference.Create(this);
    }

    public async Task Initialize(HotkeyManagerOptions options)
    {
        if (_module is null)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Json_exe.Blazor.HotkeyManager/hotkeymanager.js");
            await _module.InvokeVoidAsync("initialized", _objectReference, options);
        }
    }

    [JSInvokable]
    public Task OnHotkey(KeyboardEventArgs e)
    {
        InvokeOnHotkeyPressed(e);
        return Task.CompletedTask;
    }

    private void InvokeOnHotkeyPressed(KeyboardEventArgs e)
    {
        OnHotkeyPressed?.Invoke(e);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("dispose");
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
            }
        }
    }
}