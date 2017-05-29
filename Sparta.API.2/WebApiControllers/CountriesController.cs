using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sparta.Core.Interfaces.Services;
using Sparta.Core.Entities;

namespace Sparta.API._2.WebApiControllers
{
    public class CountriesController : ApiController
    {
        private readonly ICountryService _countryService;


        #region Constructors

        public CountriesController(ICountryService countryService)
            : base()
        {
            _countryService = countryService;

        }
        #endregion

        //public ActionResult Index()
        //{
        //    var GetAllCountries = _customerService.GetAllCountries();

        //    return View();
        //}

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("getCountries")]
        public IEnumerable<Region> GetAllCountries()
        {
            //try
            //{
            var GetAllCountries = _countryService.GetAllCountries();

            return GetAllCountries;

            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(ex.Message, ex);

            //    return new StoreResult { Message = ex.Message, Success = false };
            //}
        }
    }
}
