using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App53
{
    public class DependencyPropertyWatcher : DependencyObject, IDisposable
    {
        private DependencyPropertyChangedEventHandler _handler;
        private readonly DependencyObject _target;

        public DependencyPropertyWatcher(DependencyObject target,
                                         string propertyPath,
                                         DependencyPropertyChangedEventHandler handler)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (propertyPath == null) throw new ArgumentNullException("propertyPath");
            if (handler == null) throw new ArgumentNullException("handler");

            _handler = handler;
            _target = target;

            var binding = new Binding
            {
                Source = _target,
                Path = new PropertyPath(propertyPath),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }

        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(object),
                typeof(DependencyPropertyWatcher),
                new PropertyMetadata(null, ValuePropertyChanged));

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var watcher = d as DependencyPropertyWatcher;
            if (watcher == null)
                return;

            watcher.OnValueChanged(e);
        }

        private void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var handler = _handler;
            if (handler != null)
                handler(_target, e);
        }

        public void Dispose()
        {
            _handler = null;
            // There is no ClearBinding method, so set a dummy binding instead
            BindingOperations.SetBinding(this, ValueProperty, new Binding());
        }
    }


    public static class DependencyPropertyWatcherExtensions
    {
        public static IDisposable WatchProperty(this DependencyObject target,
                                                string propertyPath,
                                                DependencyPropertyChangedEventHandler handler)
        {
            return new DependencyPropertyWatcher(target, propertyPath, handler);
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

    }
}
