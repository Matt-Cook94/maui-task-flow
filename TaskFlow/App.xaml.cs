using TaskFlow.Controls;

namespace TaskFlow
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            //Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoBorderEntry", (handler, view) =>
            //{
            //#if ANDROID
            //        handler.PlatformView.Background = null;
            //        handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            //        handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            //#elif IOS || MACCATALYST
            //        handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            //        handler.PlatformView.Layer.BorderWidth = 0;
            //        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
            //#elif WINDOWS
            //    handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
            //#endif
            //});
        }
    }
}
