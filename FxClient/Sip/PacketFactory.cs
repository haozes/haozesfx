using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Core;

namespace Haozes.FxClient.Sip
{
    public class PacketFactory
    {
        private static long keepLiveCSeq = 2;
        public static User Ower;
        public static readonly string DEFAULT_URI = "fetion.com.cn";

        public static SipMessage RegSipcStep1(string cn)
        {
            string msgContent = Environment.NewLine;
            SipRequest req = new SipRequest(SipMethodName.Register, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, "1");
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Register);
            SipHeadField hCN = new SipHeadField("CN", cn);
            SipHeadField hCL = new SipHeadField("CL", "type = \"pc\", version = \"4.0.2510\"");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hCN);
            req.HeadFields.Add(hCL);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage RegSipcStep2(string response)
        {
            string msgContent = "<args><device accept-language=\"default\" machine-code=\"1F2E883F250398DEE59C33DD607A6B4C\" /><caps value=\"3FF\" /><events value=\"7F\" /><user-info mobile-no=\"" + Ower.MobileNo.ToString() + "\" user-id=\"" + Ower.UserId + "\"><personal version=\"0\" attributes=\"v4default;alv2-version;alv2-warn\" /><custom-config version=\"0\" /><contact-list version=\"0\" buddy-attributes=\"v4default\" /></user-info><credentials domains=\"fetion.com.cn;m161.com.cn;www.ikuwa.cn;games.fetion.com.cn;turn.fetion.com.cn\" /><presence><basic value=\"400\" desc=\"\" /><extendeds /></presence></args>";
            SipRequest req = new SipRequest(SipMethodName.Register, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, "1");
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "2 " + SipMethodName.Register);
            SipHeadField hAuth = new SipHeadField(SipHeadFieldName.Authorization, string.Format("Digest algorithm=\"SHA1-sess-v4\",response=\"{0}\"", response));
    
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hAuth);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage RegSipcStep2(string response, string verifyAlgorithm, string type, string picResponse, string chid)
        {
            string msgContent = "<args><device accept-language=\"default\" machine-code=\"1F2E883F250398DEE59C33DD607A6B4C\" /><caps value=\"3FF\" /><events value=\"7F\" /><user-info mobile-no=\"" + Ower.MobileNo.ToString() + "\" user-id=\"" + Ower.UserId + "\"><personal version=\"0\" attributes=\"v4default;alv2-version;alv2-warn\" /><custom-config version=\"0\" /><contact-list version=\"0\" buddy-attributes=\"v4default\" /></user-info><credentials domains=\"fetion.com.cn;m161.com.cn;www.ikuwa.cn;games.fetion.com.cn;turn.fetion.com.cn\" /><presence><basic value=\"400\" desc=\"\" /><extendeds /></presence></args>";
            SipRequest req = new SipRequest(SipMethodName.Register, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, "1");
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "2 " + SipMethodName.Register);
            SipHeadField hAuth = new SipHeadField(SipHeadFieldName.Authorization, string.Format("Digest algorithm=\"SHA1-sess-v4\",response=\"{0}\"", response));
            SipHeadField hAuthExt = new SipHeadField(SipHeadFieldName.Authorization, string.Format("Verify algorithm=\"{0}\",type=\"{1}\",response=\"{2}\",chid=\"{3}\"",verifyAlgorithm,type,picResponse,chid));

            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hAuth);
            req.HeadFields.Add(hAuthExt);
            req.Body = msgContent;
            return req;
        }
      

        public static SipMessage GetContactsInfo(SipUri buddyUri,string userid)
        {
            //string msgContent = string.Format("<args><contacts attributes=\"mobile-no;nickname\" extended-attributes=\"score-level\"><contact uri=\"{0}\" /></contacts></args>", buddyUri.ToString());
            string msgContent = string.Format("<args><contact uri=\"{0}\" user-id=\"{1}\" version=\"0\" /></args>", buddyUri, userid);
            return GetContactsInfo(msgContent);
        }

        private static SipMessage GetContactsInfo(string msgContent)
        {
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "GetContactInfoV4");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        /// <summary>
        /// in chat connection,callid not increase,cseq increase
        /// </summary>
        /// <param name="callid"></param>
        /// <param name="cseq"></param>
        /// <returns></returns>
        public static SipMessage KeepConnectionBusy(string callid,string cseq)
        {
            SipRequest req = new SipRequest(SipMethodName.Options, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, callid);
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, cseq+" O");
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "KeepConnectionBusy");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            return req;
        }

        /// <summary>
        /// in main sipconnection. callid increase,cseq not increase
        /// </summary>
        /// <returns></returns>
        public static SipMessage KeepConnectionBusy()
        {
            return KeepConnectionBusy(Ower.Conncetion.NextCallID().ToString(),"1");
        }

       // public static SipMessage GetKeepAlivePacket()
        public static SipMessage GetKeepAlivePacket()
        {
            SipRequest req = new SipRequest(SipMethodName.Register, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, "1");
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, NextKeepLiveCSeq());
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event,"KeepAlive");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = "<args><credentials domains=\"fetion.com.cn;m161.com.cn;www.ikuwa.cn;games.fetion.com.cn;turn.fetion.com.cn;pos.fetion.com.cn\" /></args>";
            return req;
        }

        public static SipMessage GetGroupList()
        {
            /*
                S fetion.com.cn SIP-C/4.0
                F: 616325003
                I: 2 
                Q: 1 S
                N: PGGetGroupList
                L: 27

                <args><group-list /></args>
             */
            SipRequest req = new SipRequest(SipMethodName.Register, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 S");
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "PGGetGroupList");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = "<args><group-list /></args>";
            return req;
        }

        public static SipMessage SubPresence()
        {
            /*
             * SUB fetion.com.cn SIP-C/4.0
                F: 616325003
                I: 3 
                Q: 1 SUB
                N: PresenceV4
                L: 112

                <args><subscription self="v4default;mail-count;impresa;sms-online-status" buddy="v4default" version="" /></args>
             */
            SipRequest req = new SipRequest(SipMethodName.Subscribe, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 "+SipMethodName.Subscribe);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "PresenceV4");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = "<args><subscription self=\"v4default;mail-count;impresa;sms-online-status\" buddy=\"v4default\" version=\"\" /></args>";
            return req;
        }

        public static SipMessage SMSPacket(SipUri sipUri, string msgContent)
        {
            return SMSPacket(sipUri, msgContent, false);
        }

        /// <summary>
        /// SMS packet.
        /// </summary>
        /// <param name="sipUri">The sipURI.</param>
        /// <param name="msgContent">Content of the Msg.</param>
        /// <param name="isCatMsg">是否是长短信</param>
        /// <returns></returns>
        public static SipMessage SMSPacket(SipUri sipUri, string msgContent,bool isCatMsg)
        {
            string eventName = (isCatMsg == true ? "SendCatSMS" : "SendSMS");

            SipRequest req = new SipRequest(SipMethodName.Message, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Message);
            SipHeadField hTo = new SipHeadField(SipHeadFieldName.To, sipUri.ToString());
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, eventName);
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hTo);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage ReplyMsgPacket(SipMessage packet, string msgContent)
        {
            SipRequest req = new SipRequest(SipMethodName.Message, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, packet.CallID.Value);
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, packet.CSeq.Value);
            SipHeadField hTo = new SipHeadField(SipHeadFieldName.To, packet.To.Value);
            SipHeadField hContentType = new SipHeadField(SipHeadFieldName.ContentType, "text/html-fragment");
            SipHeadField hSupported = new SipHeadField(SipHeadFieldName.Supported, "SaveHistory");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hTo);
            req.HeadFields.Add(hContentType);
            req.HeadFields.Add(hSupported);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage ReplyMsgPacket(Conversation conv, string msgContent)
        {
            SipRequest req = new SipRequest(SipMethodName.Message, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, conv.CallID.ToString());
            //SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, conv.CSeq);
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, conv.Connection.NextCseq().ToString()+" M");

            SipHeadField hTo = new SipHeadField(SipHeadFieldName.To, conv.From.ToString());
            SipHeadField hContentType = new SipHeadField(SipHeadFieldName.ContentType, "text/html-fragment");
            SipHeadField hSupported = new SipHeadField(SipHeadFieldName.Supported, "SaveHistory");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hTo);
            req.HeadFields.Add(hContentType);
            req.HeadFields.Add(hSupported);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage LeaveMsgPacket(SipUri sipUri, string msgContent)
        {
            SipRequest req = new SipRequest(SipMethodName.Message, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "2 M");
            SipHeadField hTo = new SipHeadField(SipHeadFieldName.To, sipUri.ToString());
            SipHeadField hContentType = new SipHeadField(SipHeadFieldName.ContentType, "text/plain");
            SipHeadField hSupported = new SipHeadField(SipHeadFieldName.Supported, "SaveHistory");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hTo);
            req.HeadFields.Add(hContentType);
            req.HeadFields.Add(hSupported);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage RSPInvitePacket(SipMessage packet)
        {
            System.Net.EndPoint localPoint = Ower.Conncetion.LocalEndPoint;
            string port = localPoint.ToString().Split(':')[1];

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("v=0");
            sb.AppendLine("o=-0 0 IN " + localPoint.ToString());
            sb.AppendLine("s=session");
            sb.AppendLine("c=IN IP4 " + localPoint.ToString());
            sb.AppendLine("t=0 0");
            sb.AppendLine(string.Format("m=message {0} sip {1}", port, Ower.Uri.Sid));
            SipResponse response = CreateDefaultResponse(packet);
            response.Body = sb.ToString();
            return response;
        }

        public static SipMessage RegToChatServer(string callid,string ticksAuth)
        {
            string msgContent = Environment.NewLine;
            SipRequest req = new SipRequest(SipMethodName.Register, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, callid);
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Register);
            SipHeadField hAuth = new SipHeadField(SipHeadFieldName.Authorization, "TICKS auth=\""+ticksAuth+"\"");
            SipHeadField hContentType1 = new SipHeadField(SipHeadFieldName.Supported, "text/html-fragment");
            SipHeadField hContentType2 = new SipHeadField(SipHeadFieldName.Supported, "multiparty");
            SipHeadField hContentType3 = new SipHeadField(SipHeadFieldName.Supported, "nudge");
            SipHeadField hContentType4 = new SipHeadField(SipHeadFieldName.Supported, "share-background");
            SipHeadField hContentType5 = new SipHeadField(SipHeadFieldName.Supported, "fetion-show");
            SipHeadField hContentType6 = new SipHeadField(SipHeadFieldName.Supported, "ExModulesApp");
            SipHeadField hContentType7 = new SipHeadField(SipHeadFieldName.Supported, "FileTransferV4");

            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hAuth);
            req.HeadFields.Add(hContentType1);
            req.HeadFields.Add(hContentType2);
            req.HeadFields.Add(hContentType3);
            req.HeadFields.Add(hContentType4);
            req.HeadFields.Add(hContentType5);
            req.HeadFields.Add(hContentType6);
            req.HeadFields.Add(hContentType7);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage RSPReceiveMsgPacket(SipMessage packet)
        {
            return CreateDefaultResponse(packet);
        }

        public static SipMessage RSPBye(SipMessage packet)
        {
            return CreateDefaultResponse(packet);
        }

        private static string NextKeepLiveCSeq()
        {
            return ++keepLiveCSeq + " " + SipMethodName.Register;
        }

        private static SipResponse CreateDefaultResponse(SipMessage packet)
        {
            SipResponse response = new SipResponse(200, "OK");
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, packet.CallID.Value);
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, packet.CSeq.Value);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, packet.From.Value);
            if (packet.To != null)
            {
                SipHeadField hTo = new SipHeadField(SipHeadFieldName.To, packet.To.Value);
                response.HeadFields.Add(hTo);
            }
            if (packet.Supported != null)
            {
                SipHeadField hSupported = new SipHeadField(SipHeadFieldName.Supported, packet.Supported.Value);
                response.HeadFields.Add(hSupported);
            }
            response.HeadFields.Add(hFrom);
            response.HeadFields.Add(hCallID);
            response.HeadFields.Add(hCSeq);

            response.Body = Environment.NewLine;

            return response;
        }

        public static SipMessage HandleContactRequest(string userid)
        {
            //string msgContent = string.Format("<args><contacts><buddies><buddy uri=\"{0}\" result=\"1\" buddy-lists=\"\" expose-mobile-no=\"0\" expose-name=\"0\" /></buddies></contacts></args>", buddyUri.ToString());
            string msgContent = string.Format("<args><contacts><buddies><buddy user-id=\"{0}\" result=\"1\" buddy-lists=\"\" local-name=\"\" expose-mobile-no=\"1\" expose-name=\"1\" /></buddies></contacts></args>", userid);
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "HandleContactRequestV4");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage AddBuddy(SipUri buddyUri, string desc)
        {
            if (string.IsNullOrEmpty(desc))
                desc = Ower.NickName;
            string msgContent = string.Format("<args><contacts><buddies><buddy uri=\"{0}\" buddy-lists=\"\" desc=\"{1}\" expose-mobile-no=\"1\" expose-name=\"1\" addbuddy-phrase-id=\"0\" /></buddies></contacts></args>", buddyUri.ToString(), desc);
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "AddBuddyV4");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage DeleteBuddy(string  userid)
        {
            string msgContent = string.Format("<args><contacts><buddies><buddy user-id=\"{0}\" /></buddies></contacts></args>",userid);
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "DeleteBuddyV4");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage SetPresence(PresenceStatus statu)
        {
            string msgContent = string.Format(string.Format("<args><presence><basic value=\"{0}\" /></presence></args>",(int)statu));
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "SetPresence");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage PGSetPresence()
        {
            string msgContent = string.Format("<args><groups /></args>");
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "PGSetPresence");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage GetGlobalPermission()
        {
            string msgContent = string.Format("<args><permissions objects=\"all\" /></args>");
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "GetGlobalPermission");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }

        public static SipMessage GetContactPermission()
        {
            string msgContent = string.Format("<args><permissions all=\"1\" objects=\"all\" /></args>");
            SipRequest req = new SipRequest(SipMethodName.Service, DEFAULT_URI);
            SipHeadField hFrom = new SipHeadField(SipHeadFieldName.From, Ower.Uri.Sid.ToString());
            SipHeadField hCallID = new SipHeadField(SipHeadFieldName.CallID, Ower.Conncetion.NextCallID().ToString());
            SipHeadField hCSeq = new SipHeadField(SipHeadFieldName.CSeq, "1 " + SipMethodName.Service);
            SipHeadField hEvent = new SipHeadField(SipHeadFieldName.Event, "GetContactPermission");
            req.HeadFields.Add(hFrom);
            req.HeadFields.Add(hCallID);
            req.HeadFields.Add(hCSeq);
            req.HeadFields.Add(hEvent);
            req.Body = msgContent;
            return req;
        }
    }
}
