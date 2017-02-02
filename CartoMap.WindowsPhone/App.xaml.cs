using Carto.Ui;
using CartoMap.WindowsPhone.Pages;
using CartoMap.WindowsPhone.Pages.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CartoMap.WindowsPhone
{
    partial class App : Application
    {
        const string LICENSE = "XTUMwQ0ZGbzdnUXpoUndLSjcvYUZrdkZFaFpaUzVyVE1BaFVBb0RFM2lVQjYwdGhWSUdRU05PWnkrS3RTRUMwPQoKYXBwVG9rZW49MWM2YWQ0MDUtODg1Yi00NWI3LTg5ZTktZDZiNTU2NGRmMjgwCnByb2R1Y3RJZD1hZTgxZGZkNS01NDk3LTRiYzAtOWUyMC00MjEwNDRiNjFjNTkKcHJvZHVjdHM9c2RrLXdpbnBob25lLTQuKgpvbmxpbmVMaWNlbnNlPTEKd2F0ZXJtYXJrPWNhcnRvZGIK";

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// Invoked when the application is launched normally by the end user.  Other entry points
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Register license
            MapView.RegisterLicense(LICENSE);

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(LauncherListPage), e.Arguments);
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

    }
}
