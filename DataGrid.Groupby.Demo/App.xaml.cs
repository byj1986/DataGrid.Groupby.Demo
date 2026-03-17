using System.Globalization;
using System.Windows;

namespace DataGrid.Groupby.Demo
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // TODO: 将下面的占位符替换为你在 https://www.syncfusion.com/products/communitylicense 获取的 License Key
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_SYNCFUSION_LICENSE_KEY");

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
