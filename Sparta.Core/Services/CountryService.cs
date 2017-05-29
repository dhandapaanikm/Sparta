using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Entities;
using Sparta.Core.Interfaces;
using Sparta.Core.Interfaces.Services;

namespace Sparta.Core.Services
{

    public class CountryService : Service, ICountryService
    {
        #region Constructors

        public CountryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #endregion

        public IEnumerable<Region> GetAllCountries()
        {
            return UnitOfWork.Repository<Region>()
                .Query()
                .Get()
               .ToList();
        }

        public void Dispose()
        {
        }
    }
}
