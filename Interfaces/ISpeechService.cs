using System;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// 語音辨識用介面
    /// </summary>
    public interface ISpeechService
    {
        /// <summary>
        /// 辨識輸入語音為文字
        /// </summary>
        /// <param name="subkey">訂用帳戶金鑰</param>
        /// <param name="region">訂用帳戶服務的所在區域</param>
        /// <param name="language">要辨識的語言</param>
        /// <returns>辨識後的文字</returns>
        Task<string> SpeakToText(string subkey,string region,string language);

        /// <summary>
        /// 辨識輸入文字為語音
        /// </summary>
        /// <param name="subkey">訂用帳戶金鑰</param>
        /// <param name="region">訂用帳戶服務的所在區域</param>
        /// <param name="language">要辨識的語言</param>
        /// <param name="text">輸入的文字</param>
        /// <returns></returns>
        Task TextToSpeak(string subkey,string region,string language,string text);
    }
}