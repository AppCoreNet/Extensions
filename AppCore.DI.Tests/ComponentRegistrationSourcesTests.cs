// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class ComponentRegistrationSourcesTests
    {
        private class RegistrationSource : IComponentRegistrationSource
        {
            public Type ContractType { get; private set; }

            public ComponentLifetime DefaultLifetime { get; private set; }

            public void WithContract(Type contractType)
            {
                ContractType = contractType;
            }

            public void WithDefaultLifetime(ComponentLifetime lifetime)
            {
                DefaultLifetime = lifetime;
            }

            public IEnumerable<ComponentRegistration> BuildRegistrations()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void AddSetsContractTypeOnSource()
        {
            Type contractType = typeof(IComparable);

            var sources = new ComponentRegistrationSources()
                .WithContract(contractType);

            var source = new RegistrationSource();
            sources.Add(source);

            source.ContractType.Should()
                  .Be(contractType);
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void AddSetsDefaultLifetimeOnSource(ComponentLifetime lifetime)
        {
            var sources = new ComponentRegistrationSources()
                .WithDefaultLifetime(lifetime);

            var source = new RegistrationSource();
            sources.Add(source);

            source.DefaultLifetime.Should()
                  .Be(lifetime);
        }
    }
}