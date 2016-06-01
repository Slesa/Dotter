using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Dotter.Core.Triggers
{
    public enum KeyTriggerFiredOn
    {
        KeyDown,
        KeyUp,
    }

    /// <summary>
    /// Ein Auslöser, der durch ein Tastaturereignis ausgelöst wird.  Er löst aus, wenn die Zieltaste und Zielzusatztasten erkannt werden.
    /// 
    /// </summary>
    public class KeyTrigger : EventTriggerBase<UIElement>
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(KeyTrigger));
        public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyTrigger));
        public static readonly DependencyProperty ActiveOnFocusProperty = DependencyProperty.Register("ActiveOnFocus", typeof(bool), typeof(KeyTrigger));
        public static readonly DependencyProperty FiredOnProperty = DependencyProperty.Register("FiredOn", typeof(KeyTriggerFiredOn), typeof(KeyTrigger));
        private UIElement _targetElement;

        /// <summary>
        /// Die Taste, die gedrückt werden muss, damit der Auslöser auslöst.
        /// 
        /// </summary>
        public Key Key
        {
            get
            {
                return (Key)GetValue(KeyProperty);
            }
            set
            {
                SetValue(KeyProperty, value);
            }
        }

        /// <summary>
        /// Die Zusatztasten, die aktiv sein müssen, damit der Auslöser auslöst (standardmäßig sind keine Zusatztasten gedrückt).
        /// 
        /// </summary>
        public ModifierKeys Modifiers
        {
            get
            {
                return (ModifierKeys)GetValue(ModifiersProperty);
            }
            set
            {
                SetValue(ModifiersProperty, value);
            }
        }

        /// <summary>
        /// Wenn die Bedingung wahr ist, überwacht der Auslöser nur das zugehörige "Source"-Objekt des Auslösers, was bedeutet, dass dieses Element zum Auslösen den Fokus auf dem Auslöser haben muss. Wenn die Bedingung falsch ist, überwacht der Auslöser den Stamm, sodass alle nicht behandelten "KeyDown"- bzw. "KeyUp"-Meldungen abgefangen werden.
        /// 
        /// </summary>
        public bool ActiveOnFocus
        {
            get
            {
                return (bool)GetValue(ActiveOnFocusProperty);
            }
            set
            {
                SetValue(ActiveOnFocusProperty, value);
            }
        }

        /// <summary>
        /// Gibt an, ob das "KeyDown"- oder "KeyUp"-Ereignis überwacht wird.
        /// 
        /// </summary>
        public KeyTriggerFiredOn FiredOn
        {
            get
            {
                return (KeyTriggerFiredOn)GetValue(FiredOnProperty);
            }
            set
            {
                SetValue(FiredOnProperty, value);
            }
        }

        static KeyTrigger()
        {
        }

        protected override string GetEventName()
        {
            return "Loaded";
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key || Keyboard.Modifiers != GetActualModifiers(e.Key, Modifiers))
                return;
            e.Handled = true;
            InvokeActions(e);
        }

        private static ModifierKeys GetActualModifiers(Key key, ModifierKeys modifiers)
        {
            if (key == Key.LeftCtrl || key == Key.RightCtrl)
                modifiers |= ModifierKeys.Control;
            else if (key == Key.LeftAlt || key == Key.RightAlt || key == Key.System)
                modifiers |= ModifierKeys.Alt;
            else if (key == Key.LeftShift || key == Key.RightShift)
                modifiers |= ModifierKeys.Shift;
            return modifiers;
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            _targetElement = !ActiveOnFocus ? GetRoot(Source) : Source;
            if (FiredOn == KeyTriggerFiredOn.KeyDown)
                _targetElement.KeyDown += OnKeyPress;
            else
                _targetElement.KeyUp += OnKeyPress;
        }

        protected override void OnDetaching()
        {
            if (_targetElement != null)
            {
                if (FiredOn == KeyTriggerFiredOn.KeyDown)
                    _targetElement.KeyDown -= OnKeyPress;
                else
                    _targetElement.KeyUp -= OnKeyPress;
            }
            base.OnDetaching();
        }

        private static UIElement GetRoot(DependencyObject current)
        {
            UIElement uiElement = null;
            for (; current != null; current = VisualTreeHelper.GetParent(current))
                uiElement = current as UIElement;
            return uiElement;
        }
    }
}