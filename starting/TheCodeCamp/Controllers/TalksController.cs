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
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : ApiController
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;

        public TalksController(ICampRepository campRepository, IMapper _mapper)
        {
            this.campRepository = campRepository;
            this.mapper = _mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeSpeakers = false)
        {
            try
            {
                var results = await campRepository.GetTalksByMonikerAsync(moniker,includeSpeakers);

                return Ok(mapper.Map<IEnumerable<TalkModel>>(results));

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
