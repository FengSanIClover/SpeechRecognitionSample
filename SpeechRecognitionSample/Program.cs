using Interfaces;
using Services;
using System;
using System.Threading.Tasks;

namespace SpeechRecognitionSample
{
    class Program
    {
        static ISpeechService speechService;

        static async Task Main(string[] args)
        {
            // 實體化 語音辨識 服務
            speechService = new SpeechService();

            // 實體化 語音辨識基本設定用 BM
            var recognizeBM = new RecognizeBM();

            // 判斷選擇的語言，預設為中文
            recognizeBM.Language = "zh-TW"; 

            Console.WriteLine("輸入訂用帳戶金鑰(Your Own Subscription key):");

            // 取得金鑰
            recognizeBM.Subkey = Console.ReadLine();

            // 取得服務端點位置，免費試用帳戶都為 westus
            recognizeBM.Region = "westus";

            // 預設程式啟動提示訊息
            recognizeBM.Text = "語音服務啟動";

            // 執行文字轉語音
            await speechService.TextToSpeak(recognizeBM);

            while (true)
            {
                // 執行語音轉文字
                var inputVal = await speechService.SpeakToText(recognizeBM);

                // 輸入關閉或結束，結束應用程式
                if (inputVal.Contains("關閉") || inputVal.Contains("結束"))
                    return;

                // 將輸入的語音轉文字結果傳給 BM，用來轉成語音
                recognizeBM.Text = inputVal;

                // 執行文字轉語音
                await speechService.TextToSpeak(recognizeBM);
            }
        }
    }
}
