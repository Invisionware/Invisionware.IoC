# Invisionware.Ioc
Invisionware library for working with multiple IoC and DI frameworks

Support for following libraries
- Autofac
- Ninject

## Usage
```
// Setup IoC
var container = new Invisionware.Ioc.Autofac.AutofacContainer(new Autofac.ContainerBuilder().Build());

// Register this container within the IoC framework so we can access it later if we need to register additional objects
container.Register<IDependencyContainer>(t => container);

// Set the GLOBAL Resolver (makes it easier to use)
Resolver.SetResolver(container.GetResolver());

// Use the GLOBAL Resolver to access the current IoC instance
var c = Resolver.Resolve<IDependencyContainer>();

// Now we can register a new object
c.Register(...)
```
