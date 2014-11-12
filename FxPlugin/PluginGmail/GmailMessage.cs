using System;
using System.Web.Mail;

namespace RC.Gmail
{

	/// <summary>
	/// Provides a message object that sends the email through gmail. 
	/// GmailMessage is inherited to <c>System.Web.Mail.MailMessage</c>, so all the mail message features are available.
	/// </summary>
	public class GmailMessage : System.Web.Mail.MailMessage 
	{

		#region CDO Configuration Constants

		private const string SMTP_SERVER		= "http://schemas.microsoft.com/cdo/configuration/smtpserver";
		private const string SMTP_SERVER_PORT	= "http://schemas.microsoft.com/cdo/configuration/smtpserverport";
		private const string SEND_USING			= "http://schemas.microsoft.com/cdo/configuration/sendusing";
		private const string SMTP_USE_SSL		= "http://schemas.microsoft.com/cdo/configuration/smtpusessl";
		private const string SMTP_AUTHENTICATE	= "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate";
		private const string SEND_USERNAME		= "http://schemas.microsoft.com/cdo/configuration/sendusername";
		private const string SEND_PASSWORD		= "http://schemas.microsoft.com/cdo/configuration/sendpassword";

		#endregion

		#region Private Variables

		private static string	_gmailServer = "smtp.gmail.com";
		private static long		_gmailPort = 465;
		private string			_gmailUserName	= string.Empty;
		private string			_gmailPassword	= string.Empty;

		#endregion
		
		#region Public Members

		/// <summary>
		/// Constructor, creates the GmailMessage object
		/// </summary>
		/// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
		/// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
		public GmailMessage(string gmailUserName, string gmailPassword) 
		{
			this.Fields[SMTP_SERVER] = GmailMessage.GmailServer; 
			this.Fields[SMTP_SERVER_PORT] = GmailMessage.GmailServerPort; 
			this.Fields[SEND_USING] = 2;
			this.Fields[SMTP_USE_SSL] = true;
			this.Fields[SMTP_AUTHENTICATE] = 1;
			this.Fields[SEND_USERNAME] = gmailUserName;
			this.Fields[SEND_PASSWORD] = gmailPassword;

			_gmailUserName = gmailUserName;
			_gmailPassword = gmailPassword;
		}

		/// <summary>
		/// Sends the message. If no to address is given the message will be to <c>GmailUserName</c>@Gmail.com
		/// </summary>
		public void Send() 
		{
			try 
			{
				if(this.From == string.Empty) 
				{
					this.From = GmailUserName;
					if(GmailUserName.IndexOf('@') == -1) this.From += "@Gmail.com";
				}

				System.Web.Mail.SmtpMail.Send(this);
			}
			catch(Exception ex) 
			{
				//TODO: Add error handling
				throw ex;
			}
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

		#endregion

		#region Static Members

		/// <summary>
		/// Send a <c>System.Web.Mail.MailMessage</c> through the specified gmail account
		/// </summary>
		/// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
		/// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
		/// <param name="message"><c>System.Web.Mail.MailMessage</c> object to send</param>
		public static void SendMailMessageFromGmail(string gmailUserName, string gmailPassword, MailMessage message) 
		{
			try 
			{
				message.Fields[SMTP_SERVER] = GmailMessage.GmailServer; 
				message.Fields[SMTP_SERVER_PORT] = GmailMessage.GmailServerPort; 
				message.Fields[SEND_USING] = 2;
				message.Fields[SMTP_USE_SSL] = true;
				message.Fields[SMTP_AUTHENTICATE] = 1;
				message.Fields[SEND_USERNAME] = gmailUserName;
				message.Fields[SEND_PASSWORD] = gmailPassword;

				System.Web.Mail.SmtpMail.Send(message);
			}
			catch(Exception ex) 
			{
				//TODO: Add error handling
				throw ex;
			}
		}

		/// <summary>
		/// Sends an email through the specified gmail account
		/// </summary>
		/// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
		/// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
		/// <param name="toAddress">Recipients email address</param>
		/// <param name="subject">Message subject</param>
		/// <param name="messageBody">Message body</param>
		public static void SendFromGmail(string gmailUserName, string gmailPassword, string toAddress, string subject, string messageBody) 
		{
			try 
			{
				GmailMessage gMessage = new GmailMessage(gmailUserName, gmailPassword);
				
				gMessage.To = toAddress;
				gMessage.Subject = subject;
				gMessage.Body = messageBody;
				gMessage.From = gmailUserName;
				if(gmailUserName.IndexOf('@') == -1) gMessage.From += "@Gmail.com";

				System.Web.Mail.SmtpMail.Send(gMessage);
			}
			catch(Exception ex) 
			{
				//TODO: Add error handling
				throw ex;
			}
		}

		/// <summary>
		/// The name of the gmail server, the default is "smtp.gmail.com"
		/// </summary>
		public static string GmailServer 
		{
			get { return _gmailServer; }
			set { _gmailServer = value; }
		}

		/// <summary>
		/// The port to use when sending the email, the default is 465
		/// </summary>
		public static long GmailServerPort 
		{
			get { return _gmailPort; }
			set { _gmailPort = value; }
		}

		#endregion

	} //GmailMessage

} //RC.Gmail
