using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dotter.Core.Helpers
{
    /// <summary>
    /// Diese Klasse stellt verschiedene plattformagnostische Standardoperationen für die Verwendung mit "VisualStateManager" bereit.
    /// 
    /// </summary>
    public static class VisualStateUtilities
    {
        /// <summary>
        /// Führt Übergang des Steuerelements von einem Zustand in einen anderen durch.
        /// 
        /// </summary>
        /// <param name="element">Das Element, für das der Übergang von einem Zustand in einen anderen durchgeführt wird.</param><param name="stateName">Der Zustand, in den übergegangen wird.</param><param name="useTransitions">"true", um eine "System.Windows.VisualTransition" für den Übergang von einem Zustand in einen anderen zu verwenden; andernfalls "false".</param>
        /// <returns>
        /// "true", wenn der Übergang des Steuerelements in den neuen Zustand erfolgreich durchgeführt wurde; andernfalls "false".
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">"control" ist Null.</exception><exception cref="T:System.ArgumentNullException">"stateName" ist Null.</exception>
        public static bool GoToState(FrameworkElement element, string stateName, bool useTransitions)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(stateName))
            {
                Control control = element as Control;
                if (control != null)
                {
                    control.ApplyTemplate();
                    flag = VisualStateManager.GoToState((FrameworkElement)control, stateName, useTransitions);
                }
                else
                    flag = VisualStateManager.GoToElementState(element, stateName, useTransitions);
            }
            return flag;
        }

        /// <summary>
        /// Ruft den Wert der an "VisualStateManager.VisualStateGroups" angefügten Eigenschaft ab.
        /// 
        /// </summary>
        /// <param name="targetObject">Das Element, aus dem die "VisualStateManager.VisualStateGroups" abgerufen wird.</param>
        /// <returns/>
        public static IList GetVisualStateGroups(FrameworkElement targetObject)
        {
            IList list = (IList)new List<VisualStateGroup>();
            if (targetObject != null)
            {
                list = VisualStateManager.GetVisualStateGroups(targetObject);
                if (list.Count == 0 && VisualTreeHelper.GetChildrenCount((DependencyObject)targetObject) > 0)
                    list = VisualStateManager.GetVisualStateGroups(VisualTreeHelper.GetChild((DependencyObject)targetObject, 0) as FrameworkElement);
                if (list.Count == 0)
                {
                    UserControl userControl = targetObject as UserControl;
                    if (userControl != null)
                    {
                        FrameworkElement frameworkElement = userControl.Content as FrameworkElement;
                        if (frameworkElement != null)
                            list = VisualStateManager.GetVisualStateGroups(frameworkElement);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Es wird nach dem nächsten übergeordneten Element gesucht, das visuelle Zustände enthält.
        /// 
        /// </summary>
        /// <param name="contextElement">Das Element, bei dem das nächste zustandsbehaftete Steuerelement gefunden wird.</param><param name="resolvedControl">Das nächste statusbehaftete Steuerelement, wenn "true"; andernfalls "Null".</param>
        /// <returns>
        /// "true", wenn ein übergeordnetes Element visuelle Zustände enthält; andernfalls "false".
        /// </returns>
        public static bool TryFindNearestStatefulControl(FrameworkElement contextElement, out FrameworkElement resolvedControl)
        {
            FrameworkElement frameworkElement = contextElement;
            if (frameworkElement == null)
            {
                resolvedControl = (FrameworkElement)null;
                return false;
            }
            else
            {
                FrameworkElement element = frameworkElement.Parent as FrameworkElement;
                bool flag = true;
                for (; !VisualStateUtilities.HasVisualStateGroupsDefined(frameworkElement) && VisualStateUtilities.ShouldContinueTreeWalk(element); element = element.Parent as FrameworkElement)
                    frameworkElement = element;
                if (VisualStateUtilities.HasVisualStateGroupsDefined(frameworkElement))
                {
                    if (frameworkElement.TemplatedParent != null && frameworkElement.TemplatedParent is Control)
                        frameworkElement = frameworkElement.TemplatedParent as FrameworkElement;
                    else if (element != null && element is UserControl)
                        frameworkElement = element;
                }
                else
                    flag = false;
                resolvedControl = frameworkElement;
                return flag;
            }
        }

        private static bool HasVisualStateGroupsDefined(FrameworkElement frameworkElement)
        {
            if (frameworkElement != null)
                return VisualStateManager.GetVisualStateGroups(frameworkElement).Count != 0;
            else
                return false;
        }

        internal static FrameworkElement FindNearestStatefulControl(FrameworkElement contextElement)
        {
            FrameworkElement resolvedControl = (FrameworkElement)null;
            VisualStateUtilities.TryFindNearestStatefulControl(contextElement, out resolvedControl);
            return resolvedControl;
        }

        private static bool ShouldContinueTreeWalk(FrameworkElement element)
        {
            if (element == null || element is UserControl)
                return false;
            if (element.Parent == null)
            {
                FrameworkElement templatedParent = VisualStateUtilities.FindTemplatedParent(element);
                if (templatedParent == null || !(templatedParent is Control) && !(templatedParent is ContentPresenter))
                    return false;
            }
            return true;
        }

        private static FrameworkElement FindTemplatedParent(FrameworkElement parent)
        {
            return parent.TemplatedParent as FrameworkElement;
        }
    }
}