using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RideViewMetadata: ExportAttribute, IViewMetadata
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; }

        public bool IsHidden { get; set; }
        public int Order { get; set; }
        public string Module { get; set; }
        public string Uri { get; set; }
        public string Role { get; set; }


        public RideViewMetadata()
            : base(typeof(IViewMetadata))
        {
            Name = "No Names View";
            Caption = "No Name View";
            IsHidden = false;
        }

    }

    public interface IViewMetadata
    {
        string Name { get; }
        string Caption { get; }
        string Image { get; }
        string Uri { get; }
        string Module { get; }
        [DefaultValue(false)]
        bool IsHidden { get; }
        int Order { get; }
        string Role { get; }
    }
}
