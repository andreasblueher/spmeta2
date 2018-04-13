using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class AllowedInlineDownloadedMimeTypeModelNode : TypedModelNode
    {

    }
    public static class AllowedInlineDownloadedMimeTypeDefinitionSyntax
    {
        #region methods

        public static TModelNode AddAllowedInlineDownloadedMimeType<TModelNode>(this TModelNode model, AllowedInlineDownloadedMimeTypeDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddAllowedInlineDownloadedMimeType(model, definition, null);
        }

        public static TModelNode AddAllowedInlineDownloadedMimeType<TModelNode>(this TModelNode model, AllowedInlineDownloadedMimeTypeDefinition definition,
            Action<AlternateUrlModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddAllowedInlineDownloadedMimeTypes<TModelNode>(this TModelNode model, IEnumerable<AllowedInlineDownloadedMimeTypeDefinition> definitions)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
