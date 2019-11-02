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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("模式切換方式如下:");
            Console.WriteLine("語音輸入模式下，說出 手動輸入 或 切換，即切換為手動輸入模式");
            Console.WriteLine("手動輸入模式下，輸入 語音輸入 或 切換，即切換為語音輸入模式");
            Console.WriteLine("任一模式下，輸入 關閉 或 結束，即結束應用程式");
            Console.ForegroundColor = ConsoleColor.White;

            // 實體化 語音辨識 服務
            speechService = new SpeechService();

            // 實體化 語音辨識基本設定用 BM
            var recognizeBM = new RecognizeBM();

            // 要辨識的語言
            recognizeBM.Language = "zh-TW";

            Console.WriteLine("輸入帳戶金鑰:");

            // 取得金鑰
            recognizeBM.Subkey = "eebe5b99a62b42659bff9f301a1f29b8"; // Console.ReadLine();

            // 取得服務端點位置，免費試用帳戶都為 westus
            recognizeBM.Region = "westus";

            // 預設程式啟動提示訊息
            recognizeBM.Text = "語音服務啟動";

            try
            {
                // 執行文字轉語音
                await speechService.TextToSpeak(recognizeBM);

                // 用來切換模式的 flag
                var isTextMode = false;

                while (true)
                {
                    if (isTextMode)
                    {
                        Console.WriteLine("手動輸入:");

                        // 取得輸入文字
                        recognizeBM.Text = Console.ReadLine();

                        // 執行文字轉語音
                        isTextMode = await StartTextToSpeak(recognizeBM, "語音輸入", isTextMode);
                    }
                    else
                    {
                        Console.WriteLine("語音輸入:");

                        // 取得輸入文字
                        recognizeBM.Text = await speechService.SpeakToText(recognizeBM);

                        // 執行文字轉語音
                        isTextMode = await StartTextToSpeak(recognizeBM, "手動輸入", isTextMode);
                    }
                }
            }
            catch
            {
                Console.WriteLine("應用程式執行失敗，請重新執行");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 執行文字轉語音
        /// </summary>
        /// <param name="recognizeBM">語音辨識設定</param>
        /// <param name="keyWord">切換 手動/語音模式 關鍵字</param>
        /// <param name="modeStatus">模式狀態</param>
        /// <returns></returns>
        private async static Task<bool> StartTextToSpeak(
            RecognizeBM recognizeBM, string keyWord, bool modeStatus)
        {
            // 輸入關閉或結束，結束應用程式
            if (recognizeBM.Text.Contains("關閉") || recognizeBM.Text.Contains("結束"))
            {
                Console.WriteLine("應用程式結束，請輸入任意鍵繼續...");
                Console.ReadLine();
                Environment.Exit(0);
            }
                
            // 執行文字轉語音
            await speechService.TextToSpeak(recognizeBM);

            if (recognizeBM.Text.Contains(keyWord) || recognizeBM.Text == "切換")
                return !modeStatus;

            return modeStatus;
        }
    }
}
