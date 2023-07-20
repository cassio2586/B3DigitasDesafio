using Autofac;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.Services;

namespace B3Digitas.Architecture.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
      builder.RegisterType<GetBookValuesService>()
        .As<IGetBookValuesService>().InstancePerLifetimeScope();
      
  }
}
