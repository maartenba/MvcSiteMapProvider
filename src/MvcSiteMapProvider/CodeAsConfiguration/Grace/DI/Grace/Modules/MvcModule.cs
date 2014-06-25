using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Grace.DependencyInjection;

namespace DI.Grace.Modules
{
    public class MvcModule : IConfigurationModule
    {
        public void Configure(IExportRegistrationBlock registrationBlock)
        {

            registrationBlock.Export(AllTypes())
                             .BasedOn<IController>()
                             .ByType()
                             .ExternallyOwned();
        }

        private IEnumerable<Type> AllTypes()
        {
            return GetType().Assembly.ExportedTypes;
        }
    }
}
