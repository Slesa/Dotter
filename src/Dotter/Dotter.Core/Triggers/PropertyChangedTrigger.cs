using System.Windows;
using System.Windows.Interactivity;
using Dotter.Core.Helpers;

namespace Dotter.Core.Triggers
{
    /// <summary>
    /// Stellt einen Auslöser dar, von dem Aktionen durchgeführt werden, wenn die gebundenen Daten geändert werden.
    /// 
    /// </summary>
    /// UA_REVIEW:chabiss
    public class PropertyChangedTrigger : TriggerBase<DependencyObject>
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(object), typeof(PropertyChangedTrigger), new PropertyMetadata(new PropertyChangedCallback(PropertyChangedTrigger.OnBindingChanged)));

        /// <summary>
        /// Ein Bindungsobjekt, das vom Auslöser überwacht wird und dafür sorgt, dass der Auslöser bei Änderungen ausgelöst wird.
        /// 
        /// </summary>
        /// UA_REVIEW:chabiss
        public object Binding
        {
            get
            {
                return this.GetValue(BindingProperty);
            }
            set
            {
                this.SetValue(BindingProperty, value);
            }
        }

        static PropertyChangedTrigger()
        {
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Bindungseigenschaft geändert wurde. UA_REVIEW:chabiss
        /// 
        /// </summary>
        /// <param name="args"><see cref="T:System.Windows.DependencyPropertyChangedEventArgs"/>-Argument.</param>
        protected internal virtual void EvaluateBindingChange(object args)
        {
            this.InvokeActions(args);
        }

        /// <summary>
        /// Wird nach dem Anfügen des Auslösers an das "AssociatedObject" aufgerufen. UA_REVIEW:chabiss
        /// 
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.PreviewInvoke += this.OnPreviewInvoke;
        }

        /// <summary>
        /// Wird aufgerufen, wenn der Auslöser vom "AssociatedObject" getrennt wird. Der Aufruf erfolgt vor dem eigentlichen Trennvorgang. UA_REVIEW:chabiss
        /// 
        /// </summary>
        protected override void OnDetaching()
        {
            this.PreviewInvoke -= this.OnPreviewInvoke;
            this.OnDetaching();
        }

        private void OnPreviewInvoke(object sender, PreviewInvokeEventArgs e)
        {
            DataBindingHelper.EnsureDataBindingOnActionsUpToDate(this);
        }

        private static void OnBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((PropertyChangedTrigger)sender).EvaluateBindingChange((object)args);
        }
    }
}