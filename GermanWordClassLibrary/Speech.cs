using GermanWordsClassLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;

namespace GermanWordsClassLibrary
{
    public static class Speech
    {
        public static SpeechRecognizer recognizer;
        public static SpeechSynthesizer synthesizer;
        public static SpeechRecognizerUI recognizerUI;

        private static bool initialized = false;

        public static void Initialize()
        {
            try
            {
                if (Speech.initialized)
                {
                    return;
                }
                Speech.recognizer = new SpeechRecognizer();
                Speech.synthesizer = new SpeechSynthesizer();
                Speech.recognizerUI = new SpeechRecognizerUI();

                IEnumerable<VoiceInformation> DeVoices = from voice in InstalledVoices.All
                                                         where voice.Gender == VoiceGender.Female
                                                         && voice.Language == "de-DE"
                                                         select voice;
                Speech.synthesizer.SetVoice(DeVoices.ElementAt(0));
                Speech.initialized = true;
                IsolatedStorageSettingsHelper.SetSpeechPackageState(true);
            }
            catch (Exception ex)
            {
                IsolatedStorageSettingsHelper.SetSpeechPackageState(false);
                throw new Exception();
            }
            

         }
    }
}
