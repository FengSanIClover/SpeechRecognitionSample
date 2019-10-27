using Interfaces;
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
        /// <param name="configBM">語音辨識基本設定用</param>
        /// <returns>辨識後的文字</returns>
        Task<string> SpeakToText(BaseConfigBM configBM);

        /// <summary>
        /// 辨識輸入文字為語音
        /// </summary>
        /// <param name="textToSpeakBM">輸入文字為語音用</param>
        /// <returns></returns>
        Task TextToSpeak(TextToSpeakBM textToSpeakBM);
    }
}