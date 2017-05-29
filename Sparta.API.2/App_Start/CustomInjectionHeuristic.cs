using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Ninject.Components;
using Ninject.Selection.Heuristics;
using Sparta.Core.Entities;

namespace Sparta.API._2.App_Start
{
  /// <summary>
    /// Custom injection heuristic to allow property injection without directly depending on Ninject.
    /// http://www.joaoalmeida.info/2010/07/injecting-properties-in-ninject-2-without-inject-attribute/
    /// </summary>
    public class CustomInjectionHeuristic : NinjectComponent, IInjectionHeuristic, INinjectComponent, IDisposable
    {
        public bool ShouldInject(MemberInfo member)
        {
            return member.IsDefined(typeof(InventoryInjectionAttribute), true);
        }
    }
}