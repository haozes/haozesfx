using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Sip
{
    public enum SipEvent
    {
        AddBuddy,
        AddMobileBuddy,
        Contact,
        Conversation,
        GetContactInfoV4,
        HandleContactRequestV4,
        Permission,
        PresenceV4,
        Registration,
        SyncUserInfoV4,//2010 协议
        None
    }
}
