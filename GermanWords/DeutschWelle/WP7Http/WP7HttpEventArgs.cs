using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GermanWords.DeutschWelle.WP7Http
{
    /// <summary>
    /// Http请求参数类
    /// </summary>
    public class WP7HttpEventArgs : EventArgs
    {
        #region 私有成员

        private string _result;
        private bool _is_error;

        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WP7HttpEventArgs()
        {
            this.result = "";
            this.isError = false;
        }

        public WP7HttpEventArgs(string result)
        {
            this.result = result;
            this.isError = false;
        }

        /// <summary>
        /// 结果字符串
        /// </summary>
        public string result
        {
            get { return _result; }
            set { _result = value; }
        }

        /// <summary>
        /// 是否错误
        /// </summary>
        public bool isError
        {
            get { return _is_error; }
            set { _is_error = value; }
        }
    }
}
