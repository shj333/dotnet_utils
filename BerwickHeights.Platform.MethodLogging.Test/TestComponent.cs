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

namespace BerwickHeights.Platform.MethodLogging.Test
{
    public interface ITestComponent
    {
        int Foo(string arg1, float arg2, ArgType arg3, string password);
        int FooExcept(string arg1, float arg2, ArgType arg3, string password);
    }

    public class ArgType
    {
        public string StringData { get; set; }
        public int IntData { get; set; }

        public override string ToString()
        {
            return "ArgType: "
                + "StringData: " + StringData
                + ", IntData: " + IntData;
        }
    }


    public class TestComponent : ITestComponent
    {
        public int Foo(string arg1, float arg2, ArgType arg3, string password)
        {
            return 45;
        }
        public int FooExcept(string arg1, float arg2, ArgType arg3, string password)
        {
            throw new Exception("Exception");
        }
    }
}
