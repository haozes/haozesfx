using System;
using System.Collections.Generic;
using System.Text;
using Haozes.FxClient.Core;
using Haozes.FxClient.CommUtil;
using System.Data;
using System.IO;
using System.Xml.XPath;
using System.Linq;


namespace Haozes.FxClient
{
    public class ContactMgr
    {
        private int buddyCount = 0;
        private List<Contact> contactList = new List<Contact>();
        public event EventHandler<ContactsChangedArg> ContactsChanged;

        public IList<Contact> Contacts
        {
            get { return this.contactList; }
        }

        public void InitContactList(string xml)
        {
            XmlParser xmlParser = new XmlParser(xml);
            DataTable dtBuddies = xmlParser.GetDataTable("//results/user-info/contact-list/buddies");
            DataTable dtMobileBuddies = xmlParser.GetDataTable("//results/user-info/contact-list/chat-friends");
            DataTable dtBlockedBuddies = xmlParser.GetDataTable("//results/user-info/contact-list/blacklist");

            xmlParser.Dispose();
            if (dtBuddies != null)
            {
                foreach (DataRow row in dtBuddies.Rows)
                {
                    Contact c = new Contact(
                        new SipUri(row["u"].ToString()),
                        row["i"].ToString(),
                        row["n"].ToString(),
                        string.Empty,
                        ContactType.Buddy);
                    this.contactList.Add(c);
                    this.buddyCount++;
                }
            }

            if (dtMobileBuddies != null)
            {
                foreach (DataRow row in dtMobileBuddies.Rows)
                {
                    Contact c = new Contact(
                        new SipUri(row["u"].ToString()),
                        row["i"].ToString(),
                        string.Empty,
                        string.Empty,
                        ContactType.MobileBuddy);
                    this.contactList.Add(c);
                }
            }
            if (dtBlockedBuddies != null)
            {
                foreach (DataRow row in dtBlockedBuddies.Rows)
                {
                    Contact c = new Contact(
                        new SipUri(row["u"].ToString()),
                        row["i"].ToString(),
                        row["n"].ToString(),
                        string.Empty,
                        ContactType.BlockedBuddy);
                    this.contactList.Add(c);
                }
            }

            this.contactList.Sort(delegate(Contact c1, Contact c2) { return c1.DisplayName.CompareTo(c2.DisplayName); });
        }

        public void PresenceNotify(string xml)
        {
            string id = string.Empty;
            string nickName = string.Empty;
            string impresa = string.Empty;
            string mobileNo = string.Empty;
            string name = string.Empty;
            TextReader reader = new StringReader(xml);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = ((IXPathNavigable)doc).CreateNavigator();
            XPathNodeIterator iter = nav.Select("//events/event/contacts/c");
            while (iter.MoveNext())
            {
                id = iter.Current.GetAttribute("id", string.Empty);
                if (string.IsNullOrEmpty(id))
                    continue;
                Contact contact = null;
                foreach (var c in this.contactList)
                {
                    if (c.UserId == id)
                    {
                        contact = c;
                        break;
                    }
                }
                if (contact != null)
                {
                    this.buddyCount--;
                    XPathNodeIterator pIter = iter.Current.SelectChildren("p", string.Empty);
                    pIter.MoveNext();
                    nickName = pIter.Current.GetAttribute("n", string.Empty);
                    contact.NickName = nickName;
                    impresa = pIter.Current.GetAttribute("i", string.Empty);
                    contact.Impresa = impresa;
                    mobileNo = pIter.Current.GetAttribute("m", string.Empty);
                    long mobile = long.MinValue;
                    long.TryParse(mobileNo, out mobile);
                    contact.MobileNo = mobile;
                }
            }
        }
      
        public IList<Contact> GetContactByType(IList<Contact> list, ContactType contactType)
        {
            IList<Contact> group = new List<Contact>();
            if (list.Count > 0)
            {
                foreach (Contact c in list)
                {
                    if (c.ContactTypeName == contactType)
                        group.Add(c);
                }
            }
            return group;
        }

        public Contact Find(SipUri uri)
        {
            var contact = (from c in this.contactList
                           where string.Equals(c.Uri.ToString(), uri.ToString(), StringComparison.CurrentCultureIgnoreCase) == true
                           select c).SingleOrDefault();
            return contact;
        }

        public Contact Find(string id)
        {
            var contact = (from c in this.contactList
                           where string.Equals(c.Uri.Sid.ToString(), id, StringComparison.CurrentCultureIgnoreCase) == true
                           select c).SingleOrDefault();
            return contact;
        }

        public Contact GetContact(string userid)
        {
            var contact = (from c in this.contactList
                           where string.Equals(c.UserId, userid, StringComparison.CurrentCultureIgnoreCase) == true
                           select c).SingleOrDefault();
            return contact;
        }

        public void Add(Contact c)
        {
            if (this.Find(c.Uri) == null)
            {
                this.contactList.Add(c);
                if (this.ContactsChanged != null)
                {
                    this.ContactsChanged(this, new ContactsChangedArg(ContactsChangedType.Add));
                }
            }
        }

        public void Delete(Contact c)
        {
            if (c != null && this.Find(c.Uri) != null)
            {
                this.contactList.Remove(c);
            }
            if (this.ContactsChanged != null)
            {
                this.ContactsChanged(this, new ContactsChangedArg(ContactsChangedType.Delete));
            }
        }

        public void Delete(SipUri buddyUri)
        {
            Contact c = this.Find(buddyUri);
            this.Delete(c);
        }

        public void Delete(string userid)
        {
            var contact = (from c in contactList
                           where c.UserId.Equals(userid, StringComparison.CurrentCultureIgnoreCase)
                           select c).SingleOrDefault();
            if (contact!=null)
                this.Delete(contact);
        }

        public void Update(Contact c)
        {
            if (this.ContactsChanged != null)
            {
                this.ContactsChanged(this, new ContactsChangedArg(ContactsChangedType.Update));
            }
        }
    }
}
