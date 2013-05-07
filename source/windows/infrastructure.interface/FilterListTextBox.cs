using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Bespoke.Cycling.Windows.Infrastructure
{
    public class FilterListTextBox : DependencyObject
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
            "ItemsSource",
            typeof(object),
            typeof(FilterListTextBox),
            new PropertyMetadata(false, ItemsSourcePropertyChanged));

        private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (null == textBox) return;
            var views = CollectionViewSource.GetDefaultView(e.NewValue);
            if (null != views)
            {
                textBox.TextChanged +=
                    (s, arg) =>
                    {
                        var text = textBox.Text.ToLower();
                        if (string.IsNullOrWhiteSpace(text))
                        {
                            views.Filter = null;
                            return;
                        }
                        views.Filter = o => string.Format("{0}", o).ToLower().Contains(text);
                    };
            }
        }



        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetItemsSource(DependencyObject @object)
        {
            return (bool)@object.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject @object, bool value)
        {
            @object.SetValue(ItemsSourceProperty, value);
        }
    }
}