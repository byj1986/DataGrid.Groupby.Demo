using System.Globalization;
using System.Windows;

namespace DataGrid.Groupby.Demo
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //SetCultureInfo("en-US");
            SetCultureInfo("zh-CN");
            base.OnStartup(e);
        }

        static void SetCultureInfo(string cultureName)
        {
            var cultureInfo = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            Thread.CurrentThread.CurrentCulture = cultureInfo;

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(cultureInfo.IetfLanguageTag)
                )
            );

        }
    }
}
