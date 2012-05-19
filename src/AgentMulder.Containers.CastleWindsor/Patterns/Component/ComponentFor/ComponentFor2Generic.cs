﻿using AgentMulder.ReSharper.Domain.Registrations;
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch;
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch.Placeholders;
using JetBrains.ReSharper.Psi.Services.StructuralSearch;

namespace AgentMulder.Containers.CastleWindsor.Patterns.Component.ComponentFor
{
    internal sealed class ComponentFor2Generic : ComponentForBasePattern
    {
        private static readonly IStructuralSearchPattern pattern =
            new CSharpStructuralSearchPattern("$component$.For<$service$, $forward1$>()",
                new ExpressionPlaceholder("component", "Castle.MicroKernel.Registration.Component"),
                new TypePlaceholder("service"),
                new TypePlaceholder("forward1"));

        public ComponentFor2Generic(params ComponentImplementationBasePattern[] implementedByPatterns)
            : base(pattern, "service", implementedByPatterns)
        {
        }
    }
}