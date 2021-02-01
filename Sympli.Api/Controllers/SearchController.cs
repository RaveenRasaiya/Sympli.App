using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sympli.Core.Models;
using Sympli.Search.Interfaces;
using System.Threading.Tasks;

namespace Sympli.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {        
        private readonly ISearchRequstValidator _searchRequstValidator;
        private readonly IParallelBotService _parallelBotService;        

        public SearchController(ISearchRequstValidator searchRequstValidator,IParallelBotService parallelBotService)
        {          
            _searchRequstValidator = searchRequstValidator;
            _parallelBotService = parallelBotService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(SearchRequestModel searchRequestModel)
        {
            string error = _searchRequstValidator.IsValid(searchRequestModel);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var response = await _parallelBotService.Process(searchRequestModel);

            if (response.Result == null)
            {
                return BadRequest("There is no results");
            }
            return new JsonResult(response);
        }
    }
}
