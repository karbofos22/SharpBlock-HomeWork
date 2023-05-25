using Microsoft.AspNetCore.Mvc;
using SharpBlock_HomeWork;
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
                TextProcessor textProcessor = new();

                var textToProcess = textProcessor.PrepareTextViaApi(text);

                var result = UniqueWordsCounterLibrary.UniqueWordsCounter.ParallelProcessFile(textToProcess);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while processing text: {ex.Message}");
            }
        }
    }
}
