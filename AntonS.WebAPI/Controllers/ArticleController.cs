using Microsoft.AspNetCore.Mvc;
using AntonS;
using AntonS.Abstractions.Services;
using AutoMapper;
using AntonS.WebAPI.Controllers.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AntonS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var article = _articleService.GetArticleByIdWithSourceNameAsync(id);
                if (article == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ArticleResponse>(article));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ErrorResponse() { Message = ex.Message});
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _articleService.RemoveById(id);
            return Ok();
        }
    }
}
