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
                using var reader = new BinaryReader(audioStream);
                
                // Skip WAV header (44 bytes)
                reader.BaseStream.Position = 44;
                
                byte[] buffer = new byte[4096];
                int bytesRead;
                
                while ((bytesRead = await audioStream.ReadAsync(buffer)) > 0)
                {
                    if (_recognizer.AcceptWaveform(buffer, bytesRead))
                    {
                        var partialResult = _recognizer.PartialResult();
                        _logger.LogDebug($"Partial result: {partialResult}");
                    }
                }

                var finalResult = _recognizer.FinalResult();
                _logger.LogInformation($"Final result: {finalResult}");

                var doc = JsonDocument.Parse(finalResult);
                var text = doc.RootElement.GetProperty("text").GetString();
                
                return text ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing audio");
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
