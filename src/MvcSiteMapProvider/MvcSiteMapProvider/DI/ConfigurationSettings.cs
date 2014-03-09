using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// These are the default set of configuration settings when using the internal DI container.
    /// </summary>
    public class ConfigurationSettings
    {
        public ConfigurationSettings()
        {
            this.UseExternalDIContainer = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_UseExternalDIContainer", "false"));
            this.EnableSiteMapFile = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_EnableSiteMapFile", "true"));
            this.IncludeRootNodeFromSiteMapFile = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_IncludeRootNodeFromSiteMapFile", "true"));
            this.EnableSiteMapFileNestedDynamicNodeRecursion = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_EnableSiteMapFileNestedDynamicNodeRecursion", "false"));
            this.SiteMapFileName = GetConfigurationValueOrFallback("MvcSiteMapProvider_SiteMapFileName", "~/Mvc.sitemap");
            this.ScanAssembliesForSiteMapNodes = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_ScanAssembliesForSiteMapNodes", "false"));
            this.ExcludeAssembliesForScan = GetConfigurationValueOrFallback("MvcSiteMapProvider_ExcludeAssembliesForScan", "")
                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
            this.IncludeAssembliesForScan = GetConfigurationValueOrFallback("MvcSiteMapProvider_IncludeAssembliesForScan", "")
                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
            this.AttributesToIgnore = GetConfigurationValueOrFallback("MvcSiteMapProvider_AttributesToIgnore", "")
                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
            this.CacheDuration = int.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_CacheDuration", "5"));
            this.ControllerTypeResolverAreaNamespacesToIgnore = GetConfigurationValueOrFallback("MvcSiteMapProvider_ControllerTypeResolverAreaNamespacesToIgnore", "")
                .Split(new char[] { '|', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
            this.DefaultSiteMapNodeVisibiltyProvider = GetConfigurationValueOrFallback("MvcSiteMapProvider_DefaultSiteMapNodeVisibiltyProvider", string.Empty);
            this.EnableLocalization = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_EnableLocalization", "true"));
            this.SecurityTrimmingEnabled = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_SecurityTrimmingEnabled", "false"));
            this.VisibilityAffectsDescendants = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_VisibilityAffectsDescendants", "true"));
            this.EnableSitemapsXml = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_EnableSitemapsXml", "true"));
            this.EnableResolvedUrlCaching = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_EnableResolvedUrlCaching", "true"));
            this.UseTitleIfDescriptionNotProvided = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_UseTitleIfDescriptionNotProvided", "true"));
        }

        public bool UseExternalDIContainer { get; private set; }
        public bool EnableSiteMapFile { get; private set; }
        public bool IncludeRootNodeFromSiteMapFile { get; private set; }
        public bool EnableSiteMapFileNestedDynamicNodeRecursion { get; private set; }
        public string SiteMapFileName { get; private set; }
        public bool ScanAssembliesForSiteMapNodes { get; private set; }
        public IEnumerable<string> ExcludeAssembliesForScan { get; private set; }
        public IEnumerable<string> IncludeAssembliesForScan { get; private set; }
        public IEnumerable<string> AttributesToIgnore { get; private set; }
        public int CacheDuration { get; private set; }
        public IEnumerable<string> ControllerTypeResolverAreaNamespacesToIgnore { get; private set; }
        public string DefaultSiteMapNodeVisibiltyProvider { get; private set; }
        public bool EnableLocalization { get; private set; }
        public bool SecurityTrimmingEnabled { get; private set; }
        public bool VisibilityAffectsDescendants { get; private set; }
        public bool EnableSitemapsXml { get; private set; }
        public bool EnableResolvedUrlCaching { get; private set; }
        public bool UseTitleIfDescriptionNotProvided { get; private set; }

        private string GetConfigurationValueOrFallback(string name, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}
