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
            this.SiteMapFileName = GetConfigurationValueOrFallback("MvcSiteMapProvider_SiteMapFileName", "~/Mvc.sitemap");
            this.ScanAssembliesForSiteMapNodes = bool.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_ScanAssembliesForSiteMapNodes", "false"));
            this.ExcludeAssembliesForScan = GetConfigurationValueOrFallback("MvcSiteMapProvider_ExcludeAssembliesForScan", "")
                .Split(new char[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.IncludeAssembliesForScan = GetConfigurationValueOrFallback("MvcSiteMapProvider_IncludeAssembliesForScan", "")
                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.AttributesToIgnore = GetConfigurationValueOrFallback("MvcSiteMapProvider_AttributesToIgnore", "")
                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.CacheDuration = int.Parse(GetConfigurationValueOrFallback("MvcSiteMapProvider_CacheDuration", "5"));
        }

        public bool UseExternalDIContainer { get; private set; }
        public string SiteMapFileName { get; private set; }
        public bool ScanAssembliesForSiteMapNodes { get; private set; }
        public IEnumerable<string> ExcludeAssembliesForScan { get; private set; }
        public IEnumerable<string> IncludeAssembliesForScan { get; private set; }
        public IEnumerable<string> AttributesToIgnore { get; private set; }
        public int CacheDuration { get; private set; }


        private string GetConfigurationValueOrFallback(string name, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (!String.IsNullOrEmpty(value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}
