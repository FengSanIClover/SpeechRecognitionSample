using Interfaces;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace Services
{
    // 語音服務的語言，語音名稱設定等可參考
    // https://github.com/MicrosoftDocs/azure-docs.zh-tw/blob/master/articles/cognitive-services/Speech-Service/language-support.md#text-to-speech 

    public class SpeechService : ISpeechService
    {
        /// <summary>
        /// 辨識輸入語音為文字
        /// </summary>
        /// <param name="subkey">訂用帳戶金鑰</param>
        /// <param name="region">訂用帳戶服務的所在區域</param>
        /// <param name="language">要辨識的語言</param>
        /// <returns>辨識後的文字</returns>
        public async Task<string> SpeakToText(string subkey, string region, string language)
        {
            // 設定 訂用帳戶金鑰 與 訂用帳戶服務的所在區域
            // 免費試用的服務所在區域都是 westus
            var config =
                SpeechConfig.FromSubscription(subkey, region);

            // 要辨識的語言
            config.SpeechRecognitionLanguage = language;

            // 建立語音辨識器
            using (var recognizer = new SpeechRecognizer(config))
            {
                // RecognizeOnceAsync 此方法 僅適用於單個語音識別，如命令或查詢
                // 如果需要長時間運行的多話語識別，請改用 StartContinuousRecognitionAsync（）
                var result = await recognizer.RecognizeOnceAsync();

                return result.Text;
            }
        }

        /// <summary>
        /// 辨識輸入文字為語音
        /// </summary>
        /// <param name="subkey">訂用帳戶金鑰</param>
        /// <param name="region">訂用帳戶服務的所在區域</param>
        /// <param name="language">要辨識的語言</param>
        /// <param name="text">輸入的文字</param>
        /// <returns></returns>
        public async Task TextToSpeak(string subkey, string region, string language, string text)
        {
            // 設定 訂用帳戶金鑰 與 訂用帳戶服務的所在區域
            // 免費試用的服務所在區域都是 westus
            var config = 
                SpeechConfig.FromSubscription(subkey, region);

            // 要辨識的語言
            config.SpeechRecognitionLanguage = language;

            // 根據要辨識的語言設定對應的語音名稱
            // 中文 zh-TW-Yating-Apollo、zh-TW-HanHanRUS、zh-TW-Zhiwei-Apollo
            // 英文 en-AU-Catherine、en-US-Jessa24kRUS
            if (language == "zh-TW")
                config.SpeechSynthesisVoiceName = "zh-TW-Yating-Apollo";

            if (language == "en-AU")
                config.SpeechSynthesisVoiceName = "en-US-Jessa24kRUS";

            // 建立語音辨識器
            using (var synthesizer = new SpeechSynthesizer(config))
            {
                // 將文字合成為語音
                var result = await synthesizer.SpeakTextAsync(text);

                // 釋放資源
                result.Dispose();
            }
        }
    }
}
