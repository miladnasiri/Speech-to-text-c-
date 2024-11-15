using System.Text.Json;
using milad_speechforcsharp.Models;

namespace milad_speechforcsharp.Services
{
    public class TranscriptService : ITranscriptService
    {
        private readonly string _transcriptDirectory;
        private readonly ILogger<TranscriptService> _logger;

        public TranscriptService(IWebHostEnvironment env, ILogger<TranscriptService> logger)
        {
            _logger = logger;
            _transcriptDirectory = Path.Combine(env.WebRootPath, "transcripts");
            Directory.CreateDirectory(_transcriptDirectory);
            _logger.LogInformation($"Transcript directory: {_transcriptDirectory}");
        }

        public async Task<List<Transcript>> GetTranscriptsAsync()
        {
            var transcripts = new List<Transcript>();
            var files = Directory.GetFiles(_transcriptDirectory, "*.txt");

            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    var text = await File.ReadAllTextAsync(file);
                    var sentimentFile = Path.ChangeExtension(file, ".json");
                    var sentiment = new SentimentScore();

                    if (File.Exists(sentimentFile))
                    {
                        var sentimentJson = await File.ReadAllTextAsync(sentimentFile);
                        sentiment = JsonSerializer.Deserialize<SentimentScore>(sentimentJson) ?? new SentimentScore();
                    }

                    transcripts.Add(new Transcript
                    {
                        Id = Path.GetFileNameWithoutExtension(file),
                        Text = text,
                        Timestamp = fileInfo.CreationTime,
                        Sentiment = sentiment,
                        Filename = Path.GetFileName(file)
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error loading transcript: {file}");
                }
            }

            return transcripts.OrderByDescending(t => t.Timestamp).ToList();
        }

        public async Task SaveTranscriptAsync(string text, SentimentScore sentiment)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"transcript_{timestamp}.txt";
                var filePath = Path.Combine(_transcriptDirectory, fileName);

                // Save transcript text
                await File.WriteAllTextAsync(filePath, text);
                _logger.LogInformation($"Saved transcript: {filePath}");

                // Save sentiment data
                var sentimentPath = Path.ChangeExtension(filePath, ".json");
                await File.WriteAllTextAsync(
                    sentimentPath,
                    JsonSerializer.Serialize(sentiment, new JsonSerializerOptions { WriteIndented = true }));
                _logger.LogInformation($"Saved sentiment: {sentimentPath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving transcript");
                throw;
            }
        }

        public async Task<Stream> GetTranscriptFileAsync(string filename)
        {
            var path = Path.Combine(_transcriptDirectory, filename);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Transcript file not found", filename);
            }
            return File.OpenRead(path);
        }
    }
}
