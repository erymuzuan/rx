using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;

namespace Bespoke.Cycling.Windows.RideOrganizerModule
{
    public class DraggablePushpin : Pushpin
    {
        private bool m_isDragging;
        MouseEventHandler m_parentMapMousePanHandler;
        MouseButtonEventHandler m_parentMapMouseLeftButtonUpHandler;
        MouseEventHandler m_parentMapMouseMoveHandler;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            // Check if the Map Event Handlers have been created/attached to the Map
            // If not, then attach them. This is done in the "Pushpin.OnMouseLeftButtonDown"
            // event because we don't know when the Pushpin is added to a Map or MapLayer, but
            // we do know that when this event is fired the Pushpin will already have been added.
            var parentLayer = this.Parent as MapLayer;
            if (parentLayer != null)
            {
                var parentMap = parentLayer;//.ParentMap;

                if (this.m_parentMapMousePanHandler == null)
                {
                    this.m_parentMapMousePanHandler = ParentMapMousePan;
                    parentMap.MouseMove += this.m_parentMapMousePanHandler;
                }
                if (this.m_parentMapMouseLeftButtonUpHandler == null)
                {
                    this.m_parentMapMouseLeftButtonUpHandler = ParentMapMouseLeftButtonUp;
                    parentMap.MouseLeftButtonUp += this.m_parentMapMouseLeftButtonUpHandler;
                }
                if (this.m_parentMapMouseMoveHandler == null)
                {
                    this.m_parentMapMouseMoveHandler = ParentMapMouseMove;
                    parentMap.MouseMove += this.m_parentMapMouseMoveHandler;
                }

            }

            // Enable Dragging
            this.m_isDragging = true;

            base.OnMouseLeftButtonDown(e);
        }


        void ParentMapMousePan(object sender, MouseEventArgs e)
        {
            // If the Pushpin is being dragged, specify that the Map's MousePan
            // event is handled. This is to suppress the Panning of the Map that
            // is done when the mouse drags the map.
            if (this.m_isDragging)
            {
                e.Handled = true;
            }
        }

        void ParentMapMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Left Mouse Button released, stop dragging the Pushpin
            this.m_isDragging = false;
        }

        void ParentMapMouseMove(object sender, MouseEventArgs e)
        {
            var map = sender as Map;
            // Check if the user is currently dragging the Pushpin
            if (this.m_isDragging)
            {
                // If so, the Move the Pushpin to where the Mouse is.
                var mouseMapPosition = e.GetPosition(map);
                if (map != null)
                {
                    var mouseGeocode = map.ViewportPointToLocation(mouseMapPosition);
                    this.Location = mouseGeocode;
                }
            }
        }

    }
}
