using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloMap.Forms.WP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        // Followed guides:
        // https://visualstudiomagazine.com/articles/2016/02/01/xamarin-forms.aspx
        // and
        // https://developer.xamarin.com/guides/xamarin-forms/platform-features/windows/installation/universal/
        // in order to set up UWP Xamarin.Forms project

        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Forms.App());
        }
    }
}
