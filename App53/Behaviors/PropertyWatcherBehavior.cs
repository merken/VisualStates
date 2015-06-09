using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using App53.Util;

namespace App53.Behaviors
{
    [ContentProperty(Name = "Actions")]
    public class PropertyWatcherBehavior : BehaviorBase<FrameworkElement>
    {
        IDisposable _watcher;

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(PropertyWatcherBehavior), new PropertyMetadata(null));

        public ActionCollection Actions
        {
            get
            {
                ActionCollection actionCollection = (ActionCollection)base.GetValue(PropertyWatcherBehavior.ActionsProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    base.SetValue(PropertyWatcherBehavior.ActionsProperty, actionCollection);
                }
                return actionCollection;
            }
        }

        // Using a DependencyProperty as the backing store for Actions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(PropertyWatcherBehavior), new PropertyMetadata(null));

        protected override void ElementAttached()
        {
            base.ElementAttached();
            _watcher = AssociatedElement.WatchProperty(PropertyName, OnPropertyChanged);
        }

        protected override void ElementDetached()
        {
            _watcher.Dispose();
            base.ElementDetached();
        }

        private async void OnPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            await Task.Delay(1);
            foreach (IAction action in Actions)
                action.Execute(sender, e.NewValue);
        }
    }
}
