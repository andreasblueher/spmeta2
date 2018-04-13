using System;
using System.Runtime.Serialization;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy alternate URL.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.AllowedInlineDownloadedMimeTypes", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ParentHostCapability(typeof(WebApplicationDefinition))]
    public class AllowedInlineDownloadedMimeTypeDefinition : DefinitionBase
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string MimeType { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<AllowedInlineDownloadedMimeTypeDefinition>(this)
                .AddPropertyValue(p => p.MimeType)

                .ToString();
        }

        #endregion
    }
}
