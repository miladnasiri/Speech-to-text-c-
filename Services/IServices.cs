using milad_speechforcsharp.Models;

namespace milad_speechforcsharp.Services
{
    public interface ISpeechRecognitionService
    {
        Task<string> RecognizeSpeechAsync(Stream audioStream);
    }

    public interface ISentimentAnalysisService
    {
        Task<SentimentScore> AnalyzeSentimentAsync(string text);
    }

    public interface ITranscriptService
    {
        Task<List<Transcript>> GetTranscriptsAsync();
        Task SaveTranscriptAsync(string text, SentimentScore sentiment);
        Task<Stream> GetTranscriptFileAsync(string filename);
    }
}
