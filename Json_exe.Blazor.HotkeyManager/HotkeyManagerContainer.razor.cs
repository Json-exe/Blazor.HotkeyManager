using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Json_exe.Blazor.HotkeyManager;

public partial class HotkeyManagerContainer : ComponentBase, IAsyncDisposable
{
    [Inject] private HotkeyManager HotkeyManager { get; set; } = default!;
    [Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter] public HotkeyManagerOptions Options { get; set; } = new();
    [Parameter] public EventCallback<KeyboardEventArgs> OnHotkeyPressed { get; set; }
    private ElementReference? Container { get; set; }

    protected override void OnInitialized()
    {
        HotkeyManager.OnHotkeyPressed += HotkeyManagerOnOnHotkeyPressed;
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (Container is not null)
            {
                var optionsWithContainer = Options with { Container = Container };
                await HotkeyManager.Initialize(optionsWithContainer);
            }
            else
            {
                await HotkeyManager.Initialize(Options);
            }
        }
        await base.OnAfterRenderAsync(firstRender);
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