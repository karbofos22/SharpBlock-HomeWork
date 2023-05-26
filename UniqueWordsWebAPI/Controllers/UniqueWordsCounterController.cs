using Microsoft.AspNetCore.Mvc;
using StopWord;
using System.ComponentModel.DataAnnotations;
using UniqueWordsCounterLibrary;

namespace UniqueWordsWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UniqueWordsCounterController : ControllerBase
    {
        [HttpPost("Counter")]
        public IActionResult Post([FromForm][Required] string text)
        {
            try
            {
                var stopWords = StopWords.GetStopWords("ru");

                var result = UniqueWordsCounterLibrary.Counter.ParallelProcessFile(text, stopWords);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while processing text: {ex.Message}");
            }
        }
    }
}
