﻿using AutoMapper;
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
        public async Task<IHttpActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var result = await campRepository.GetAllCampsAsync(includeTalks);
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


        [Route("searchByDate/{eventDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false)
        {
            try
            {
                var result = await campRepository.GetAllCampsByEventDate(eventDate,includeTalks);
                
                if (result == null)
                {
                    return NotFound();
                }

                //Mapping
                var mappedResult = mapper.Map<CampModel[]>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                //TODO:Logging
                return InternalServerError(ex);
            }

        }

        [Route("{moniker}", Name = "GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await campRepository.GetCampAsync(moniker,includeTalks);

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

        [Route()]
        public async Task<IHttpActionResult> Post(CampModel campModel)
        {
            try
            {   
                if(await campRepository.GetCampAsync(campModel.Moniker) != null)
                {
                    //if the surrogate key already exists, then flag an error
                    ModelState.AddModelError("Moniker", "Moniker in Use");
                }

                if (ModelState.IsValid)
                {
                    var camp = mapper.Map<Camp>(campModel);

                    campRepository.AddCamp(camp);

                    if(await campRepository.SaveChangesAsync())
                    {
                        var newModel = mapper.Map<CampModel>(camp);
                        return CreatedAtRoute("GetCamp", new { moniker = newModel.Moniker }, newModel);
                    }
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel campModel)
        {
            try
            {
                var camp = await campRepository.GetCampAsync(moniker);
                if (camp == null)
                {
                    return NotFound();
                }

                mapper.Map(campModel,camp);

                if(await campRepository.SaveChangesAsync())
                {
                    return Ok(mapper.Map<CampModel>(camp));
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




        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await campRepository.GetCampAsync(moniker);
                if (camp == null)
                {
                    return NotFound();
                }

                campRepository.DeleteCamp(camp);

                if (await campRepository.SaveChangesAsync())
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
