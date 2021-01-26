using System.Diagnostics;
using System.Windows;

namespace TMac.WebView2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            browser.Source = new System.Uri("https://www.cnn.com/");

            this.Loaded += (_, __) =>
            {
                Debug.WriteLine("Main Window Loaded");

                tabs.SelectedIndex = 1;
                tabs.UpdateLayout();
                tabs.SelectedIndex = 0;
            };

            browser.CoreWebView2InitializationCompleted += (_, __) =>
            {
                Debug.WriteLine("Web View Initialized");
            };
        }
    }
}
