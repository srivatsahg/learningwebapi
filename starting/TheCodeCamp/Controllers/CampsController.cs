using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps")]
    public class CampsController : ApiController
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;

        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await campRepository.GetAllCampsAsync();
                //return BadRequest("we are learning");
                //return Ok(new { Name = "Srivatsa", Occupation = "Developer" });

                if(result == null)
                {
                    return NotFound();
                }
                //Mapping
                var mappedResult = mapper.Map<IEnumerable<CampModel>>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                //TODO:Logging
                return InternalServerError(ex);
            }

        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker)
        {
            try
            {
                var result = await campRepository.GetCampAsync(moniker);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<CampModel>(result));
            }
            catch (Exception)
            {

                return InternalServerError();
            }
        }
    }
}
