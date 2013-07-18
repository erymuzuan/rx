using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;

namespace routes.editor
{
    /// <summary>
    /// Provides row (item) reoder capabilities to RadGridView
    /// </summary>
    public class RowReorderBehavior
    {
        /// <summary>
        /// AssociatedObject Property
        /// </summary>
        public RadGridView AssociatedObject { get; set; }

        private static readonly Dictionary<RadGridView, RowReorderBehavior> instances;

        static RowReorderBehavior()
        {
            instances = new Dictionary<RadGridView, RowReorderBehavior>();
        }

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            RowReorderBehavior behavior = GetAttachedBehavior(obj as RadGridView);

            behavior.AssociatedObject = obj as RadGridView;

            if (value)
            {
                behavior.Initialize();
            }
            else
            {
                behavior.CleanUp();
            }
            obj.SetValue(IsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(RowReorderBehavior),
                new PropertyMetadata(OnIsEnabledPropertyChanged));

        public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            SetIsEnabled(dependencyObject, (bool)e.NewValue);
        }

        private static RowReorderBehavior GetAttachedBehavior(RadGridView gridview)
        {
            if (!instances.ContainsKey(gridview))
            {
                instances[gridview] = new RowReorderBehavior {AssociatedObject = gridview};
            }

            return instances[gridview];
        }

        protected virtual void Initialize()
        {
            this.AssociatedObject.RowLoaded -= this.AssociatedObjectRowLoaded;
            this.AssociatedObject.RowLoaded += this.AssociatedObjectRowLoaded;
            this.UnsubscribeFromDragDropEvents();
            this.SubscribeToDragDropEvents();
        }

        protected virtual void CleanUp()
        {
            this.AssociatedObject.RowLoaded -= this.AssociatedObjectRowLoaded;
            this.UnsubscribeFromDragDropEvents();
        }

        void AssociatedObjectRowLoaded(object sender, RowLoadedEventArgs e)
        {
            if (e.Row is GridViewHeaderRow || e.Row is GridViewNewRow || e.Row is GridViewFooterRow)
                return;

            var row = e.Row as GridViewRow;
            this.InitializeRowDragAndDrop(row);
        }

        private void InitializeRowDragAndDrop(GridViewRow row)
        {
            if (row == null)
                return;

            DragDropManager.RemoveDragOverHandler(row, OnRowDragOver);
            DragDropManager.AddDragOverHandler(row, OnRowDragOver);
        }

        private void SubscribeToDragDropEvents()
        {
            DragDropManager.AddDragInitializeHandler(this.AssociatedObject, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(this.AssociatedObject, OnGiveFeedback);
            DragDropManager.AddDropHandler(this.AssociatedObject, OnDrop);
        }

        private void UnsubscribeFromDragDropEvents()
        {
            DragDropManager.RemoveDragInitializeHandler(this.AssociatedObject, OnDragInitialize);
            DragDropManager.RemoveGiveFeedbackHandler(this.AssociatedObject, OnGiveFeedback);
            DragDropManager.RemoveDropHandler(this.AssociatedObject, OnDrop);
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs e)
        {
            var sourceRow = e.OriginalSource as GridViewRow ?? (e.OriginalSource as FrameworkElement).ParentOfType<GridViewRow>();
            if (sourceRow != null && sourceRow.Name != "PART_RowResizer")
            {
                var details = new DropIndicationDetails();
                var item = sourceRow.Item;
                details.CurrentDraggedItem = item;

                IDragPayload dragPayload = DragDropPayloadManager.GeneratePayload(null);

                dragPayload.SetData("DraggedItem", item);
                dragPayload.SetData("DropDetails", details);

                e.Data = dragPayload;

                e.DragVisual = new DragVisual
                    {
                    Content = details,
                    ContentTemplate = this.AssociatedObject.Resources["DraggedItemTemplate"] as DataTemplate
                };
                e.DragVisualOffset = e.RelativeStartPoint;
                e.AllowedEffects = DragDropEffects.All;
            }
        }

        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs e)
        {
            e.SetCursor(Cursors.Arrow);
            e.Handled = true;
        }

        private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var draggedItem = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedItem");
            var details = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as DropIndicationDetails;

            if (details == null || draggedItem == null)
            {
                return;
            }

            if (e.Effects == DragDropEffects.Move || e.Effects == DragDropEffects.All)
            {
                ((sender as RadGridView).ItemsSource as IList).Remove(draggedItem);
            }

            if (e.Effects != DragDropEffects.None)
            {
                var collection = (sender as RadGridView).ItemsSource as IList;
                int index = details.DropIndex < 0 ? 0 : details.DropIndex;
                index = details.DropIndex > collection.Count - 1 ? collection.Count : index;

                collection.Insert(index, draggedItem);
            }
        }

        private void OnRowDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var row = sender as GridViewRow;
            var details = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as DropIndicationDetails;

            if (details == null || row == null)
            {
                return;
            }

            details.CurrentDraggedOverItem = row.DataContext;

            if (details.CurrentDraggedItem == details.CurrentDraggedOverItem)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            details.CurrentDropPosition = GetDropPositionFromPoint(e.GetPosition(row), row);
            int dropIndex = (this.AssociatedObject.Items as IList).IndexOf(row.DataContext);
            int draggedItemIdex = (this.AssociatedObject.Items as IList).IndexOf(DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedItem"));

            if (dropIndex >= row.GridViewDataControl.Items.Count - 1 && details.CurrentDropPosition == DropPosition.After)
            {
                details.DropIndex = dropIndex;
                return;
            }

            dropIndex = draggedItemIdex > dropIndex ? dropIndex : dropIndex - 1;
            details.DropIndex = details.CurrentDropPosition == DropPosition.Before ? dropIndex : dropIndex + 1;
        }

        public virtual DropPosition GetDropPositionFromPoint(Point absoluteMousePosition, GridViewRow row)
        {
            if (row != null)
            {
                return absoluteMousePosition.Y < row.ActualHeight / 2 ? DropPosition.Before : DropPosition.After;
            }

            return DropPosition.Inside;
        }
    }
}