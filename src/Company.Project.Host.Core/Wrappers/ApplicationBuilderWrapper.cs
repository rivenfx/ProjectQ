using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Wrappers
{
    public class ApplicationBuilderWrapper
    {
        private IApplicationBuilder _applicationBuilder;
        public IApplicationBuilder ApplicationBuilder
        {
            get
            {
                return this._applicationBuilder;
            }
            set
            {
                if (_applicationBuilder == null)
                {
                    this._applicationBuilder = value;
                }
            }
        }
    }
}
