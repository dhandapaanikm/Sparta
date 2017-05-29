using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Entities;

namespace Sparta.Core.Interfaces.Services
{
   public interface ICountryService : IDisposable
    {
       IEnumerable<Region> GetAllCountries();
    }
}
