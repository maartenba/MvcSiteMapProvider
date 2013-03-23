﻿using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract to provide a caching technology agnostic way of passing a cache depencency.
    /// </summary>
    public interface ICacheDependency
    {
        object Dependency { get; }
    }
}
