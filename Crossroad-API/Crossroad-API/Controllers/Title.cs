using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crossroad_API.Database;
using Crossroad_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crossroad_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class TitleController : ControllerBase
    {
        private readonly CrossroadDBContext crossroadDbContext;

        public TitleController(CrossroadDBContext crossroadDbContext)
        {
            this.crossroadDbContext = crossroadDbContext;
        }

        [HttpGet("GetTitleById")]
        public async Task<ActionResult<IQueryable<Title>>> GetTitle(int titleId=0)
        {
            try
            {
                IQueryable<Title> searchQuery = null;

                if (titleId>0)
                    searchQuery = crossroadDbContext.Titles.Where(t=>t.TitleId==titleId).AsNoTracking();
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "A title Id was not provided with the request.");

                var recordsFound = await searchQuery.ToListAsync();

                if (recordsFound.Count() == 0)
                   return StatusCode(StatusCodes.Status400BadRequest, "No titles match Id supplied.");

                return Ok(recordsFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message.ToString());
            }
        }

        [HttpGet("SearchByTitle")]        
        public async Task<ActionResult<IQueryable<Title>>> SearchTitle(string titleName)
        {
            try
            {
                IQueryable<Title> searchQuery = null;

                if (!string.IsNullOrEmpty(titleName))
                    searchQuery = crossroadDbContext.Titles
                                 .AsQueryable().Where
                                 (e => e.TitleName.Contains(titleName)
                                 || e.TitleNameSortable.Contains(titleName)).AsNoTracking();
                else
                    return StatusCode(StatusCodes.Status400BadRequest,"Please enter a title to search.");

                var recordsFound = await searchQuery.ToListAsync();

                if (recordsFound.Count() == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "No titles match your search keyword.");

                return Ok(recordsFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message.ToString());
            }
        }

        [HttpGet("GetTitleDetails")]
        public async Task<ActionResult<IQueryable<Title>>> GetDetails(int titleId = 0)
        {
            try
            {                
                IQueryable<Title> searchQuery=null;

                if (titleId > 0)
                    searchQuery = crossroadDbContext.Titles
                            .Include(a => a.Awards)
                            .Include(o => o.OtherNames)
                            .Include(p => p.TitleParticipants)
                            .Include(g => g.TitleGenres).AsQueryable().Where
                                (e => e.TitleId == titleId)
                                .AsNoTracking();
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Please enter a title to search.");
               
                var recordsFound= await searchQuery.ToListAsync();

                if (recordsFound.Count() == 0)
                   return StatusCode(StatusCodes.Status400BadRequest, "No titles match your search keyword.");

                return Ok(recordsFound);                       
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message.ToString());
            }
        }
    }
}
