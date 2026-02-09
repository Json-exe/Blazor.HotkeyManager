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
    private HotkeyManagerOptions? _loadedOptions;

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
        if (_loadedOptions is not null)
            throw new InvalidOperationException("This HotkeyManager instance has already been initialized!");

        _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Json_exe.Blazor.HotkeyManager/hotkeymanager.js");
        await _module.InvokeVoidAsync("initialize", _objectReference, options);
        _loadedOptions = options;
    }

    /// <summary>
    /// Will be called from JavaScript when a hotkey is pressed.
    /// </summary>
    /// <param name="e">The keyboard event args for the hotkey.</param>
    /// <param name="hotkeyId">The id of the hotkey.</param>
    [JSInvokable]
    public async ValueTask OnHotkey(KeyboardEventArgs e, Guid hotkeyId)
    {
        InvokeOnHotkeyPressed(e);
        var task = _loadedOptions?.Hotkeys.First(h => h.Id == hotkeyId).TriggerHotkeyEvent();
        if (task is not null) await task;
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
                // TODO: Align this disposal logic with the pattern recommended in the Blazor documentation for
                // JavaScript interop cleanup. In the official docs, a `<dispose-element>` pattern is shown as an
                // example of how to associate JS resources with a specific DOM element so they can be released
                // deterministically when the component is disposed. See, for example:
                // https://learn.microsoft.com/aspnet/core/blazor/javascript-interoperability/?view=aspnetcore-10.0#dom-cleanup-tasks-during-component-disposal
                // Evaluate whether this HotkeyManager should use a similar element-scoped disposal pattern or an
                // equivalent mechanism, and update the JS module and this call site accordingly.
                await _module.InvokeVoidAsync("dispose");
                await _module.DisposeAsync();
                _module = null;
            }
            catch (JSDisconnectedException)
            {
                // Ignore.
            }
        }

        _objectReference.Dispose();
        _loadedOptions = null;
    }
}