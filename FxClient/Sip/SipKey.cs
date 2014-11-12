using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public class SipKey
    {
        private static IDictionary<string, string> sipMethodNames = new Dictionary<string, string>();
        private static IDictionary<string, string> sipHeaderFields = new Dictionary<string, string>();
        static SipKey()
        {
            //method
            sipMethodNames.Add(SipMethodName.Ack, string.Empty);
            sipMethodNames.Add(SipMethodName.Benotify, string.Empty);
            sipMethodNames.Add(SipMethodName.Bye, string.Empty);
            sipMethodNames.Add(SipMethodName.Cancel, string.Empty);
            sipMethodNames.Add(SipMethodName.Info, string.Empty);
            sipMethodNames.Add(SipMethodName.Invite, string.Empty);
            sipMethodNames.Add(SipMethodName.Message, string.Empty);
            sipMethodNames.Add(SipMethodName.Negotiate, string.Empty);
            sipMethodNames.Add(SipMethodName.Notify, string.Empty);
            sipMethodNames.Add(SipMethodName.Options, string.Empty);
            sipMethodNames.Add(SipMethodName.Refer, string.Empty);
            sipMethodNames.Add(SipMethodName.Register, string.Empty);
            sipMethodNames.Add(SipMethodName.Service, string.Empty);
            sipMethodNames.Add(SipMethodName.Subscribe, string.Empty);
            //header
            sipHeaderFields.Add(SipHeadFieldName.Authorization, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.CallID, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Contact, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.ContentEncoding, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.ContentLength, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.ContentType, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.CSeq, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Date, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.EndPoints, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Event, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Expires, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.From, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.MessageID, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.ReferredBy, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.ReferTo, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Require, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.RosterManager, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Source, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Supported, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.To, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.Unsupported, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.WWWAuthenticate, string.Empty);
            sipHeaderFields.Add(SipHeadFieldName.AL, string.Empty);
        }

        public static IDictionary<string, string> HeadFieldList
        {
            get
            {
                return sipHeaderFields;
            }
        }

        public static IDictionary<string, string> MethodNameList
        {
            get
            {
                return sipMethodNames;
            }
        }
    }
}
