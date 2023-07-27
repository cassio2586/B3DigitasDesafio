using Autofac;
using B3Digitas.Cripto.Core.Interfaces;
using B3Digitas.Cripto.Core.Services;

namespace B3Digitas.Cripto.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
      builder.RegisterType<GetOrderBookService>()
        .As<IGetOrderBookService>().InstancePerLifetimeScope();
      
  }
}
