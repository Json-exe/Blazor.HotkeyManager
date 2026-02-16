using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Json_exe.Blazor.HotkeyManager;

/// <summary>
/// A container for the <see cref="HotkeyManager"/> that initializes the HotkeyManager and
/// only apply hotkeys inside the container.
/// </summary>
public sealed partial class HotkeyManagerContainer : ComponentBase, IAsyncDisposable
{
    [Inject] private HotkeyManager HotkeyManager { get; set; } = null!;

    /// <summary>
    /// Additional classes you want to apply to the container wrapping your child content.
    /// </summary>
    [Parameter]
    public string Class { get; set; } = string.Empty;

    /// <summary>
    /// Additional styles you want to apply to the container wrapping your child content.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// The child content to be rendered.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    /// The options to be used by the <see cref="HotkeyManager"/>.
    /// </summary>
    [Parameter]
    public HotkeyManagerOptions Options { get; set; } = new();

    /// <summary>
    /// The event that is triggered when any hotkey is pressed providing the <see cref="KeyboardEventArgs"/> when
    /// this hotkey was pressed.
    /// </summary>
    [Parameter]
    public EventCallback<KeyboardEventArgs> OnHotkeyPressed { get; set; }

    private ElementReference? _container;
    private HotkeyManagerOptions? _loadedOptions;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        HotkeyManager.OnHotkeyPressed += HotkeyManagerOnOnHotkeyPressed;
        base.OnInitialized();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender && _loadedOptions?.Hotkeys == Options.Hotkeys) return;
        if (_container is not null)
        {
            _loadedOptions = Options with { Container = _container };
        }
        else
        {
            _loadedOptions = Options with { };
        }

        await HotkeyManager.Initialize(_loadedOptions, _cancellationTokenSource.Token);
    }

    private Task HotkeyManagerOnOnHotkeyPressed(KeyboardEventArgs e)
    {
        return OnHotkeyPressed.InvokeAsync(e);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync();
        HotkeyManager.OnHotkeyPressed -= HotkeyManagerOnOnHotkeyPressed;
        await HotkeyManager.DisposeAsync();
    }
}