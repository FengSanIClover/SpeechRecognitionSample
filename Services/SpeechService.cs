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

            // 建立語音辨識器類別
            using (var recognizer = new SpeechRecognizer(config))
            {
                // RecognizeOnceAsync 此方法 僅適用於單個語音識別，如命令或查詢，輸入時間小於 15 秒
                // 如果需要長時間運行的多話語識別，請改用 StartContinuousRecognitionAsync()， 
                // 輸入時間大於 15 秒
                var result = await recognizer.RecognizeOnceAsync();
                
                // 判斷回傳執行結果狀態
                this.CheckReason(result);

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

            // 建立語音合成器類別
            using (var synthesizer = new SpeechSynthesizer(config))
            {
                Console.WriteLine("處理中...");

                // 判斷輸入是否為空白
                if (string.IsNullOrEmpty(recognizeBM.Text.Trim()))
                    recognizeBM.Text = "無法辨識輸入，請重新輸入\n";

                // 將文字合成為語音
                using (var result = await synthesizer.SpeakTextAsync(recognizeBM.Text))
                {
                    // 判斷回傳執行結果狀態
                    this.CheckReason(result);

                    Console.WriteLine($"系統辨識為: {recognizeBM.Text}\n");
                }
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
            if (language == "zh-TW")
                speechConfig.SpeechSynthesisVoiceName = "zh-TW-Yating-Apollo";

            return speechConfig;
        }

        /// <summary>
        /// 判斷回傳執行結果狀態
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool CheckReason(dynamic result)
        {
            // 發生錯誤
            if (result.Reason == ResultReason.Canceled)
            {
                var typeName = result.GetType().Name;

                // 發生錯誤，提示訊息
                if (typeName == "RecognitionResult")
                    this.DisPlayMessage(CancellationDetails.FromResult(result));

                if (typeName == "SpeechSynthesisResult")
                    this.DisPlayMessage(SpeechSynthesisCancellationDetails.FromResult(result));
            }

            // 如果 執行結果為 語音轉文字類型
            // 設定 辨識後的文字
            if (result as RecognitionResult != null)
                recognizeText = result.Text;

            // 無法辨識輸入結果
            if (result.Reason == ResultReason.NoMatch)
            {
                recognizeText = $"無法辨識輸入語音，請重新輸入\n";
            }

            return true;
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
