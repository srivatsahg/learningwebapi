using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TheCodeCamp.Controllers
{
    /// <summary>
    /// A simple example to demonstrate Functional APIs 
    /// Example:
    /// APIs are not just for exchanging of data. 
    /// They provide much needed functionality for developers and integrators. 
    /// The functionality of an API can provide shipping costs as the Fedex API does, 
    /// or provide you with directions from San Francisco, CA to New York, NY as with the Google Maps API.
    /// 
    /// In this simple example, the RefreshConfiguration call refreshes the Web configuration file for any changes
    /// </summary>
    public class OperationsController : ApiController
    {
        [HttpOptions]
        [Route("api/refreshConfig")]
        public IHttpActionResult RefreshConfiguration()
        {
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                return Ok();
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }
    }
}
