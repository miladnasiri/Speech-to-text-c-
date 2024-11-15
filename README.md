# 🎙️ Speech-to-Text C# Dashboard

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![Vosk](https://img.shields.io/badge/Vosk-Speech_Recognition-blue.svg)](https://alphacephei.com/vosk/)
[![TailwindCSS](https://img.shields.io/badge/TailwindCSS-Frontend-06B6D4.svg)](https://tailwindcss.com/)

![Dashboard](https://github.com/miladnasiri/Speech-to-text-c-/blob/800c9a071a35e6d2814adae4e3f17624732d38d7/DAshboard.png)

> 🚀 A powerful real-time speech recognition and sentiment analysis web application built with ASP.NET Core and Vosk. This application provides accurate speech-to-text conversion with sentiment analysis capabilities in a user-friendly dashboard interface.

## ✨ Key Features

- 🎤 Real-time speech-to-text conversion
- 🎯 Sentiment analysis using VADER algorithm
- 📝 Comprehensive transcript history with downloadable files
- 🎨 User-friendly dashboard interface
- 🌐 Offline speech recognition capabilities
- 💡 Simple and intuitive user experience

## 🛠️ Technology Stack

- 🔷 ASP.NET Core 8.0
- 🔊 Vosk Speech Recognition Engine
- 📊 VaderSharp2 for sentiment analysis
- 🎵 Web Audio API
- 🎨 TailwindCSS

## 📋 Prerequisites

- ✅ .NET SDK 8.0
- 🐧 Linux/Ubuntu system
- 🌐 Modern web browser
- 🎬 FFmpeg installed

## 🚀 Installation

### 1️⃣ Clone the repository:
```bash
git clone https://github.com/miladnasiri/Speech-to-text-c-.git
cd Speech-to-text-c-
```

### 2️⃣ Install system dependencies:
```bash
sudo apt-get update
sudo apt-get install -y ffmpeg
```

### 3️⃣ Install .NET packages:
```bash
/usr/bin/snap run dotnet-sdk.dotnet add package NAudio
/usr/bin/snap run dotnet-sdk.dotnet add package Vosk
/usr/bin/snap run dotnet-sdk.dotnet add package VaderSharp2
```

### 4️⃣ Download and set up Vosk model:
```bash
mkdir -p wwwroot/models
cd wwwroot/models
wget https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip
unzip vosk-model-small-en-us-0.15.zip
mv vosk-model-small-en-us-0.15 vosk-model
cd ../..
```

### 5️⃣ Create transcripts directory:
```bash
mkdir -p wwwroot/transcripts
```

## 🎯 Running the Application

```bash
/usr/bin/snap run dotnet-sdk.dotnet build
/usr/bin/snap run dotnet-sdk.dotnet run --urls="http://localhost:5267"
```

## 📖 Usage Guide

1. 🌐 Open http://localhost:5267 in your browser
2. 🎤 Click the microphone button to start recording
3. 🗣️ Speak into your microphone
4. ⏹️ Click the button again to stop recording
5. 📊 View the transcription and sentiment analysis results
6. ⬇️ Download transcripts from the history section

## 📁 Project Structure

```
Speech-to-text-c-/
├── 📂 Controllers/
│   └── 📄 SpeechController.cs
├── 📂 Models/
│   └── 📄 Transcript.cs
├── 📂 Services/
│   ├── 📄 VoskSpeechService.cs
│   └── 📄 TranscriptService.cs
├── 📂 Pages/
│   └── 📄 Index.cshtml
├── 📂 wwwroot/
│   ├── 📂 js/
│   │   └── 📄 speech.js
│   ├── 📂 models/
│   │   └── 📂 vosk-model/
│   └── 📂 transcripts/
└── 📄 Program.cs
```

## 🔍 Key Components

### 🎤 Speech Recognition
The application uses Vosk for offline speech recognition, providing high accuracy without requiring an internet connection.

### 📊 Sentiment Analysis
Implements the VADER algorithm through VaderSharp2 to analyze text sentiment, providing detailed positive, negative, and neutral scores.

### 💾 Transcript Storage
Automatically saves all transcripts with timestamps and sentiment data for future reference.

## 🤝 Support
For issues and questions, please use GitHub issues.

## 📜 License
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

## 👨‍💻 Author
**Milad Nasiri**

[![GitHub followers](https://img.shields.io/github/followers/miladnasiri?style=social)](https://github.com/miladnasiri)

---
<div align="center">
⭐ Star this repository if you find it helpful!
</div>
