using System;
using Autofac;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StartLauncher.App;

namespace StartLauncher.Tests
{
    [TestClass]
    public class AutofacTests
    {
        [TestMethod]
        public void ResolveProgram_ShouldNotThrow()
        {
            var appContainer = new AppContainerBuilder().Build();
            Action resolveApp = () => appContainer.Resolve<App.App>();
            resolveApp.ShouldNotThrow();
        }
    }
}