using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using milad_speechforcsharp.Models;
using milad_speechforcsharp.Services;

namespace milad_speechforcsharp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITranscriptService _transcriptService;
        private readonly ILogger<IndexModel> _logger;

        public List<Transcript> Transcripts { get; private set; } = new List<Transcript>();

        public IndexModel(ITranscriptService transcriptService, ILogger<IndexModel> logger)
        {
            _transcriptService = transcriptService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Transcripts = await _transcriptService.GetTranscriptsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading transcripts");
                Transcripts = new List<Transcript>();
            }
        }
    }
}
