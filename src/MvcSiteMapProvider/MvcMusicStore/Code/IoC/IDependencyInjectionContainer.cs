using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Code.IoC
{
    public interface IDependencyInjectionContainer
    {
        object Resolve(Type type);
        T Resolve<T>();
    }
}