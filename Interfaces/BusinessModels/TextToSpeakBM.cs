using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    /// <summary>
    /// 輸入文字為語音用 BM
    /// </summary>
    public class TextToSpeakBM : BaseConfigBM
    {
        /// <summary>
        /// 輸入的文字
        /// </summary>
        public string Text { get; set; }
    }
}
