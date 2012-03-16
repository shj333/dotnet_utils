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
    /// Interface to a loggging service. This is a facade to abstract away the logging implementation.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Determines if messages of priority "debug" will be logged.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Determines if messages of priority "error" will be logged.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Determines if messages of priority "fatal" will be logged.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// Determines if messages of priority "info" will be logged.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Determines if messages of priority "warn" will be logged.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log</param>
        void Debug(string message);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log</param>
        void Error(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The message to log</param>
        void Fatal(string message);

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The message to log</param>
        void Info(string message);

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        void Info(string message, Exception exception);

        /// <summary>
        /// Logs a warn message.
        /// </summary>
        /// <param name="message">The message to log</param>
        void Warn(string message);

        /// <summary>
        /// Logs a warn message.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        void Warn(string message, Exception exception);
    }
}
