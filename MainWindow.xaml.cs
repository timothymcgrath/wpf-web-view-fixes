using System;
using System.Diagnostics;
using System.Windows;

namespace TMac.WebView2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Pre-renders the browser before the tab is clicked on, so the user immediately sees the web content when clicking on the tab.
            this.Loaded += (_, __) =>
            {
                Debug.WriteLine("Main Window Loaded");

                tabs.SelectedIndex = 1;
                tabs.UpdateLayout();
                tabs.SelectedIndex = 0;
            };

            Configure();
        }

        private void Configure()
        {
            browser.CoreWebView2InitializationCompleted += (_, e) =>
            {
                // Dispatcher BeginInvoke is to handle intermittent thread affinity errors when testing this fix.
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Debug.WriteLine("Web View Initialized");

                    if (e.IsSuccess && browser.CoreWebView2 != null)
                    {
                        browser.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;
                    }
                }));
            };

            browser.Source = new System.Uri("https://www.cnn.com/");
        }

        private void CoreWebView2_ProcessFailed(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ProcessFailedEventArgs e)
        {
            Debug.WriteLine($"WebView2 Process Failed - ProcessFailedKind:{e.ProcessFailedKind}");

            // Dispatcher BeginInvoke is to handle intermittent thread affinity errors when testing this fix.
            Dispatcher.BeginInvoke(new Action(() =>
            {
                // try/catch these calls because they sometimes error after the process failure.
                try
                {
                    webHost.Children.Clear();
                }
                catch { }

                try
                {
                    // Dispose to help eliminate a UI thread error when disposing the browser from the finalizer.
                    browser.Dispose();
                }
                catch { }

                browser = new Microsoft.Web.WebView2.Wpf.WebView2();
                webHost.Children.Add(browser);

                Configure();
            }));
        }
    }
}
