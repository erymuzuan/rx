using System;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    public interface IFormRendererMetadata
    {
        Type FormType { get;  }
        string Text { get;  }
    }
}