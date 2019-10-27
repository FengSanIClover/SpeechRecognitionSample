using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    /// <summary>
    /// 語音辨識基本設定用 BM
    /// </summary>
    public class BaseConfigBM
    {
        /// <summary>
        /// 訂用帳戶金鑰
        /// </summary>
        public string Subkey { get; set; }

        /// <summary>
        /// 訂用帳戶服務的所在區域
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 要辨識的語言
        /// </summary>
        public string Language { get; set; }
    }
}
