﻿using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class CustomAction : DomainObject
    {
        public virtual void Execute(RuleContext context)
        {
            throw new Exception("NotImplemented");
        }
        public virtual Task ExecuteAsync(RuleContext context)
        {
            throw new Exception("NotImplemented");
        }
        public virtual bool UseAsync
        {
            get
            {
                throw new Exception("NotImplemented");
            }
        }

        public virtual string GetEditorViewModel()
        {
            throw new NotImplementedException();
        }

        public virtual string GetEditorView()
        {
            throw new NotImplementedException();
        }
    }
}
