using System.Windows;
using Dotter.Core.Helpers;

namespace Dotter.Core.Triggers
{
    /// <summary>
    /// Stellt einen Auslöser dar, von dem Aktionen durchgeführt werden, wenn die gebundenen Daten eine angegebene Bedingung erfüllen.
    /// 
    /// </summary>
    public class DataTrigger : PropertyChangedTrigger
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", 
            typeof (object),
            typeof (DataTrigger), new PropertyMetadata(OnValueChanged));

        public static readonly DependencyProperty ComparisonProperty = DependencyProperty.Register("Comparison",
            typeof (ComparisonConditionType), 
            typeof (DataTrigger), new PropertyMetadata(OnComparisonChanged));

        /// <summary>
        /// Ruft den Wert ab, der mit der dem Eigenschaftswert des Datenobjekts verglichen wird, oder legt den Wert fest. Hierbei handelt es sich um eine Abhängigkeitseigenschaft.
        /// 
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Ruft den den Typ des Vergleichs ab, der für die angegebenen Werte durchgeführt werden soll, oder legt den Typ fest. Hierbei handelt es sich um eine Abhängigkeitseigenschaft.
        /// 
        /// </summary>
        public ComparisonConditionType Comparison
        {
            get
            {
                return (ComparisonConditionType) GetValue(ComparisonProperty);
            }
            set
            {
                this.SetValue(ComparisonProperty, value);
            }
        }

        static DataTrigger()
        {
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Bindungseigenschaft geändert wurde. UA_REVIEW:chabiss
        /// 
        /// </summary>
        /// <param name="args"><see cref="T:System.Windows.DependencyPropertyChangedEventArgs"/>-Argument.</param>
        protected internal override void EvaluateBindingChange(object args)
        {
            if (!this.Compare())
                return;
            this.InvokeActions(args);
        }

        static void OnValueChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((PropertyChangedTrigger) sender).EvaluateBindingChange((object) args);
        }

        static void OnComparisonChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((PropertyChangedTrigger) sender).EvaluateBindingChange((object) args);
        }

        bool Compare()
        {
            if (this.AssociatedObject != null)
                return ComparisonLogic.EvaluateImpl(this.Binding, this.Comparison, this.Value);
            else
                return false;
        }
    }
}