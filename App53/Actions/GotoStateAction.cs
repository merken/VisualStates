using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace App53.Actions
{
    public class GotoStateAction : ActionBase
    {
        private Control _control;

        public string StateName
        {
            get { return (string)GetValue(StateNameProperty); }
            set { SetValue(StateNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StateName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateNameProperty =
            DependencyProperty.Register("StateName", typeof(string), typeof(GotoStateAction), new PropertyMetadata(null));

        public FrameworkElement Element
        {
            get { return (FrameworkElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Element.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(GotoStateAction), new PropertyMetadata(null, OnElementPropertyChanged));

        private static void OnElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var action = d as GotoStateAction;
            var newElement = e.NewValue as FrameworkElement;

            action._control = GetControl(newElement);
        }

        private static Control GetControl(FrameworkElement element)
        {
            Control control = null;
            var parent = element.Parent;
            if (parent is Control)
                control = parent as Control;
            else
            {
                while (parent != null && parent as Control == null)
                {
                    parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
                }
                control = parent as Control;
            }
            return control;
        }

        public override object Execute(object sender, object parameter)
        {
            if (_control == null)
                _control = GetControl(sender as FrameworkElement);

            if (_control != null && !String.IsNullOrEmpty(StateName))
            {
                var state = VisualStateManager.GoToState(_control, StateName, true);
                return state;
            }
            return false;
        }
    }
}
