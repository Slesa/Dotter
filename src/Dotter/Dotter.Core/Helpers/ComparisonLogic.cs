using System;
using System.Globalization;

namespace Dotter.Core.Helpers
{
    internal static class ComparisonLogic
    {
        /// <summary>
        /// Mit dieser Methode werden Operanden ausgewertet.
        /// 
        /// </summary>
        /// <param name="leftOperand">Linker Operand aus der "LeftOperand"-Eigenschaft.</param><param name="operatorType">Operator aus "Operator"-Eigenschaft.</param><param name="rightOperand">Rechter Operand aus der "RightOperand"-Eigenschaft.</param>
        /// <returns>
        /// Gibt "true" zurück, wenn die Bedingung erfüllt ist, gibt andernfalls "false" zurück.
        /// </returns>
        internal static bool EvaluateImpl(object leftOperand, ComparisonConditionType operatorType, object rightOperand)
        {
            var flag = false;
            if (leftOperand != null)
            {
                var type = leftOperand.GetType();
                if (rightOperand != null)
                    rightOperand = TypeConverterHelper.DoConversionFrom(TypeConverterHelper.GetTypeConverter(type), rightOperand);
            }
            var leftOperand1 = leftOperand as IComparable;
            var rightOperand1 = rightOperand as IComparable;
            if (leftOperand1 != null && rightOperand1 != null)
                return EvaluateComparable(leftOperand1, operatorType, rightOperand1);
            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    flag = Equals(leftOperand, rightOperand);
                    break;
                case ComparisonConditionType.NotEqual:
                    flag = !Equals(leftOperand, rightOperand);
                    break;
                case ComparisonConditionType.LessThan:
                case ComparisonConditionType.LessThanOrEqual:
                case ComparisonConditionType.GreaterThan:
                case ComparisonConditionType.GreaterThanOrEqual:
                    if (leftOperand1 == null && rightOperand1 == null)
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "LeftOperand of type {1} and RightOperand of type {0} cannot be used with operator {2}."
                            , leftOperand != null ? (object)leftOperand.GetType().Name : (object)"null"
                            , rightOperand != null ? (object)rightOperand.GetType().Name : (object)"null"
                            , (object)((object)operatorType).ToString()));
                    else if (leftOperand1 == null)
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "LeftOperand of type {0} cannot be used with operator {1}.", new object[]
            {
              leftOperand != null ? leftOperand.GetType().Name : "null",
              ((object) operatorType).ToString()
            }));
                    else
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "RightOperand of type {0} cannot be used with operator {1}.", new object[]
            {
              rightOperand != null ? rightOperand.GetType().Name : "null",
              ((object) operatorType).ToString()
            }));
            }
            return flag;
        }

        /// <summary>
        /// Wertet beide Operanden aus, mit denen die "IComparable"-Schnittstelle implementiert wird.
        /// 
        /// </summary>
        /// <param name="leftOperand">Linker Operand aus der "LeftOperand"-Eigenschaft.</param><param name="operatorType">Operator aus "Operator"-Eigenschaft.</param><param name="rightOperand">Rechter Operand aus der "RightOperand"-Eigenschaft.</param>
        /// <returns>
        /// Gibt "true" zurück, wenn die Bedingung erfüllt ist, gibt andernfalls "false" zurück.
        /// </returns>
        private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
        {
            object obj = null;
            try
            {
                obj = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
            }
            catch (FormatException /*ex*/)
            {
            }
            catch (InvalidCastException /*ex*/)
            {
            }
            if (obj == null)
                return operatorType == ComparisonConditionType.NotEqual;
            int num = leftOperand.CompareTo(obj);
            bool flag = false;
            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    flag = num == 0;
                    break;
                case ComparisonConditionType.NotEqual:
                    flag = num != 0;
                    break;
                case ComparisonConditionType.LessThan:
                    flag = num < 0;
                    break;
                case ComparisonConditionType.LessThanOrEqual:
                    flag = num <= 0;
                    break;
                case ComparisonConditionType.GreaterThan:
                    flag = num > 0;
                    break;
                case ComparisonConditionType.GreaterThanOrEqual:
                    flag = num >= 0;
                    break;
            }
            return flag;
        }
    }
}