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

namespace BerwickHeights.Platform.Core.Logging
{
    /// <summary>
    /// Interface to a factory service that produces loggers that implement the ILogger interface. ILogger is a facade 
    /// to abstract away the logging implementation (e.g., Log4Net).
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Returns an transient (multi-instance, non-singleton) instance of a logger that implements ILogger. The 
        /// logger uses the given consumerType to differentiate the messages that the consumer produces in the log.
        /// </summary>
        /// <param name="consumerType">The type of the consumer.</param>
        ILogger GetLogger(Type consumerType);

        /// <summary>
        /// Returns an transient (multi-instance, non-singleton) instance of a logger that implements ILogger. The 
        /// logger uses the given name to differentiate the messages that the consumer produces in the log. Most 
        /// consumers will use the other implementation of GetLogger() passing the consumer type.
        /// </summary>
        /// <param name="name">The name used to differentiate messages in the log.</param>
        ILogger GetLogger(string name);
    }
}
