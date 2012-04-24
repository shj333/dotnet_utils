/*
 * Copyright 2012 Berwick Heights Software, Inc
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in 
 * compliance with the License. You may obtain a copy of the License at 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under the License is 
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
 * See the License for the specific language governing permissions and limitations under the License.
 *  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.Logging;
using BerwickHeights.Platform.Core.Utils;

namespace BerwickHeights.Platform.Email
{
    /// <inheritDoc/>
    public class EmailDeliverySvc : IEmailDeliverySvc
    {
        #region Private Fields

        private readonly ILogger logger;

        private readonly int port;
        private readonly bool enableSsl;
        private readonly string host;
        private readonly string userName;
        private readonly string password;
        private readonly bool useDefaultCredentials;
        private readonly int timeoutMSecs;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailDeliverySvc(IConfigurationSvc configurationSvc,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.GetLogger(GetType());

            port = configurationSvc.GetIntConfig("SMTPPort", false, 25);
            enableSsl = configurationSvc.GetBooleanConfig("STMPEnableSSL", false);
            host = configurationSvc.GetStringConfig("SMTPHost");
            userName = configurationSvc.GetStringConfig("SMTPUser", false);
            if (!string.IsNullOrEmpty(userName)) password = configurationSvc.GetStringConfig("SMTPPassword");
            timeoutMSecs = configurationSvc.GetIntConfig("STMPTimeoutSecs", false, 1) * 1000;
            useDefaultCredentials = configurationSvc.GetBooleanConfig("SMTPUseDefaultCredentials", false);

            logger.Info("EmailDeliverySvc started with "
                + "Port: " + port
                + ", EnableSSL: " + enableSsl
                + ", Host: " + host
                + ", UserName: " + userName
                + ", TimeoutMSecs: " + timeoutMSecs);
        }

        #endregion

        #region Implementation of IEmailDeliverySvc

        /// <inheritDoc/>
        public EmailDeliveryResult DeliverEmail(string from, IEnumerable<string> toList, string subject, string messageBody,
            bool isBodyHtml, IEnumerable<string> ccList, IEnumerable<string> bccList, IEnumerable<Attachment> attachments)
        {
            SmtpClient client = CreateSmtpClient();
            using (client)
            {
                if (logger.IsDebugEnabled) logger.Debug("Using " + DumpSmtpClient(client));
                MailMessage message = ComposeMessage(from, toList, subject, messageBody, isBodyHtml, ccList, bccList, attachments);
                if (logger.IsDebugEnabled) logger.Debug("Sending" + DumpMessage(message));
                return new EmailDeliveryResult(SendEmail(client, message));
            }

        }

        #endregion

        #region Private Methods

        private SmtpClient CreateSmtpClient()
        {
            SmtpClient client = new SmtpClient();

            client.Port = port;
            client.EnableSsl = enableSsl;
            client.Host = host;
            client.UseDefaultCredentials = useDefaultCredentials;
            if ((!client.UseDefaultCredentials) && (!string.IsNullOrEmpty(userName)))
            {
                client.Credentials = new NetworkCredential(userName, password);
            }
            client.Timeout = timeoutMSecs;

            return client;
        }

        private static MailMessage ComposeMessage(string from, IEnumerable<string> toList, string subject,
            string messageBody, bool isBodyHtml, IEnumerable<string> ccList, IEnumerable<string> bccList,
            IEnumerable<Attachment> attachments)
        {
            ParamChecker.CheckParam(from, "from");
            ParamChecker.CheckParam(toList, "toList");
            ParamChecker.CheckParam(subject, "subject");
            ParamChecker.CheckParam(messageBody, "messageBody");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            foreach (string to in toList.Where(to => !string.IsNullOrEmpty(to)))
            {
                message.To.Add(to);
            }
            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = isBodyHtml;

            if (ccList != null)
            {
                foreach (string cc in ccList.Where(cc => !string.IsNullOrEmpty(cc)))
                {
                    message.CC.Add(cc);
                }
            }

            if (bccList != null)
            {
                foreach (string bcc in bccList.Where(bcc => !string.IsNullOrEmpty(bcc)))
                {
                    message.CC.Add(bcc);
                }
            }

            if (attachments != null)
            {
                foreach (Attachment attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            return message;
        }

        private string SendEmail(SmtpClient client, MailMessage message)
        {
            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                logger.Error("Failed to send message: " + DumpMessage(message) + " using " + DumpSmtpClient(client), e);
                return e.Message;
            }

            return null;
        }

        private static string DumpMessage(MailMessage message)
        {
            StringBuilder attachSB = new StringBuilder("[");
            foreach (Attachment attach in message.Attachments) attachSB.Append(attach.Name + ",");
            attachSB.Append("]");
            return "MailMessage: "
                + "From: " + message.From
                + ", To: " + DumpMailAddrList(message.To)
                + ", Subject: '" + message.Subject
                + "', Body: '" + message.Body
                + "', IsBodyHTML: " + message.IsBodyHtml
                + ", CC: " + DumpMailAddrList(message.CC)
                + ", BCC: " + DumpMailAddrList(message.Bcc)
                + ", Attachments: " + attachSB;
        }

        private static string DumpMailAddrList(IEnumerable<MailAddress> addrs)
        {
            if (addrs == null) return string.Empty;

            StringBuilder sb = new StringBuilder("[");
            foreach (MailAddress to in addrs) sb.Append(to.Address + ",");
            sb.Append("]");
            return sb.ToString();
        }

        private static string DumpSmtpClient(SmtpClient client)
        {
            return "SmtpClient:  "
                + "Port: " + client.Port
                + ", EnableSSL: " + client.EnableSsl
                + ", Host: " + client.Host
                + ", UseDefaultCredentials: " + client.UseDefaultCredentials
                + ", Network Credentials: " + ((client.Credentials == null) ? "none" : "provided")
                + ", TimeoutMSecs: " + client.Timeout;
        }

        #endregion
    }
}
