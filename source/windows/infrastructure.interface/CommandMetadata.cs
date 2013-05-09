using System;
using System.ComponentModel.Composition;
using Bespoke.Cycling.Windows.Infrastructure;

namespace Bespoke.Sph.Windows.Infrastructure
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandMetadata : ExportAttribute, ICommandMetadata
    {
        public CommandMetadata()
            : base(typeof(ICommandMetadata))
        {
            Name = "No Names View";
            Caption = "No Name View";
            IsHidden = false;
        }


        public CommandMetadata(string contractName, Type contractType)
            : base(contractName, contractType)
        {
            Name = "No Names View";
            Caption = "No Name View";
            IsHidden = false;
        }

        public ViewGroup Group { get; set; }
        public string SubGroup { get; set; }
        public bool IsHidden { get; set; }
        public string Image { get; set; }
        public string Caption { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
    }
}