let audioContext = null;
let mediaRecorder = null;
let audioChunks = [];

const recordButton = document.getElementById('recordButton');
const status = document.getElementById('status');
const latestTranscript = document.getElementById('latestTranscript');
const sentimentDisplay = document.getElementById('sentimentDisplay');

async function startRecording() {
    try {
        status.textContent = 'Requesting microphone access...';
        const stream = await navigator.mediaDevices.getUserMedia({ 
            audio: {
                channelCount: 1,
                sampleRate: 16000,
                echoCancellation: false,
                noiseSuppression: false,
                autoGainControl: false
            } 
        });
        
        audioContext = new (window.AudioContext || window.webkitAudioContext)({sampleRate: 16000});
        const source = audioContext.createMediaStreamSource(stream);
        const processor = audioContext.createScriptProcessor(8192, 1, 1);
        
        audioChunks = [];
        
        processor.onaudioprocess = (e) => {
            const inputData = e.inputBuffer.getChannelData(0);
            const pcmData = new Int16Array(inputData.length);
            for (let i = 0; i < inputData.length; i++) {
                pcmData[i] = Math.max(-1, Math.min(1, inputData[i])) * 0x7FFF;
            }
            audioChunks.push(pcmData);
        };

        source.connect(processor);
        processor.connect(audioContext.destination);
        
        mediaRecorder = { stream, source, processor };
        recordButton.classList.add('bg-red-500');
        status.textContent = 'Recording...';
        console.log('Recording started');
    } catch (error) {
        console.error('Error:', error);
        status.textContent = error.message;
    }
}

function createWavFile(audioData) {
    const headerLength = 44;
    const dataLength = audioData.length * 2;
    const totalLength = headerLength + dataLength;
    
    const buffer = new ArrayBuffer(totalLength);
    const view = new DataView(buffer);
    
    const writeString = (offset, string) => {
        for (let i = 0; i < string.length; i++) {
            view.setUint8(offset + i, string.charCodeAt(i));
        }
    };
    
    // WAV Header
    writeString(0, 'RIFF');
    view.setUint32(4, totalLength - 8, true);
    writeString(8, 'WAVE');
    writeString(12, 'fmt ');
    view.setUint32(16, 16, true);
    view.setUint16(20, 1, true);
    view.setUint16(22, 1, true);
    view.setUint32(24, 16000, true);
    view.setUint32(28, 16000 * 2, true);
    view.setUint16(32, 2, true);
    view.setUint16(34, 16, true);
    writeString(36, 'data');
    view.setUint32(40, dataLength, true);
    
    // Audio Data
    let offset = 44;
    for (let i = 0; i < audioData.length; i++) {
        view.setInt16(offset, audioData[i], true);
        offset += 2;
    }
    
    return new Blob([buffer], { type: 'audio/wav' });
}

async function stopRecording() {
    if (!mediaRecorder) return;
    
    console.log('Stopping recording...');
    status.textContent = 'Processing...';
    
    const { stream, source, processor } = mediaRecorder;
    source.disconnect();
    processor.disconnect();
    stream.getTracks().forEach(track => track.stop());
    
    const totalLength = audioChunks.reduce((acc, chunk) => acc + chunk.length, 0);
    const audioData = new Int16Array(totalLength);
    let offset = 0;
    for (const chunk of audioChunks) {
        audioData.set(chunk, offset);
        offset += chunk.length;
    }
    
    const wavBlob = createWavFile(audioData);
    console.log('WAV created, size:', wavBlob.size);
    
    await sendAudioToServer(wavBlob);
    
    if (audioContext) {
        await audioContext.close();
        audioContext = null;
    }
    mediaRecorder = null;
    audioChunks = [];
}

async function sendAudioToServer(audioBlob) {
    try {
        const formData = new FormData();
        formData.append('audio', audioBlob, 'speech.wav');
        
        const response = await fetch('/api/speech/recognize', {
            method: 'POST',
            body: formData
        });
        
        const data = await response.json();
        console.log('Server response:', data);
        
        if (data.success) {
            latestTranscript.textContent = data.text || 'No speech detected';
            if (data.sentiment) {
                sentimentDisplay.innerHTML = `
                    <span class="text-green-400">Positive: ${data.sentiment.positive.toFixed(2)}</span>
                    <span class="text-red-400 ml-4">Negative: ${data.sentiment.negative.toFixed(2)}</span>
                    <span class="text-yellow-400 ml-4">Neutral: ${data.sentiment.neutral.toFixed(2)}</span>
                `;
            }
        } else {
            status.textContent = data.error || 'Error processing audio';
        }
    } catch (error) {
        console.error('Server error:', error);
        status.textContent = error.message;
    } finally {
        recordButton.classList.remove('bg-red-500');
        setTimeout(() => status.textContent = 'Tap to speak', 2000);
    }
}

let isRecording = false;
recordButton.addEventListener('click', async () => {
    if (!isRecording) {
        isRecording = true;
        await startRecording();
    } else {
        isRecording = false;
        await stopRecording();
    }
});
