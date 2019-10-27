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
        /// <param name="language">要辨識的語言</param>
        /// <returns></returns>
        Task SpeakToText(string subkey,string language);

        /// <summary>
        /// 辨識輸入文字為語音
        /// </summary>
        /// <param name="subkey">訂用帳戶金鑰</param>
        /// <param name="language">要辨識的語言</param>
        /// <returns></returns>
        Task TextToSpeak(string subkey, string language);
    }
}
