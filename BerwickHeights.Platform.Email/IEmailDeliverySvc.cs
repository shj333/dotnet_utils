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

using System.Collections.Generic;
using System.Net.Mail;
using BerwickHeights.Platform.Core.IoC;

namespace BerwickHeights.Platform.Email
{
    /// <summary>
    /// Interface to a service that sends email to an SMTP server for delivery.
    /// </summary>
    public interface IEmailDeliverySvc : IIoCComponent
    {
        /// <summary>
        /// Delivers given message to the configured SMTP server.
        /// </summary>
        /// <param name="from">The sender of the email.</param>
        /// <param name="toList">The list of email recipients.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="messageBody">The message body for the email.</param>
        /// <param name="isBodyHtml">Whether or not the message body is in HTML format.</param>
        /// <param name="ccList">An optional list of people to receive a copy of the message. Set to
        /// <code>null</code> if not applicable to the email message.</param>
        /// <param name="bccList">An optional list of people to receive a blind copy of the message. Set to
        /// <code>null</code> if not applicable to the email message.</param>
        /// <param name="attachments">An optional list of attached files for the message. Set to <code>null</code> if 
        /// email message as no attachments.</param>
        EmailDeliveryResult DeliverEmail(string from, IEnumerable<string> toList, string subject, string messageBody,
            bool isBodyHtml, IEnumerable<string> ccList = null, IEnumerable<string> bccList = null,
            IEnumerable<Attachment> attachments = null);
    }
}
