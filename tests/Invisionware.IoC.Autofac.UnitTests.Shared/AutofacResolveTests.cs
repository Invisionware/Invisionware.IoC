using Autofac;
using Invisionware.IoC;
using Invisionware.IoC.Autofac;
using NUnit.Framework;

namespace IocTests
{

    [TestFixture()]
    public class AutofacResolveTests : ResolveTests
    {
        protected override IResolver GetEmptyResolver()
        {
            return new AutofacResolver(new ContainerBuilder().Build());
        }

        protected override IDependencyContainer GetEmptyContainer()
        {
            return new AutofacContainer(new ContainerBuilder().Build());
        }
    }
}
