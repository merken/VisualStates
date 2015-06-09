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
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(GotoStateAction), new PropertyMetadata(null));

        public override object Execute(object sender, object parameter)
        {
            if (Element != null && !String.IsNullOrEmpty(StateName))
            {
                var parent = Element.Parent as FrameworkElement;
                while (parent != null && parent as Control == null)
                {
                    parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
                }
                if (parent != null)
                {
                    var state = VisualStateManager.GoToState(parent as Control, StateName, true);
                    if (!state)
                        Debug.WriteLine("Could not go to state");
                    else
                        Debug.WriteLine("State ok");
                }
                return true;
            }
            return false;
        }
    }
}
