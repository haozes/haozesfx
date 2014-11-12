using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public static class SipHelper
    {
        public static string GetCSeqMethod(SipHeadField field)
        {
            //Q: 2 R
            string raw = field.ToString();
            if (!raw.StartsWith(SipHeadFieldName.CSeq))
            {
                throw new ArgumentException("can't get CSeqMethod from :" + field.Value);
            }
            string[] arr = raw.Split(new char[] { ' ' });
            if (arr.Length > 1)
            {
                return arr[2];
            }
            return string.Empty;
        }
    }
}
