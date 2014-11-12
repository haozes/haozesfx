using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Core
{
    public class ErrManager
    {
        public static event EventHandler<FxErrArgs> Erroring;

        public static void RaiseError(FxErrArgs e)
        {
            if (Erroring != null)
            {
                Erroring(null, e);
            }
        }
    }

    public enum ErrLevel
    {
        Normal,
        Critical,
        Fatal
    }

    public class FxErrArgs : EventArgs
    {
        private ErrLevel level = ErrLevel.Normal;
        private Exception ex = null;
        private string summary = string.Empty;

        public FxErrArgs(Exception ex)
            : this(ErrLevel.Normal, ex)
        {
        }

        public FxErrArgs(ErrLevel level, Exception ex)
            : this(level, string.Empty, ex)
        {
        }

        public FxErrArgs(ErrLevel level, string summary, Exception ex)
        {
            this.level = level;
            this.summary = summary;
            this.ex = ex;
        }

        public string Summary
        {
            get { return this.summary; }
            set { this.summary = value; }
        }

        public ErrLevel Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public Exception InnerException
        {
            get { return this.ex; }
            set { this.ex = value; }
        }

        public override string ToString()
        {
            string str = string.Format("ErrLevel:{0} {1} Detail:{1}", this.level, this.summary, this.ex.ToString());
            return str;
        }
    }
}
