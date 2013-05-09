using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Windows.Infrastructure
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OptionPanelMetadata: ExportAttribute, IOptionPanelMetadata
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; }

        public bool IsHidden { get; set; }
        public int Order { get; set; }
        public string Module { get; set; }
        public string Uri { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public string SubGroup { get;set; }
        public string Group { get; set; }
        public bool IsHome { get; set; }


        public OptionPanelMetadata()
            : base(typeof(IViewMetadata))
        {
            Name = "No Names View";
            Caption = "No Name View";
            IsHidden = false;
        }
        public OptionPanelMetadata(string contractName, Type contractType)
            : base(contractName, contractType)
        {
            Name = "No Names View";
            Caption = "No Name View";
            IsHidden = false;
        }

    }
}