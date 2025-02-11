# Blazor Hotkey Manager

Blazor Hotkey Manager is a library for managing hotkeys in Blazor applications. It provides a simple and flexible way to handle keyboard shortcuts in your Blazor components.

## Installation

To install the NuGet package, run the following command in your NuGet Package Manager Console:

```sh
Install-Package Blazor.HotkeyManager
```

Alternatively, you can add the package reference directly to your .csproj file:

```cs
<PackageReference Include="Blazor.HotkeyManager" Version="1.0.0" />
```

## Building Locally

To build the package locally, follow these steps:

1. Clone the repository:

```sh
git clone https://github.com/yourusername/Blazor.HotkeyManager.git
```

2. Navigate to the project directory:

```sh
cd Blazor.HotkeyManager/Json_exe.Blazor.HotkeyManager
```

3. Restore the dependencies:

```sh
dotnet restore
```

4. Build the project:

```sh
dotnet build
```

## How to use?
1. Register the HotkeyManager on your Service provider:
```csharp
builder.Services.AddHotkeyManager();
```

2. To globally use the HotkeyManager in your program or on a whole page/layout, inject the HotkeyManager in your component:
```csharp
[Inject] private HotkeyManager HotkeyManager {get; init;}
```

3. In the OnAfterRender (or any other lifecycle method you like, that supports JSInterop), Initialize the HotkeyManager with your options like this:
```csharp
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
                    }
                ]
            });
```

4. Register on the OnHotkeyPressed event of the HotkeyManager to recieve hotkey events:
```csharp
HotkeyManager.OnHotkeyPressed += OnHotkeyManagerOnHotkeyPressed;
```

### Attention: Dont forget to Dispose the HotkeyManager and deregister afterwards when you are finished using it! Else it will lead to unexpected behaviour like your Hotkeys being triggered on another component.
```csharp
HotkeyManager.OnHotkeyPressed -= OnHotkeyManagerOnHotkeyPressed;
await HotkeyManager.DisposeAsync();
```

## Contributing
I welcome contributions from the community! If you have any ideas, suggestions, or bug reports, please open an issue or submit a pull request. Let's make Blazor Hotkey Manager better together!

## Technologies Used
- Blazor: A framework for building interactive web UIs with C#.
- .NET: A free, cross-platform, open-source developer platform for building many different types of applications.
- NuGet: A package manager for .NET.

Thank you for using Blazor Hotkey Manager ^^
