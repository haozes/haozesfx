using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Core
{
    public class ContactsChangedArg : EventArgs
    {
        private ContactsChangedType changedType;
        public ContactsChangedArg(ContactsChangedType changedType)
        {
            this.changedType = changedType;
        }
        public ContactsChangedType ChangedType
        {
            get { return this.changedType; }
        }
    }

    public enum ContactsChangedType
    {
        LoadCompleted,
        Add,
        Update,
        Delete
    }
}
