using Microsoft.AspNetCore.Mvc;
using studbud.Server.Models;

namespace studbud.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardsController : ControllerBase
    {
        private readonly AiApi _aiApi;

        public FlashcardsController(AiApi aiApi)
        {
            _aiApi = aiApi;
        }

        // POST /api/flashcards - for typed notes
        [HttpPost]
        public async Task<ActionResult<List<Flashcard>>> Generate([FromBody] FlashcardRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Notes))
                return BadRequest("Notes cannot be empty.");

            var cards = await _aiApi.GenerateFlashcardsAsync(request.Notes);
            return Ok(cards);
        }

        // POST /api/flashcards/upload - for uploaded files
        [HttpPost("upload")]
        public async Task<ActionResult<List<Flashcard>>> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();

                var cards = await _aiApi.GenerateFlashcardsAsync(content);

                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }

    public class FlashcardRequest
    {
        public string Notes { get; set; } = "";
    }
}
