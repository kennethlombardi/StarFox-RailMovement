using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Events;
using System.Threading.Tasks;

#if UNITY_STANDALONE_WIN
using Microsoft.CognitiveServices.Speech;
#endif

[Serializable]
public class TTS
{
    public AudioContent audioContent;
}

[Serializable]
public class AudioContent
{
    public byte[] data;
}

[Serializable]
public class NluResponse
{
    public TTS tts;
    public string response;
}

public class SpeechToText : MonoBehaviour
{
    // Hook up the two properties below with a Text and Button object in your UI.
    public AudioSource aud;
    private string finalResultMessage;
    private string partialResultMessage;
    public Text resultText;
    public float time = 0.01f;
    public int timer = 0;
    private int i = 0;
    public string message;
    public Text responseText;
    public Text nluText;
    private string nluMessage = "";

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message2;
    private bool micPermissionGranted = false;
    public int samplerate = 44100;
    string nluEndpoint = "https://vre-api-2.azurewebsites.net/queryBot";
    public GameEvent onFinalResult;

#if UNITY_STANDALONE_WIN
    private SpeechRecognizer recognizer;
    private SpeechConfig speechConfig;

    public async Task RecognizeSpeechAsync()
    {
        // Creates an instance of a speech config with specified subscription key and service region.
        // Replace with your own subscription key // and service region (e.g., "westus").
        // var config = SpeechConfig.FromSubscription("4725470f35034e9dab0cbcc072241da1", "westus2");

        // Creates a speech recognizer.
        recognizer = new SpeechRecognizer(speechConfig);
        // using (var recognizer = new SpeechRecognizer(speechConfig))
        // {
        recognizer.Recognizing += (s, e) =>
        {
            Debug.Log($"RECOGNIZING: Text={e.Result.Text}");
            partialResultMessage = e.Result.Text;
        };

        recognizer.Recognized += (s, e) =>
        {
            var result = e.Result;
            Console.WriteLine($"Reason: {result.Reason.ToString()}");
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Debug.Log($"Final result: Text: {result.Text}.");
                finalResultMessage = result.Text;
            }
        };

        recognizer.Canceled += (s, e) =>
        {
            Debug.Log($"\n    Recognition Canceled. Reason: {e.Reason.ToString()}, CanceledReason: {e.Reason}");
        };

        recognizer.SessionStarted += (s, e) =>
        {
            Debug.Log("\n    Session started event.");
        };

        recognizer.SessionStopped += (s, e) =>
        {
            Debug.Log("\n    Session stopped event.");
        };

        // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
    }

#endif

    void Start()
    {
        message2 = responseText.text;
        responseText.text = "";

        if (resultText == null)
        {
            UnityEngine.Debug.LogError("result text property is null! Assign a UI Text element to it.");
        }
#if UNITY_STANDALONE_WIN
        speechConfig = SpeechConfig.FromSubscription("4725470f35034e9dab0cbcc072241da1", "westus2");
        RecognizeSpeechAsync();
#endif
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
#if UNITY_STANDALONE_WIN
            recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            recognizer.Dispose();
            recognizer = null;
#endif
        }

        lock (threadLocker)
        {
            if (resultText != null)
            {
                if (partialResultMessage != "")
                {
                    resultText.text = partialResultMessage;
                }
                if (finalResultMessage != null && finalResultMessage != "" && finalResultMessage.Length >= 5)
                {
                    StartCoroutine(GetAndSayNluResponse(finalResultMessage));
                    resultText.text = finalResultMessage;
                    finalResultMessage = "";
                    partialResultMessage = "";
                    onFinalResult.Raise();
                }
                if (nluMessage != null && nluMessage != "")
                {
                    nluText.text = nluMessage;
                    nluMessage = "";
                }
            }
        }
    }

    IEnumerator GetAndSayNluResponse(string text)
    {
        WWWForm form = new WWWForm();
        form.AddField("query", text);
        using (UnityWebRequest www = UnityWebRequest.Post(this.nluEndpoint, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                NluResponse nluResponse = JsonUtility.FromJson<NluResponse>(www.downloadHandler.text);
                nluMessage = nluResponse.response;
                Debug.Log("NLU response: " + nluMessage);
                PlayTTSAudio(nluResponse);
            }
        }
    }

    void PlayTTSAudio(NluResponse nluResponse)
    {
        bool doStream = true;
        int channels = 1;
        float[] floatArray = ConvertByteToFloat16(nluResponse.tts.audioContent.data);
        string clipName = "HttpResponseClip";

        AudioClip myClip = AudioClip.Create(
            clipName,
            floatArray.Length,
            channels,
            samplerate,
            !doStream);

        myClip.SetData(floatArray, 0);

        // AudioSource aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.Play();
    }

    private float[] ConvertByteToFloat16(byte[] array)
    {
        float[] floatArr = new float[array.Length / 2];
        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = (float)(BitConverter.ToInt16(array, i * 2) / 32767f);
        }
        return floatArr;
    }
}