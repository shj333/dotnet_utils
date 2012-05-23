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
using BerwickHeights.Platform.Core.Utils;

namespace BerwickHeights.Platform.Core.Model.Results
{
    /// <summary>
    /// A message returned from a call to a service. These messages are returned as part of ISvcResult.
    /// </summary>
    public class SvcResultMessage
    {
        #region Constructors

        /// <summary>
        /// Empty constructor needed by XmlSerializer, etc
        /// </summary>
        public SvcResultMessage()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SvcResultMessage(SvcResultMessageTypes messageType, string resourceId, params string[] arguments) 
        {
            MessageType = messageType;
            ResourceId = resourceId;
            if (arguments != null)
            {
                foreach (string arg in arguments) ((IList<string>)Arguments).Add(arg);
            }
            MessageTime = DateTime.UtcNow;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The type of this message (error, informational, statistic, etc).
        /// </summary>
        public virtual SvcResultMessageTypes MessageType { get; private set; }

        /// <summary>
        /// The resource ID that is a key into localized messages displayed to user.
        /// </summary>
        public virtual string ResourceId { get; private set; }

        /// <summary>
        /// The list of substitution arguments for this message.
        /// </summary>
        public virtual IEnumerable<string> Arguments
        {
            get { return arguments ?? (arguments = new List<string>()); }
        }
        private List<string> arguments;

        /// <summary>
        /// The time this message was created.
        /// </summary>
        public virtual DateTime MessageTime { get; private set; }

        #endregion

        #region Override Methods

        /// <inheritDoc/>
        public override string ToString()
        {
            return "SvcResultMessage: "
                + "MessageType: " + MessageType
                + ", ResourceId: " + ResourceId
                + ", Arguments: " + ListUtils.ListToString(Arguments)
                + ", MessageTime: " + MessageTime;
        }

        /// <summary>
        /// This takes a string.Format() string and applies it using the Arguments property. This can be 
        /// used, for example, when the format string is derived from a resource file.
        /// </summary>
        /// <param name="formatString">string.Format() format string</param>
        /// <returns>formatted string</returns>
        public virtual string ToString(string formatString)
        {
            return Arguments.Any() 
                ? string.Format(formatString, Arguments.ToArray()) 
                : formatString;
        }

        #endregion    
    }
}
