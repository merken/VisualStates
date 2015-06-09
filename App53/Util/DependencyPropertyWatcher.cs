using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace App53.Util
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
}
