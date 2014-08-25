﻿using SPMeta2.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy business data field.
    /// </summary>
    public class BusinessDataFieldDefinition : FieldDefinition
    {
        #region constructors

        public BusinessDataFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.BusinessData;
        }

        #endregion

        #region properties

        /// <summary>
        /// System instance of the target business data field.
        /// </summary>
        public string SystemInstanceName { get; set; }

        /// <summary>
        /// Entity namespace of the target business data field
        /// </summary>
        public string EntityNamespace { get; set; }

        /// <summary>
        /// Entity name of the target business data field
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Name of the the target business data field
        /// </summary>
        public string BdcFieldName { get; set; }

        #endregion
    }
}
