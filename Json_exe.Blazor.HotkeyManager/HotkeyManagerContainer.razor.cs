using Microsoft.AspNetCore.Components;

namespace Json_exe.Blazor.HotkeyManager;

public partial class HotkeyManagerContainer : ComponentBase
{
    [Inject] private HotkeyManager HotkeyManager { get; set; } = default!;
    [Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = default!;
    private ElementReference Container { get; set; }
}