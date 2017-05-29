using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta.Core.Entities
{
    /// <summary>
    ///     Custom injection attribute in order to get Ninject to fire property injection without needing
    ///     a dependency on Ninject itself.
    ///     http://www.joaoalmeida.info/2010/07/injecting-properties-in-ninject-2-without-inject-attribute/
    /// </summary>
    public class InventoryInjectionAttribute : Attribute
    {
    }
}