using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace App53.Behaviors
{
    [ContentProperty(Name="Actions")]
    public class DataWatcherBehavior : BehaviorBase<FrameworkElement>
    {
        public ActionCollection Actions
        {
            get
            {
                ActionCollection actionCollection = (ActionCollection)base.GetValue(DataWatcherBehavior.ActionsProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    base.SetValue(DataWatcherBehavior.ActionsProperty, actionCollection);
                }
                return actionCollection;
            }
        }

        // Using a DependencyProperty as the backing store for Actions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(DataWatcherBehavior), new PropertyMetadata(null));

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(DataWatcherBehavior), new PropertyMetadata(null, OnDataPropertyChanged));

        private static void OnDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsBehaviorActive(d, e))
            {
                var behavior = GetBehavior<DataWatcherBehavior>(d);
                behavior.ExecuteActions(behavior.AssociatedElement, behavior.Data);
            }
        }

        private async void ExecuteActions(object sender, object parameter)
        {
            await Task.Delay(1);
            foreach (IAction action in Actions)
                action.Execute(sender, parameter);
        }
    }
}
