# Speech-to-Text C# Dashboard

![Dashboard](https://github.com/miladnasiri/Speech-to-text-c-/blob/800c9a071a35e6d2814adae4e3f17624732d38d7/DAshboard.png)

A powerful real-time speech recognition and sentiment analysis web application built with ASP.NET Core and Vosk. This application provides accurate speech-to-text conversion with sentiment analysis capabilities in a user-friendly dashboard interface.

## Key Features

- Real-time speech-to-text conversion
- Sentiment analysis using VADER algorithm
- Comprehensive transcript history with downloadable files
- User-friendly dashboard interface
- Offline speech recognition capabilities
- Simple and intuitive user experience

## Technology Stack

- ASP.NET Core 8.0
- Vosk Speech Recognition Engine
- VaderSharp2 for sentiment analysis
- Web Audio API
- TailwindCSS

## Prerequisites

- .NET SDK 8.0
- Linux/Ubuntu system
- Modern web browser
- FFmpeg installed

## Installation

1. Clone the repository:
```bash
git clone https://github.com/miladnasiri/Speech-to-text-c-.git
cd Speech-to-text-c-
```

2. Install system dependencies:
```bash
sudo apt-get update
sudo apt-get install -y ffmpeg
```

3. Install .NET packages:
```bash
/usr/bin/snap run dotnet-sdk.dotnet add package NAudio
/usr/bin/snap run dotnet-sdk.dotnet add package Vosk
/usr/bin/snap run dotnet-sdk.dotnet add package VaderSharp2
```

4. Download and set up Vosk model:
```bash
mkdir -p wwwroot/models
cd wwwroot/models
wget https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip
unzip vosk-model-small-en-us-0.15.zip
mv vosk-model-small-en-us-0.15 vosk-model
cd ../..
```

5. Create transcripts directory:
```bash
mkdir -p wwwroot/transcripts
```

## Running the Application

```bash
/usr/bin/snap run dotnet-sdk.dotnet build
/usr/bin/snap run dotnet-sdk.dotnet run --urls="http://localhost:5267"
```

## Usage Guide

1. Open http://localhost:5267 in your browser
2. Click the microphone button to start recording
3. Speak into your microphone
4. Click the button again to stop recording
5. View the transcription and sentiment analysis results
6. Download transcripts from the history section

## Project Structure

```
Speech-to-text-c-/
├── Controllers/
│   └── SpeechController.cs
├── Models/
│   └── Transcript.cs
├── Services/
│   ├── VoskSpeechService.cs
│   └── TranscriptService.cs
├── Pages/
│   └── Index.cshtml
├── wwwroot/
│   ├── js/
│   │   └── speech.js
│   ├── models/
│   │   └── vosk-model/
│   └── transcripts/
└── Program.cs
```

## Key Components

### Speech Recognition
The application uses Vosk for offline speech recognition, providing high accuracy without requiring an internet connection.

### Sentiment Analysis
Implements the VADER algorithm through VaderSharp2 to analyze text sentiment, providing detailed positive, negative, and neutral scores.

### Transcript Storage
Automatically saves all transcripts with timestamps and sentiment data for future reference.

## Troubleshooting

If you encounter any issues:

1. Make sure all prerequisites are installed
2. Check if the Vosk model is properly downloaded
3. Ensure microphone permissions are granted in browser
4. Verify FFmpeg is installed correctly

## Support

For issues and questions, please use GitHub issues.

## License

MIT License

## Author

Milad Nasiri
