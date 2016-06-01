using System;
using System.Globalization;
using System.Windows;
using System.Windows.Interactivity;
using Dotter.Core.Helpers;

namespace Dotter.Core.Actions
{
    /// <summary>
    /// Eine Aktion, die, wenn sie aufgerufen wird, ein "FrameworkElement" in einen angegebenen "VisualState" übergehen lässt.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// Wenn die "TargetName"-Eigenschaft gesetzt ist, versucht diese Aktion, den Zustand des Zielelements zu ändern. Wenn diese Eigenschaft nicht gesetzt ist, sucht die Aktion in der Elementstruktur nach einem anderen Ziel, das Zustände beschreibt. "ControlTemplate" und "UserControl" sind zwei häufig verwendete Möglichkeiten.
    /// 
    /// </remarks>
    public class GoToStateAction : TargetedTriggerAction<FrameworkElement>
    {
        #region Use transitions

        public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions"
            , typeof(bool)
            , typeof(GoToStateAction)
            , new PropertyMetadata(true));

        /// <summary>
        /// Gibt an, ob für den Übergang von einem Zustand in einen anderen ein "VisualTransition" verwendet wird.
        /// 
        /// </summary>
        public bool UseTransitions
        {
            get
            {
                return (bool)GetValue(UseTransitionsProperty);
            }
            set
            {
                SetValue(UseTransitionsProperty, value);
            }
        }

        #endregion

        #region State name

        public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register("StateName"
            , typeof(string)
            , typeof(GoToStateAction)
            , new PropertyMetadata(string.Empty));


        /// <summary>
        /// Der Name des "VisualState".
        /// 
        /// </summary>
        public string StateName
        {
            get
            {
                return (string)GetValue(StateNameProperty);
            }
            set
            {
                SetValue(StateNameProperty, value);
            }
        }

        #endregion


        private FrameworkElement StateTarget { get; set; }

        private bool IsTargetObjectSet
        {
            get
            {
                return ReadLocalValue(TargetObjectProperty) != DependencyProperty.UnsetValue;
            }
        }

        static GoToStateAction()
        {
        }

        /// <summary>
        /// Wird aufgerufen, wenn sich das Ziel ändert. Wenn die "TargetName"-Eigenschaft nicht gesetzt wird, weist diese Aktion ein benutzerdefiniertes Verhalten auf.
        /// 
        /// </summary>
        /// <param name="oldTarget"/><param name="newTarget"/><exception cref="T:System.InvalidOperationException">Es wurde kein entsprechendes "FrameworkElement" mit Zuständen gefunden.</exception>
        protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);
            var resolvedControl = (FrameworkElement) null;
            if (string.IsNullOrEmpty(TargetName) && !IsTargetObjectSet)
            {
                if (!VisualStateUtilities.TryFindNearestStatefulControl(AssociatedObject as FrameworkElement, out resolvedControl) && resolvedControl != null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                        "Target {0} does not define any VisualStateGroups.", new object[]
                        {
                            resolvedControl.Name
                        }));
            }
            else
                resolvedControl = Target;
            StateTarget = resolvedControl;
        }

        /// <summary>
        /// Diese Methode wird aufgerufen, wenn einige Kriterien erfüllt werden und die Aktion aufgerufen wird.
        /// 
        /// </summary>
        /// <param name="parameter"/><exception cref="T:System.InvalidOperationException">Das Ziel konnte nicht in den angegebenen "StateName" geändert werden.</exception>
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject == null)
                return;
            InvokeImpl(StateTarget);
        }

        internal void InvokeImpl(FrameworkElement stateTarget)
        {
            if (stateTarget == null)
                return;
            VisualStateUtilities.GoToState(stateTarget, StateName, UseTransitions);
        }
    }
}