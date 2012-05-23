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

using BerwickHeights.Platform.PerfTest.Model;
using FluentNHibernate.Automapping;

namespace BerwickHeights.Platform.PerfTest.DAL.NHibernate
{
    /// <summary>
    /// Configures FluentNHibernate to map the domain types used in PerfTest.
    /// </summary>
    public static class PerfTestFluentConfig
    {
        /// <summary>
        /// Configures FluentNHibernate to map the domain types used in PerfTest.
        /// </summary>
        public static AutoPersistenceModel AutoMap(AutoPersistenceModel model)
        {
            model
                .AddEntityAssembly(typeof(TestSuiteResult).Assembly)
                .Override<TestSuiteResult>(map =>
                {
                    map.Id(x => x.TestSuiteResultId);
                    map.HasMany(x => x.SystemInfoList).AsSet().Inverse().Cascade.All().Not.LazyLoad();
                    map.HasMany(x => x.TestResultList).AsSet().Inverse().Cascade.All().Not.LazyLoad();
                    map.Map(x => x.UserId).CustomSqlType("UNIQUEIDENTIFIER");
                })
                .Override<SystemInfo>(map =>
                {
                    map.Id(x => x.SystemInfoId);
                    map.References(x => x.TestSuiteResult, "TestSuiteResultID").Cascade.None();
                })
                .Override<TestResult>(map =>
                {
                    map.Id(x => x.TestResultId);
                    map.HasMany(x => x.TimingDataList).AsSet().Inverse().Cascade.All().Not.LazyLoad();
                    map.References(x => x.TestSuiteResult, "TestSuiteResultID").Cascade.None();
                })
                .Override<TimingData>(map =>
                {
                    map.Id(x => x.TimingDataId);
                    map.References(x => x.TestResult, "TestResultID").Cascade.None();
                });
            return model;
        }
    }
}
