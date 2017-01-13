using System;
using FluentAssertions;
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
            tryResolveIconUrl.ShouldNotThrow();
        }

        [TestMethod]
        public void TryResolveIconUrl_WithInvalidCharacter_ReturnsNone()
        {
            var tryResolveIconUrlReslt = _simpleIconResolver.TryResolveIconUrl("\"");
            tryResolveIconUrlReslt.IsNone.Should().BeTrue();
        }
    }
}