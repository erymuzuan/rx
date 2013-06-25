using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace routes.editor
{
    public class CustomFilterDescriptor : FilterDescriptorBase
    {
        private readonly CompositeFilterDescriptor m_compositeFilterDesriptor;
        private static readonly ConstantExpression m_trueExpression = Expression.Constant(true);
        private string m_filterValue;

        public CustomFilterDescriptor(IEnumerable<GridViewColumn> columns)
        {
            this.m_compositeFilterDesriptor = new CompositeFilterDescriptor
                {
                    LogicalOperator = FilterCompositionLogicalOperator.Or
                };

            foreach (GridViewDataColumn column in columns)
            {
                this.m_compositeFilterDesriptor.FilterDescriptors.Add(this.CreateFilterForColumn(column));
            }
        }

        public string FilterValue
        {
            get
            {
                return this.m_filterValue;
            }
            set
            {
                if (this.m_filterValue != value)
                {
                    this.m_filterValue = value;
                    this.UpdateCompositeFilterValues();
                    this.OnPropertyChanged("FilterValue");
                }
            }
        }

        protected override Expression CreateFilterExpression(ParameterExpression parameterExpression)
        {
            if (string.IsNullOrEmpty(this.FilterValue))
            {
                return m_trueExpression;
            }
            try
            {
                return this.m_compositeFilterDesriptor.CreateFilterExpression(parameterExpression);
            }
            catch
            {
            }

            return m_trueExpression;
        }

        private IFilterDescriptor CreateFilterForColumn(GridViewDataColumn column)
        {
            FilterOperator filterOperator = GetFilterOperatorForType(column.DataType);
            var descriptor = new FilterDescriptor(column.UniqueName, filterOperator, this.m_filterValue)
                {
                    MemberType = column.DataType
                };

            return descriptor;
        }

        private static FilterOperator GetFilterOperatorForType(Type dataType)
        {
            return dataType == typeof(string) ? FilterOperator.Contains : FilterOperator.IsEqualTo;
        }

        private void UpdateCompositeFilterValues()
        {
            foreach (FilterDescriptor descriptor in this.m_compositeFilterDesriptor.FilterDescriptors)
            {
                object convertedValue;

                try
                {
                    convertedValue = Convert.ChangeType(this.FilterValue, descriptor.MemberType, CultureInfo.CurrentCulture);
                }
                catch
                {
                    convertedValue = OperatorValueFilterDescriptorBase.UnsetValue;
                }

                if (descriptor.MemberType.IsAssignableFrom(typeof(DateTime)))
                {
                    DateTime date;
                    if (DateTime.TryParse(this.FilterValue, out date))
                    {
                        convertedValue = date;
                    }
                }

                descriptor.Value = convertedValue;
            }
        }

        private static object DefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}