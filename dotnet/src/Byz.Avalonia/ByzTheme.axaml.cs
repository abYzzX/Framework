using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Byz.Avalonia.Theme;

public partial class ByzTheme : Styles, IResourceNode
{
    public ByzTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
