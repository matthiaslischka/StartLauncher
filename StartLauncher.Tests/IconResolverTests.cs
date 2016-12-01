using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StartLauncher.App.Core;
using StartLauncher.App.DataAccess;

namespace StartLauncher.Tests
{
    [TestClass]
    public class IconResolverTests
    {
        [TestMethod]
        public void TryResolveIconUrl_WithInvalidCharacter_DoesNotThrow()
        {
            var commandDtoWithInvalidCharacterCommand = new CommandDto {Command = "\""};
            Action tryResolveIconUrl = () => { IconResolver.TryResolveIconUrl(commandDtoWithInvalidCharacterCommand); };
            tryResolveIconUrl.ShouldNotThrow();
        }

        [TestMethod]
        public void TryResolveIconUrl_WithInvalidCharacter_ReturnsNone()
        {
            var commandDtoWithInvalidCharacterCommand = new CommandDto {Command = "\""};
            var tryResolveIconUrlReslt = IconResolver.TryResolveIconUrl(commandDtoWithInvalidCharacterCommand);
            tryResolveIconUrlReslt.IsNone.Should().BeTrue();
        }
    }
}