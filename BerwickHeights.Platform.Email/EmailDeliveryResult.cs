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

namespace BerwickHeights.Platform.Email
{
    /// <summary>
    /// Result type for the EmailDeliverySvc.
    /// </summary>
    public class EmailDeliveryResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="failureMessage">Message indicating nature of failure. Set to null if successful.</param>
        public EmailDeliveryResult(string failureMessage)
        {
            IsSuccess = string.IsNullOrEmpty(failureMessage);
            FailureMessage = failureMessage ?? string.Empty;
        }

        /// <summary>
        /// Whether or not the email was delivered successfully to SMTP server.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// A message as to why the email was not delivered successfully to SMTP server.
        /// </summary>
        public string FailureMessage { get; private set; }

        /// <inheritDoc/>
        public override string ToString()
        {
            return "EmailDeliveryResult: "
                + "IsSuccess: " + IsSuccess
                + ", FailureMessage: " + FailureMessage;
        }
    }
}
