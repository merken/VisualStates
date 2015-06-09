using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App53
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string testData;
        public string TestData
        {
            get { return testData; }
            set { testData = value; RaisePropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }


        private void TestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty((sender as TextBox).Text))
                if (VisualStateManager.GoToState(this, (sender as TextBox).Text, true))
                    Debug.WriteLine("Page ok");
                else
                    Debug.WriteLine("page nok");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestBox.Text = "standard";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ViewModel).TestData = DateTime.Now.ToString();
        }

    }
}
