using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Json_exe.Blazor.HotkeyManager;

public partial class HotkeyManagerContainer : ComponentBase, IAsyncDisposable
{
    [Inject] private HotkeyManager HotkeyManager { get; set; } = default!;
    [Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter] public HotkeyManagerOptions Options { get; set; } = default!;
    [Parameter] public EventCallback<KeyboardEventArgs> OnHotkeyPressed { get; set; }
    private ElementReference Container { get; set; }

    protected override void OnInitialized()
    {
        HotkeyManager.OnHotkeyPressed += HotkeyManagerOnOnHotkeyPressed;
        base.OnInitialized();
    }

    protected override async Task OnParametersSetAsync()
    {
        await HotkeyManager.Initialize(Options);
        await base.OnParametersSetAsync();
    }

    private Task HotkeyManagerOnOnHotkeyPressed(KeyboardEventArgs e)
    {
        return OnHotkeyPressed.InvokeAsync(e);
    }

    public async ValueTask DisposeAsync()
    {
        HotkeyManager.OnHotkeyPressed -= HotkeyManagerOnOnHotkeyPressed;
        await HotkeyManager.DisposeAsync();
    }
}