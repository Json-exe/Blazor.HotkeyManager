using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Json_exe.Blazor.HotkeyManager;

/// <summary>
/// The delegate for the OnHotkeyPressed event.
/// </summary>
public delegate Task OnHotkeyPressed(KeyboardEventArgs e);

/// <summary>
/// The HotkeyManager Service for talking to the JavaScript module.
/// </summary>
public sealed class HotkeyManager : IAsyncDisposable
{
    private IJSObjectReference? _module;
    private readonly IJSRuntime _jsRuntime;
    private readonly DotNetObjectReference<HotkeyManager> _objectReference;

    /// <summary>
    /// The event that is triggered when a hotkey is pressed.
    /// </summary>
    public event OnHotkeyPressed? OnHotkeyPressed;

    /// <summary>
    /// The constructor for the HotkeyManager for dependency injection.
    /// </summary>
    /// <param name="jsRuntime">
    /// The JavaScript runtime to use. Typically injected by the Blazor framework.
    /// </param>
    public HotkeyManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _objectReference = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Initializes the HotkeyManager with the given options.
    /// </summary>
    /// <param name="options"></param>
    public async Task Initialize(HotkeyManagerOptions options)
    {
        if (_module is null)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Json_exe.Blazor.HotkeyManager/hotkeymanager.js");
            await _module.InvokeVoidAsync("initialized", _objectReference, options);
        }
    }

    /// <summary>
    /// Will be called from JavaScript when a hotkey is pressed.
    /// </summary>
    /// <param name="e"></param>
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

    /// <inheritdoc />
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