using System;
using System.Xml;
using System.Text;
using System.Net;
using System.IO;
using System.Globalization;

namespace RC.Gmail
{

    /// <summary>
    /// Provides an easy method of retreiving and programming against gmail atom feeds.
    /// </summary>
    public class GmailAtomFeed
    {

        #region Private Variables

        //private static string	_gmailFeedUrl	= "https://gmail.google.com/gmail/feed/atom";
        private static string _gmailFeedUrl = "https://mail.google.com/mail/feed/atom";
        private string _gmailUserName = string.Empty;
        private string _gmailPassword = string.Empty;
        private string _feedLabel = string.Empty;
        private string _title = string.Empty;
        private string _message = string.Empty;
        private DateTime _modified = DateTime.MinValue;
        private XmlDocument _feedXml = null;

        private AtomFeedEntryCollection _entryCol = null;

        #endregion


        /// <summary>
        /// Constructor, creates the gmail atom feed object. 
        /// <note>
        /// Creating the object does not get the feed, the <c>GetFeed</c> method must be called to get the current feed.
        /// </note>
        /// </summary>
        /// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
        /// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
        public GmailAtomFeed(string gmailUserName, string gmailPassword)
        {
            _gmailUserName = gmailUserName;
            _gmailPassword = gmailPassword;
            _entryCol = new AtomFeedEntryCollection();
        }

        /// <summary>
        /// Gets the current atom feed for the specified account and loads all properties and collections with the feed data. Any existing data will be replaced by the new feed.
        /// <note>
        /// If the <c>FeedLabel</c> property equals <c>string.Empty</c> the feed for the inbox will be retreived.
        /// </note>
        /// </summary>
        public void GetFeed()
        {
            StringBuilder sBuilder = new StringBuilder();
            byte[] buffer = new byte[8192];
            int byteCount = 0;

            //try
            //{
            string url = GmailAtomFeed.FeedUrl;

            if (this.FeedLabel != string.Empty)
            {
                url += (url.EndsWith("/")) ? string.Empty : "/";
                url += this.FeedLabel;
            }

            System.Net.NetworkCredential credentials = new NetworkCredential(this.GmailUserName, this.GmailPassword);

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Credentials = credentials;

            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();

            while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                sBuilder.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));


            _feedXml = new XmlDocument();
            _feedXml.LoadXml(sBuilder.ToString());

