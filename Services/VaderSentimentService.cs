using VaderSharp2;
using milad_speechforcsharp.Models;

namespace milad_speechforcsharp.Services
{
    public class VaderSentimentService : ISentimentAnalysisService
    {
        private readonly SentimentIntensityAnalyzer _analyzer;

        public VaderSentimentService()
        {
            _analyzer = new SentimentIntensityAnalyzer();
        }

        public Task<SentimentScore> AnalyzeSentimentAsync(string text)
        {
            var scores = _analyzer.PolarityScores(text);
            
            return Task.FromResult(new SentimentScore
            {
                Positive = scores.Positive,
                Negative = scores.Negative,
                Neutral = scores.Neutral
            });
        }
    }
}
