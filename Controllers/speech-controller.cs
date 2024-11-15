using Microsoft.AspNetCore.Mvc;
using milad_speechforcsharp.Services;

namespace milad_speechforcsharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeechController : ControllerBase
    {
        private readonly ISpeechRecognitionService _speechService;
        private readonly ISentimentAnalysisService _sentimentService;
        private readonly ITranscriptService _transcriptService;
        private readonly ILogger<SpeechController> _logger;

        public SpeechController(
            ISpeechRecognitionService speechService,
            ISentimentAnalysisService sentimentService,
            ITranscriptService transcriptService,
            ILogger<SpeechController> logger)
        {
            _speechService = speechService;
            _sentimentService = sentimentService;
            _transcriptService = transcriptService;
            _logger = logger;
        }

        [HttpPost("recognize")]
        public async Task<IActionResult> RecognizeSpeech()
        {
            try
            {
                _logger.LogInformation("Starting speech recognition");

                // Read the raw audio data
                using var memoryStream = new MemoryStream();
                await Request.Body.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                _logger.LogInformation($"Received audio data size: {memoryStream.Length} bytes");

                var text = await _speechService.RecognizeSpeechAsync(memoryStream);
                
                if (string.IsNullOrEmpty(text))
                {
                    _logger.LogWarning("No speech detected");
                    return Ok(new { success = true, text = "No speech detected" });
                }

                _logger.LogInformation($"Recognized text: {text}");
                var sentiment = await _sentimentService.AnalyzeSentimentAsync(text);
                await _transcriptService.SaveTranscriptAsync(text, sentiment);

                return Ok(new { success = true, text, sentiment });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing speech");
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
