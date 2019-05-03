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

              
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

            return BadRequest(ModelState);

        }

        /// <summary>
        /// Update Talk
        /// </summary>
        /// <param name="moniker"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("{talkId:int}")]
        public async Task<IHttpActionResult> Put(string moniker, 
            int talkId, 
            TalkModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var talk = await campRepository.GetTalkByMonikerAsync(moniker, talkId, true);
                    if (talk == null) return NotFound(); //doesnt exists

                    //ignores the speaker 
                    mapper.Map(model, talk);

                    //so update the speaker here only if the speaker is changed for some reason
                    if(talk.Speaker.SpeakerId != model.Speaker.SpeakerId)
                    {
                        var speaker = await campRepository.GetSpeakerAsync(model.Speaker.SpeakerId);
                        if (speaker != null) talk.Speaker = speaker;
                    }

                    if(await campRepository.SaveChangesAsync())
                    {
                        return Ok(mapper.Map<TalkModel>(talk));
                    }
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }


        [Route("{talkId:int}")]
        public async Task<IHttpActionResult> Delete(string moniker,int talkId)
        {
            try
            {
                var talk = await campRepository.GetTalkByMonikerAsync(moniker, talkId);
                if (talk == null) return NotFound();

                //DELETE the Talk
                campRepository.DeleteTalk(talk);

                if(await campRepository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
