﻿using System;
using System.Collections.Generic;
using System.Linq;
using AgentMulder.ReSharper.Domain.Registrations;
using AgentMulder.ReSharper.Domain.Search;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Services.StructuralSearch;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentMulder.Containers.CastleWindsor.Patterns.FromTypes
{
    public abstract class FromTypesBase : RegistrationBase
    {
        private readonly string argumentsElementName;

        protected FromTypesBase(IStructuralSearchPattern pattern, string argumentsElementName)
            : base(pattern)
        {
            this.argumentsElementName = argumentsElementName;
        }

        public override IComponentRegistrationCreator CreateComponentRegistrationCreator()
        {
            return new FromTypesComponentCreator(argumentsElementName);
        }

        private sealed class FromTypesComponentCreator : IComponentRegistrationCreator
        {
            private readonly string elementName;

            public FromTypesComponentCreator(string elementName)
            {
                this.elementName = elementName;
            }

            public IEnumerable<IComponentRegistration> CreateRegistrations(params IStructuralMatchResult[] matchResults)
            {
                foreach (var match in matchResults)
                {
                    IEnumerable<ICSharpArgument> matchedArguments = match.GetMatchedElementList(elementName).OfType<ICSharpArgument>();

                    foreach (ICSharpArgument argument in matchedArguments)
                    {
                        foreach (IComponentRegistration componentRegistration in ComponentRegistrations(match, argument.Value))
                            yield return componentRegistration;
                    }
                }
            }

            private static IEnumerable<IComponentRegistration> ComponentRegistrations(IStructuralMatchResult match, ICSharpExpression expression)
            {
                var typeOfExpression = expression as ITypeofExpression;
                if (typeOfExpression != null)
                {
                     var typeElement = (IDeclaredType)typeOfExpression.ArgumentType;

                     yield return new ComponentRegistration(match.GetDocumentRange(), typeElement.GetTypeElement());
                }

                var arrayExpression = expression as IArrayCreationExpression;
                if (arrayExpression != null)
                {
                    foreach (var initializer in arrayExpression.ArrayInitializer.ElementInitializers.OfType<IExpressionInitializer>())
                    {
                        foreach (IComponentRegistration componentRegistration in ComponentRegistrations(match, initializer.Value))
                        {
                            yield return componentRegistration;
                        }
                    }
                }

                var objectCreationExpression = expression as IObjectCreationExpression;
                if (objectCreationExpression != null)
                {

                }
            }
        }
    }
}