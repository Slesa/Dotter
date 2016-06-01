using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Dotter.Core.Helpers
{
    /// <summary>
    /// "Helper"-Klasse zum Verwalten von Bindungsausdrücken für Abhängigkeitsobjekte.
    /// 
    /// </summary>
    internal static class DataBindingHelper
    {
        private static readonly Dictionary<Type, IList<DependencyProperty>> DependenciesPropertyCache = new Dictionary<Type, IList<DependencyProperty>>();

        static DataBindingHelper()
        {
        }

        /// <summary>
        /// Es muss sichergestellt werden, dass alle DPs für eine Aktion mit Bindungsausdrücken aktuell sind. "DataTrigger" wird während der Datenbindungsphase ausgelöst. Da Aktionen untergeordnete Elemente des Auslösers sind, sind Bindungen für die Aktion möglicherweise nicht aktuell. Diese Routine wird vor dem Aufruf der Aktion aufgerufen, um sicherzustellen, dass alle Bindungen mit den aktuellsten Daten aktualisiert wurden.
        /// 
        /// </summary>
        public static void EnsureDataBindingUpToDateOnMembers(DependencyObject dpObject)
        {
            IList<DependencyProperty> list = (IList<DependencyProperty>)null;
            if (!DataBindingHelper.DependenciesPropertyCache.TryGetValue(dpObject.GetType(), out list))
            {
                list = (IList<DependencyProperty>)new List<DependencyProperty>();
                for (Type type = dpObject.GetType(); type != (Type)null; type = type.BaseType)
                {
                    foreach (FieldInfo fieldInfo in type.GetFields())
                    {
                        if (fieldInfo.IsPublic && fieldInfo.FieldType == typeof(DependencyProperty))
                        {
                            var dependencyProperty = fieldInfo.GetValue((object)null) as DependencyProperty;
                            if (dependencyProperty != null)
                                list.Add(dependencyProperty);
                        }
                    }
                }
                DataBindingHelper.DependenciesPropertyCache[dpObject.GetType()] = list;
            }
            if (list == null)
                return;
            foreach (DependencyProperty dp in (IEnumerable<DependencyProperty>)list)
                DataBindingHelper.EnsureBindingUpToDate(dpObject, dp);
        }

        /// <summary>
        /// Stellt sicher, dass alle Bindungsausdrücke für Aktionen aktuell sind.
        /// 
        /// </summary>
        public static void EnsureDataBindingOnActionsUpToDate(System.Windows.Interactivity.TriggerBase<DependencyObject> trigger)
        {
            foreach (DependencyObject dpObject in (FreezableCollection<System.Windows.Interactivity.TriggerAction>)trigger.Actions)
                DataBindingHelper.EnsureDataBindingUpToDateOnMembers(dpObject);
        }

        /// <summary>
        /// Mit dieser Helferfunktion wird sichergestellt, dass der Bindungsausdruck aktuell ist, wenn eine Abhängigkeitseigenschaft für ein Abhängigkeitsobjekt über einen Bindungsausdruck verfügt.
        /// 
        /// </summary>
        /// <param name="target"/><param name="dp"/>
        public static void EnsureBindingUpToDate(DependencyObject target, DependencyProperty dp)
        {
            BindingExpression bindingExpression = BindingOperations.GetBindingExpression(target, dp);
            if (bindingExpression == null)
                return;
            bindingExpression.UpdateTarget();
        }
    }
}