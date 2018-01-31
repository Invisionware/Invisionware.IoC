#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace IocTests
{
    using Invisionware.IoC;
    using Invisionware.IoC.Ninject;

    [TestFixture()]
    public class NinjectResolveTests : ResolveTests
    {
        protected override IResolver GetEmptyResolver()
        {
            return GetEmptyContainer().GetResolver();
        }

        protected override IDependencyContainer GetEmptyContainer()
        {
#if WINDOWS_PHONE
            return new NinjectContainer(new Ninject.StandardKernel());
#else
            return new NinjectContainer();
#endif
        }
    }
}
