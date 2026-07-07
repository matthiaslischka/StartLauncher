using System;
using FluentAssertions;
using FluentOptionals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StartLauncher.App.DataAccess;

namespace StartLauncher.Tests
{
    [TestClass]
    public class SimpleIconResolverTests
    {
        private readonly SimpleIconResolver _simpleIconResolver = new SimpleIconResolver();

        [TestMethod]
        public void TryResolveIconUrl_WithInvalidCharacter_DoesNotThrow()
        {
            Action tryResolveIconUrl = () => { _simpleIconResolver.TryResolveIconUrl("\""); };
            tryResolveIconUrl.Should().NotThrow();
        }

        [TestMethod]
        public void TryResolveIconUrl_WithInvalidCharacter_ReturnsNone()
        {
            var tryResolveIconUrlReslt = _simpleIconResolver.TryResolveIconUrl("\"");
            var isNone = tryResolveIconUrlReslt.Match(_ => false, () => true);
            isNone.Should().BeTrue();
        }
    }
}