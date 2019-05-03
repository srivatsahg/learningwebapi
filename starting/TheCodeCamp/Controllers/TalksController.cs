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
                var results = await campRepository.GetTalksByMonikerAsync(moniker, includeSpeakers);

                return Ok(mapper.Map<IEnumerable<TalkModel>>(results));

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{id:int}",Name ="GetTalk")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeSpeakers = false)
        {
            try
            {
                var result = await campRepository.GetTalkByMonikerAsync(moniker,id,includeSpeakers);
                if(result == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<TalkModel>(result));

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(string moniker, TalkModel model) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //STEP 1.   Get the camp first to assign the talk to the camp 
                    var camp = await campRepository.GetCampAsync(moniker);
                    if(camp != null)
                    {
                        //STEP 2.   Convert TalkModel to Talk
                        var talk = mapper.Map<Talk>(model);
                        talk.Camp = camp;

                        //MAP the speaker if necessary
                        var speaker = await campRepository.GetSpeakerAsync(model.Speaker.SpeakerId);
                        if (speaker != null) talk.Speaker = speaker;

                        //STEP 3.   Add to the repository and save changes
                        campRepository.AddTalk(talk);
                        if(await campRepository.SaveChangesAsync())
                        {
                            return CreatedAtRoute("GetTalk",
                                new { moniker = moniker, id = talk.TalkId },
                                mapper.Map<TalkModel>(talk));
                        }
                    }
                }

                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }
    }
}
