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
                    _logger.LogError(ex, "Error loading transcript: {File}", file);
                }
            }

            return transcripts.OrderByDescending(t => t.Timestamp).ToList();
        }

        public async Task SaveTranscriptAsync(string text, SentimentScore sentiment)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filename = $"transcript_{timestamp}";

            await File.WriteAllTextAsync(
                Path.Combine(_transcriptDirectory, $"{filename}.txt"),
                text);

            await File.WriteAllTextAsync(
                Path.Combine(_transcriptDirectory, $"{filename}.json"),
                JsonSerializer.Serialize(sentiment));
        }

        public Task<Stream> GetTranscriptFileAsync(string filename)
        {
            var path = Path.Combine(_transcriptDirectory, filename);
            return Task.FromResult<Stream>(File.OpenRead(path));
        }
    }
}
