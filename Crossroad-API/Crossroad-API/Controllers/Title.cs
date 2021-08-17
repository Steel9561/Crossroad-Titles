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

        static IQueryable<Object> searchQueryTitles = null;
        static IQueryable<Object> searchQueryAwards = null;
        static IQueryable<Object> searchQueryOtherNames = null;
        static IQueryable<Object> searchQueryTitleParticipants = null;
        static IQueryable<Object> searchQueryGenres = null;
        static IQueryable<Object> searchQueryStoryLines = null;

        public TitleController(CrossroadDBContext crossroadDbContext)
        {
            //We DI the Dbcontext to be used to connect to Warnermedia's title database            
            this.crossroadDbContext = crossroadDbContext;
        }

        [HttpGet("GetTitleDetails")]
        public async Task<ActionResult<IQueryable<Title>>> GetDetails(int titleId = 0)
        {
            try
            {
                if (titleId > 0)
                {
                    GenerateEFQueries(titleId);

                    //The following will take of actually executing the queries
                    //to pull up the details about the specific movie title.
                    var recordsFoundTitles = await searchQueryTitles.ToListAsync();
                    var recordsFoundAwards = await searchQueryAwards.ToListAsync();
                    var recordsOtherNames = await searchQueryOtherNames.ToListAsync();
                    var recordsOtherTitleParticipants = await searchQueryTitleParticipants.ToListAsync();
                    var recordsOtherTitleGenres = await searchQueryGenres.ToListAsync();
                    var recordsOtherStoryLines = await searchQueryStoryLines.ToListAsync();
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Please enter a title to search.");

                if (searchQueryTitles.Count() == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "No titles match your search keyword.");

                return Ok(searchQueryTitles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message.ToString());
            }
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
     

        //The following are the queries that are in charge
        //of pulling all the details about a specific movie title. 
        //Note that these queries are in defered execution mode
        //and won't execute untli the ToListAsync method is called on them.
        private void GenerateEFQueries(int titleId)
        {

            //Get Titles
            searchQueryTitles = (from t in crossroadDbContext.Titles.Where(t => t.TitleId == titleId)
                                 select new { Titles = t }).Distinct();

            //Get Title Awards
            searchQueryAwards = (from a in crossroadDbContext.Awards.Where(a => a.TitleId == titleId)
                                 select new { Awards = a }).Distinct();

            //Get Title Other Names
            searchQueryOtherNames = (from o in crossroadDbContext.OtherNames.Where(o => o.TitleId == titleId)
                                     select new { OtherNames = o }).Distinct();

            //Get Title Story Lines
            searchQueryStoryLines = (from o in crossroadDbContext.StoryLines.Where(o => o.TitleId == titleId)
                                     select new { StoryLines = o });

            //Get Title Participants
            searchQueryTitleParticipants = (from t in crossroadDbContext.Titles
                                                .Where(t => t.TitleId == titleId)
                                            join tp in crossroadDbContext.TitleParticipants
                                            .Where(tp => tp.TitleId == titleId)
                                            on t.TitleId equals tp.TitleId
                                            join tpa in crossroadDbContext.Participants
                                            on tp.ParticipantId equals tpa.Id
                                            select new { Titles = t, TitleParticipants = tp, Participants = tpa }).Distinct();

            //Get Title Genres
            searchQueryGenres = (from t in crossroadDbContext.Titles
                                           .Where(t => t.TitleId == titleId)
                                 join tg in crossroadDbContext.TitleGenres
                                 .Where(tg => tg.TitleId == titleId)
                                 on t.TitleId equals tg.TitleId
                                 join g in crossroadDbContext.Genres
                                 on tg.GenreId equals g.Id
                                 select new { Titles = t, TitleGenres = tg, Genres = g }).Distinct();

        }       
    }
}
