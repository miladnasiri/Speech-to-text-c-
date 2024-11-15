namespace milad_speechforcsharp.Models
{
    public class Transcript
    {
        public Transcript()
        {
            Id = string.Empty;
            Text = string.Empty;
            Filename = string.Empty;
            Sentiment = new SentimentScore();
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public SentimentScore Sentiment { get; set; }
        public string Filename { get; set; }
    }

    public class SentimentScore
    {
        public double Positive { get; set; }
        public double Negative { get; set; }
        public double Neutral { get; set; }
    }
}
