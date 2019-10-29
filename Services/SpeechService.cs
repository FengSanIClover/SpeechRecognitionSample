using Interfaces;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace Services
{
    // 語音服務的語言，語音名稱設定等可參考
    // https://github.com/MicrosoftDocs/azure-docs.zh-tw/blob/master/articles/cognitive-services/Speech-Service/language-support.md#text-to-speech 

    /// <summary>
    /// 語音辨識
    /// </summary>
    public class SpeechService : ISpeechService
    {
        private string recognizeText = string.Empty;

        /// <summary>
        /// 辨識輸入語音為文字
        /// </summary>
        /// <param name="recognizeBM">語音辨識基本設定用</param>
        /// <returns>辨識後的文字</returns>
        public async Task<string> SpeakToText(RecognizeBM recognizeBM)
        {
            // 設定語音服務設定
            var config = this.GetSpeechConfig(recognizeBM);

            // 建立語音辨識器
            using (var recognizer = new SpeechRecognizer(config))
            {
                Console.WriteLine("輸入語音:");

                // RecognizeOnceAsync 此方法 僅適用於單個語音識別，如命令或查詢
                // 如果需要長時間運行的多話語識別，請改用 StartContinuousRecognitionAsync（）
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);

                    this.DisPlayMessage(cancellation);
                }

                recognizeText = result.Text;

                if (result.Reason == ResultReason.NoMatch)
                {
                    recognizeText = $"無法辨識輸入語音，請重新輸入";
                }

                return recognizeText;
            }
        }

        /// <summary>
        /// 辨識輸入文字為語音
        /// </summary>
        /// <param name="textToSpeakBM">輸入文字為語音用</param>
        /// <returns></returns>
        public async Task TextToSpeak(RecognizeBM recognizeBM)
        {
            // 設定語音服務設定
            var config = this.GetSpeechConfig(recognizeBM);

            // 建立語音辨識器
            using (var synthesizer = new SpeechSynthesizer(config))
            {
                Console.WriteLine("處理中...");

                // 將文字合成為語音
                var result = await synthesizer.SpeakTextAsync(recognizeBM.Text);

                if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);

                    result.Dispose();

                    this.DisPlayMessage(cancellation);

                    throw new Exception("發生系統錯誤，請重新執行");
                }

                Console.WriteLine(recognizeBM.Text);

                // 釋放資源
                result.Dispose();
            }
        }

        /// <summary>
        /// 設定語音服務設定
        /// </summary>
        /// <param name="recognizeBM"></param>
        /// <returns></returns>
        private SpeechConfig GetSpeechConfig(RecognizeBM recognizeBM)
        {
            // 設定 訂用帳戶金鑰 與 訂用帳戶服務的所在區域
            // 免費試用的服務所在區域都是 westus
            var speechConfig =
                SpeechConfig.FromSubscription(recognizeBM.Subkey, recognizeBM.Region);

            // 要辨識的語言
            speechConfig.SpeechRecognitionLanguage = recognizeBM.Language;

            // 設定語音名稱
            speechConfig = this.SetVoiceName(recognizeBM.Language, speechConfig);

            return speechConfig;
        }

        /// <summary>
        /// 設定語音名稱
        /// </summary>
        /// <param name="language">要辨識的語言</param>
        /// <param name="speechConfig">語音服務設定</param>
        /// <returns></returns>
        private SpeechConfig SetVoiceName(string language, SpeechConfig speechConfig)
        {
            // 根據要辨識的語言設定對應的語音名稱
            // 中文 zh-TW-Yating-Apollo、zh-TW-HanHanRUS、zh-TW-Zhiwei-Apollo
            // 英文 en-AU-Catherine、en-US-Jessa24kRUS
            if (language == "zh-TW")
                speechConfig.SpeechSynthesisVoiceName = "zh-TW-Yating-Apollo";

            if (language == "en-AU")
                speechConfig.SpeechSynthesisVoiceName = "en-US-Jessa24kRUS";

            return speechConfig;
        }

        /// <summary>
        /// 發生錯誤，提示訊息
        /// </summary>
        /// <param name="cancellation"></param>
        private void DisPlayMessage(dynamic cancellation)
        {
            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                Console.WriteLine($"CANCELED: Did you update the subscription info?");
            }

            throw new Exception("發生系統錯誤，請重新執行");
        }
    }
}
