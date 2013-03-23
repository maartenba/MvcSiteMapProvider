﻿using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// ISiteMapNode Visibility Provider contract.
    /// </summary>
    public interface ISiteMapNodeVisibilityProvider
    {
        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata);

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the visibility provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        bool AppliesTo(string providerName);
    }
}
