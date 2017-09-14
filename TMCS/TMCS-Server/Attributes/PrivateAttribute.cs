using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMCS.Server.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
                           Inherited = false,
                           AllowMultiple = false)]
    sealed class PrivateAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string positionalString;

        // This is a positional argument
        public PrivateAttribute(string positionalString)
        {
            this.positionalString = positionalString;

            // TODO: Implement code here

            throw new NotImplementedException();
        }

        public string PositionalString
        {
            get { return positionalString; }
        }

        // This is a named argument
        public int NamedInt { get; set; }
    }
}
