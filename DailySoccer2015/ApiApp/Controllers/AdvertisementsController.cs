using ApiApp.Models;
using ApiApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiApp.Controllers
{
    /// <summary>
    /// Advertisements API
    /// </summary>
    [RoutePrefix("api/advertisements")]
    public class AdvertisementsController : ApiController
    {
        private IAdvertisementsRepository _repo;

        /// <summary>
        /// Initialize Advertisements API
        /// </summary>
        /// <param name="repo">Advertisements repository</param>
        public AdvertisementsController(IAdvertisementsRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Advertisements
        /// <summary>
        /// Get an advertisement
        /// </summary>
        [HttpGet]
        public AdvertisementInformation Get()
        {
            var ads = _repo.GetAdvertisement();
            if (ads == null) return null;

            var result = new AdvertisementInformation
            {
                ImageUrl = ads.ImageUrl,
                LinkUrl = ads.LinkUrl
            };
            return result;
        }
    }
}