            loadFeedEntries();
            // }
            //catch(Exception ex) 
            //{
            //    //TODO: add error handling
            //    throw ex;
            //}
        }


        /// <summary>
        /// Loads the <c>FeedEntries</c> collection to the data retreived in the feed.
        /// </summary>
        private void loadFeedEntries()
        {
            XmlNamespaceManager nsm = new XmlNamespaceManager(_feedXml.NameTable);
            nsm.AddNamespace("atom", "http://purl.org/atom/ns#");

            _title = _feedXml.SelectSingleNode("/atom:feed/atom:title", nsm).InnerText;
            _message = _feedXml.SelectSingleNode("/atom:feed/atom:tagline", nsm).InnerText;
            _modified = DateTime.Parse(_feedXml.SelectSingleNode("/atom:feed/atom:modified", nsm).InnerText.Replace("T24:", "T00:"));



            int nodeCount = _feedXml.SelectNodes("//atom:entry", nsm).Count;
            string baseXPath = string.Empty;
            _entryCol.Clear();

            for (int i = 1; i <= nodeCount; i++)
            {
                baseXPath = "/atom:feed/atom:entry[position()=" + i.ToString() + "]/atom:";
                string subject = _feedXml.SelectSingleNode(baseXPath + "title", nsm).InnerText;
                string summary = _feedXml.SelectSingleNode(baseXPath + "summary", nsm).InnerText;
                string fromName = string.Empty;
                string fromEmail = string.Empty;
                try
                {
                    fromName = _feedXml.SelectSingleNode(baseXPath + "author/atom:name", nsm).InnerText;
                    fromEmail = _feedXml.SelectSingleNode(baseXPath + "author/atom:email", nsm).InnerText;
                }
                catch
                {
                }
                string id = _feedXml.SelectSingleNode(baseXPath + "id", nsm).InnerText.Split(':')[2];
                DateTime received = DateTime.Now;
                DateTime.TryParse(_feedXml.SelectSingleNode(baseXPath + "issued", nsm).InnerText.Replace("T24:", "T00:"), out received);
                AtomFeedEntry atomEntry = new AtomFeedEntry(subject, summary, fromName, fromEmail, id, received);
                _entryCol.Add(atomEntry);
            }
        }


        /// <summary>
        /// Collection containing the feeds entry objects
        /// </summary>
        public AtomFeedEntryCollection FeedEntries
        {
            get { return _entryCol; }
        }

        /// <summary>
        /// The username of the gmail account that the message will be sent through
        /// </summary>
        public string GmailUserName
        {
            get { return _gmailUserName; }
            set { _gmailUserName = value; }
        }

        /// <summary>
        /// The password of the gmail account that the message will be sent through
        /// </summary>
        public string GmailPassword
        {
            get { return _gmailPassword; }
            set { _gmailPassword = value; }
        }

        /// <summary>
        /// The label to retreive the feeds to. To get the new inbox messages set this to <c>string.Empty</c>.
        /// </summary>
        public string FeedLabel
        {
            get { return _feedLabel; }
            set { _feedLabel = value; }
        }

        /// <summary>
        /// Returns the feed data retreived to gmail
        /// </summary>
        public XmlDocument FeedXml
        {
            get { return _feedXml; }
        }

        /// <summary>
        /// Returns the feed data retreived to gmail
        /// </summary>
        public string RawFeed
        {
            get { return _feedXml.OuterXml; }
        }

        /// <summary>
        /// Returns the <c>/feed/tagline</c> property
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Returns the <c>/feed/title</c> property
        /// </summary>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Returns the <c>/feed/modified</c> property
        /// </summary>
        public DateTime Modified
        {
            get { return _modified; }
        }

        /// <summary>
        /// Base Url for the gmail atom feed, the default is "https://gmail.google.com/gmail/feed/atom"
        /// </summary>
        public static string FeedUrl
        {
            get { return _gmailFeedUrl; }
            set { _gmailFeedUrl = value; }
        }



        /// <summary>
        /// Class for storing the <c>/feed/entry</c> items
        /// </summary>
        public class AtomFeedEntry
        {
            private string _subject = string.Empty;
            private string _summary = string.Empty;
            private string _fromName = string.Empty;
            private string _fromEmail = string.Empty;
            private string _id = string.Empty;
            private DateTime _received = DateTime.MinValue;

            /// <summary>
            /// Constructor, loads the object
            /// </summary>
            /// <param name="subject"><c>/feed/entry/title</c> property</param>
            /// <param name="summary"><c>/feed/entry/summary</c> property</param>
            /// <param name="fromName"><c>/feed/entry/author/name</c> property</param>
            /// <param name="fromEmail"><c>/feed/entry/author/email</c> property</param>
            /// <param name="id"><c>/feed/entry/id</c> property</param>
            /// <param name="received"><c>/feed/entry/issued</c> property</param>
            public AtomFeedEntry(string subject, string summary, string fromName, string fromEmail, string id, DateTime received)
            {
                _subject = subject;
                _summary = summary;
                _fromName = fromName;
                _fromEmail = fromEmail;
                _id = id;
                _received = received;
            }

            /// <summary>
            /// Returns the <c>/feed/entry/title</c> property
            /// </summary>
            public string Subject { get { return _subject; } }

            /// <summary>
            /// Returns the <c>/feed/entry/summary</c> property
            /// </summary>
            public string Summary { get { return _summary; } }

            /// <summary>
            /// Returns the <c>/feed/entry/author/name</c> property
            /// </summary>
            public string FromName { get { return _fromName; } }

            /// <summary>
            /// Returns the <c>/feed/entry/author/email</c> property
            /// </summary>
            public string FromEmail { get { return _fromEmail; } }

            /// <summary>
            /// Returns the <c>/feed/entry/id</c> property
            /// </summary>
            public string Id { get { return _id; } }

            /// <summary>
            /// Returns the <c>/feed/entry/issued</c> property
            /// </summary>
            public DateTime Received { get { return _received; } }

        } //AtomFeedEntry


        /// <summary>
        /// Collection of <c>AtomFeedEntry</c> objects
        /// </summary>
        public class AtomFeedEntryCollection : System.Collections.CollectionBase
        {

            /// <summary>
            /// Indexer for retreiving an <c>AtomFeedEntry</c> object
            /// </summary>
            public AtomFeedEntry this[int index]
            {
                get { return this.List[index] as AtomFeedEntry; }
                set { this.List[index] = value; }
            }

            /// <summary>
            /// Adds an <c>AtomFeedEntry</c> object to the collection
            /// </summary>
            /// <param name="feedEntry"><c>AtomFeedEntry</c> to add</param>
            public void Add(AtomFeedEntry feedEntry) { this.List.Add(feedEntry); }

            /// <summary>
            /// Clears the collection
            /// </summary>
            public new void Clear() { this.List.Clear(); }

            /// <summary>
            /// Returns true if the collection contains the specified object
            /// </summary>
            /// <param name="feedEntry"><c>AtomFeedEntry</c> to find</param>
            /// <returns></returns>
            public bool Contains(AtomFeedEntry feedEntry) { return this.List.Contains(feedEntry); }

            /// <summary>
            /// Returns the position of the first of the <c>AtomFeedEntry</c> object. If it is not found then <c>-1</c> is returned.
            /// </summary>
            /// <param name="feedEntry"><c>AtomFeedEntry</c> to find</param>
            /// <returns></returns>
            public int IndexOf(AtomFeedEntry feedEntry) { return this.List.IndexOf(feedEntry); }

            /// <summary>
            /// Inserts an <c>AtomFeedEntry</c> at the specified position
            /// </summary>
            /// <param name="index">Position to insert at</param>
            /// <param name="feedEntry"><c>AtomFeedEntry</c> to insert</param>
            public void Insert(int index, AtomFeedEntry feedEntry) { this.List.Insert(index, feedEntry); }

            /// <summary>
            /// Removes an <c>AtomFeedEntry</c> to the collection
            /// </summary>
            /// <param name="feedEntry"><c>AtomFeedEntry</c> to be removed</param>
            public void Remove(AtomFeedEntry feedEntry) { this.List.Remove(feedEntry); }

            /// <summary>
            /// Removes an <c>AtomFeedEntry</c> object to the specified position
            /// </summary>
            /// <param name="index">Position of <c>AtomFeedEntry</c> to be removed</param>
            public new void RemoveAt(int index) { this.List.RemoveAt(index); }

        } //AtomFeedEntryCollection

    } //GmailAtomFeed

} //RC.Gmail
