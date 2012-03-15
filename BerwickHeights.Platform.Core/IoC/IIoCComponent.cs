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

namespace BerwickHeights.Platform.Core.IoC
{
    /// <summary>
    /// Empty interface that is used to find components to install in registration-by-convention logic. 
    /// All components that are to be automatically registered using configuration-by-convention should
    /// have their interfaces inherit this interface so that they'll be registered in the IoC container.
    /// </summary>
    public interface IIoCComponent
    {
    }
}
