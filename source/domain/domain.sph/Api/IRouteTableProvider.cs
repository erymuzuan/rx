using System.Collections.Generic;

namespace Bespoke.Sph.Domain.Api
{
    public interface IRouteTableProvider
    {
        string GetEditorViewModel(JsRoute route);
        string GetEditorView(JsRoute route);
        IEnumerable<JsRoute> Routes { get; }
    }
}