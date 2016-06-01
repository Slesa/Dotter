using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Dotter.Core.Actions
{
    public class SetFocusAction : TriggerAction<UIElement>
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(FrameworkElement), typeof(SetFocusAction), new FrameworkPropertyMetadata(null));

        public FrameworkElement Target
        {
            get { return (FrameworkElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            var target = Target ?? AssociatedObject;
            if (!target.Focusable || !target.IsEnabled) return;

            var targetScope = FocusManager.GetFocusScope(target);
            var targetScopeFocusedElement = FocusManager.GetFocusedElement(targetScope);
            if (!ReferenceEquals(targetScopeFocusedElement, target))
            {
                try
                {
                    FocusManager.SetFocusedElement(targetScope, target);
                    Debug.WriteLine("Set focused element in scope of " + targetScope + " to " + target);
                }
                catch (Exception)
                {
                    return;
                }
            }

            Debug.WriteLine("Focussing " + target);
            target.Focus();
            Keyboard.Focus(target);
        }
    }
}