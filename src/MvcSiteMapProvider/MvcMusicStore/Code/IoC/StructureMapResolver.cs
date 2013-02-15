namespace MvcMusicStore.Code.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using StructureMap;
    using MvcSiteMapProvider.IoC;

    public class StructureMapResolver : IResolver
    {
        private readonly StructureMap.Container container;

        public StructureMapResolver(StructureMap.Container container)
        {
            this.container = container;
        }

        #region IResolver Members

        public void Inject(object instance)
        {
            container.BuildUp(instance);
        }

        #endregion
    }
}