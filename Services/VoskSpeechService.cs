using Vosk;
using NAudio.Wave;
using System.Text.Json;

namespace milad_speechforcsharp.Services
{
    public class VoskSpeechService : ISpeechRecognitionService, IDisposable
    {
        private readonly Model _model;
        private readonly VoskRecognizer _recognizer;
        private readonly ILogger<VoskSpeechService> _logger;

        public VoskSpeechService(IWebHostEnvironment env, ILogger<VoskSpeechService> logger)
        {
            _logger = logger;
            var modelPath = Path.Combine(env.WebRootPath, "models", "vosk-model");
            _model = new Model(modelPath);
            _recognizer = new VoskRecognizer(_model, 16000.0f);
            _logger.LogInformation("VoskSpeechService initialized");
        }

        public async Task<string> RecognizeSpeechAsync(Stream audioStream)
        {
            try
            {
                _logger.LogInformation("Starting speech recognition");
                
                // Read the entire stream into memory
                using var memoryStream = new MemoryStream();
                await audioStream.CopyToAsync(memoryStream);
                memoryStream.Position = 44; // Skip WAV header
                
                var buffer = new byte[8192];
                int bytesRead;
                
                while ((bytesRead = memoryStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (_recognizer.AcceptWaveform(buffer, bytesRead))
                    {
                        var result = _recognizer.PartialResult();
                        _logger.LogInformation($"Partial recognition: {result}");
                    }
                }

                var finalResult = _recognizer.FinalResult();
                _logger.LogInformation($"Final result: {finalResult}");
                
                var text = JsonDocument.Parse(finalResult)
                    .RootElement.GetProperty("text").GetString();
                
                _logger.LogInformation($"Recognized text: {text}");
                return text ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Speech recognition error");
                throw;
            }
        }

        public void Dispose()
        {
            _recognizer?.Dispose();
            _model?.Dispose();
        }
    }
}
