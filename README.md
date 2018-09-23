# Invisionware.Ioc
Invisionware library for working with multiple IoC and DI frameworks

Support for following libraries
- Autofac
- Ninject

## Usage
```
// Setup IoC
var container = new Invisionware.Ioc.Autofac.AutofacContainer(new Autofac.ContainerBuilder().Build());
Resolver.SetResolver(container.GetResolver());

container.Register<IDependencyContainer>(t => container);

var c = Resolver.Resolve<IDependencyContainer>();
```
