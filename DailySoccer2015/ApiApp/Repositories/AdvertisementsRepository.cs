using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiApp.Models;
using ApiApp.MongoAccess;
using MongoDB.Driver;

namespace ApiApp.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับโฆษณา
    /// </summary>
    public class AdvertisementsRepository : IAdvertisementsRepository
    {
        #region Fields

        private const string TableName = "dailysoccer.Advertisements";

        #endregion Fields

        #region IAdvertisementsRepository members

        /// <summary>
        /// ดึงข้อมูลโฆษณา
        /// </summary>
        public Advertisement GetAdvertisement()
        {
            var qry = MongoUtil.GetCollection<Advertisement>(TableName)
               .Find(it => true)
               .FirstOrDefault();
            return qry;
        }

        #endregion IAdvertisementsRepository members
    }
}
