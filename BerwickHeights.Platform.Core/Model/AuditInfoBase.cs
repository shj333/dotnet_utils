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
using BerwickHeights.Platform.Core.Utils;

namespace BerwickHeights.Platform.Core.Model
{
    /// <summary>
    /// Abstract base class for auditing information (who and when the row was created/last modified)
    /// </summary>
    public abstract class AuditInfoBase
    {
        private Guid createdBy;
        private Guid modifiedBy;


        /// <summary>
        /// Empty constructor.
        /// </summary>
        protected AuditInfoBase() { }

        /// <summary>
        /// Constructor for new instance from database record.
        /// </summary>
        protected AuditInfoBase(DateTime created, string createdBy, DateTime modified, string modifiedBy)
        {
            Created = created;
            this.createdBy = Guid.Parse(createdBy);
            Modified = modified;
            this.modifiedBy = Guid.Parse(modifiedBy);
        }

        /// <summary>
        /// Constructor for cloning.
        /// </summary>
        protected AuditInfoBase(AuditInfoBase auditInfo)
        {
            // Set auditing info based on original since we're cloning
            Created = auditInfo.Created;
            createdBy = auditInfo.createdBy;
            Modified = auditInfo.Modified;
            modifiedBy = auditInfo.modifiedBy;
        }

        /// <summary>
        /// When the record was created.
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Who created the record.
        /// </summary>
        public virtual string CreatedBy
        {
            get { return StringUtils.SafeToString(createdBy); }
            set { createdBy = Guid.Parse(value); }
        }

        /// <summary>
        /// When the record was last modified.
        /// </summary>
        public virtual DateTime Modified { get; set; }

        /// <summary>
        /// Who last modifed the record.
        /// </summary>
        public virtual string ModifiedBy
        {
            get { return StringUtils.SafeToString(modifiedBy); }
            set { modifiedBy = Guid.Parse(value); }
        }

        /// <summary>
        /// Used by NHibernate interceptor. If set to true, then the interceptor will
        /// not set the Modified and ModifedBy columns to the current time and current
        /// user before saving. Instead, the Modified and ModifiedBy columns will be 
        /// saved as these values exist in the object. This is useful for cloning GBO
        /// elements since these values should remain consistent between the original
        /// and the clone. NB: This value is not persisted in the database.
        /// </summary>
        public virtual bool DoNotSetModified { get; set; }


        /// <inheritDoc/>
        public override string ToString()
        {
            return ", Created: " + Created
                + ", CreatedBy: " + CreatedBy
                + ", Modified: " + Modified
                + ", ModifiedBy: " + ModifiedBy
                + ", DoNotSetModified: " + DoNotSetModified;
        }

        /// <inheritDoc/>
        public override bool Equals(object other)
        {
            if (this == other) return true;

            AuditInfoBase auditInfo = other as AuditInfoBase;
            if (auditInfo == null) return false;

            if (!DateTimeComparison.DateTimesAreEqualExcludingMilliseconds(Created, auditInfo.Created)) return false;
            if (!Equals(CreatedBy, auditInfo.CreatedBy)) return false;
            if (!DateTimeComparison.DateTimesAreEqualExcludingMilliseconds(Modified, auditInfo.Modified)) return false;
            if (!Equals(ModifiedBy, auditInfo.ModifiedBy)) return false;

            return true;
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = Created.GetHashCode();
                if (CreatedBy != null) result = 29 * result + CreatedBy.GetHashCode();
                result = 29 * result + Modified.GetHashCode();
                if (ModifiedBy != null) result = 29 * result + ModifiedBy.GetHashCode();
                return result;
            }
        }
    }
}
