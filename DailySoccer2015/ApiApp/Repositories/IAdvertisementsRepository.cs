using ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับโฆษณา
    /// </summary>
    public interface IAdvertisementsRepository
    {
        #region Methods

        /// <summary>
        /// ดึงข้อมูลโฆษณา
        /// </summary>
        Advertisement GetAdvertisement();

        #endregion Methods
    }
}
