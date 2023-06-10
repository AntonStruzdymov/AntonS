using Microsoft.AspNetCore.Mvc;
using AntonS;
using AntonS.Abstractions.Services;
using AutoMapper;
using AntonS.Core.DTOs;
using AntonS.Models;
using AntonS.Business;
using AntonS.WebAPI.Requests;
using AntonS.WebAPI.Responses;


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
        public async Task<IActionResult> GetAll([FromBody] GetArticlesByPagesRequest request)
        {
            try
            {
                var articles = await _articleService.GetArticlesByPageAsync(request.PageNumber,request.PageSize);             
                return Ok(articles.Select(a => _mapper.Map<ArticleResponse>(a)));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var article = await _articleService.GetArticleByIdWithSourceNameAsync(id);
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
        public async Task<IActionResult> Post([FromBody] CreateOrUpdateArticleRequest request)
        {
            var article = _mapper.Map<ArticleDTO>(request);
            await _articleService.AddAsync(article);
            var addedArticle = (await _articleService.GetArticlesByNameAsync(article.Title)).Take(1);
            var articleResponse = _mapper.Map<ArticleResponse>(addedArticle);
            return Created($"Article/{articleResponse.Id}", articleResponse);
            
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateOrUpdateArticleRequest request)
        {
            var articleDto = _articleService.GetArticleByIdWithSourceNameAsync(id);
            if (articleDto == null)
            {
                await _articleService.AddAsync(_mapper.Map<ArticleDTO>(request));
            }
            else
            {
                var valuesToPatch = new List<PatchDTO>();
                var newValues = request.GetType().GetProperties();
                foreach (var property in newValues)
                {
                    if(property.GetValue(request) != null)
                    {
                        var patchDto = new PatchDTO()
                        {
                            Name = property.Name,
                            Value = property.GetValue(request)
                        };
                        valuesToPatch.Add(patchDto);
                    }
                }
                await _articleService.UpdateArticlesAsync(id, valuesToPatch);
            }
            return Ok();
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
