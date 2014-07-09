using System;
using Mantle.Configuration.Interfaces;
using Ninject.Activation;
using Ninject.Syntax;

namespace Mantle.Ninject
{
// ReSharper disable once InconsistentNaming
    public static class IBindingOnSyntaxExtensions
    {
        public static IBindingOnSyntax<T> ConfigureUsing<T>(this IBindingOnSyntax<T> bindingSyntax,
                                                            IConfigurer<T> configurer)
        {
            return
                bindingSyntax.OnActivation(
                                           t =>
                                               configurer.Configure(t, bindingSyntax.BindingConfiguration.Metadata.Name));
        }

        public static IBindingOnSyntax<T> ConfigureUsing<T>(this IBindingOnSyntax<T> bindingSyntax,
                                                            Func<IContext, IConfigurer<T>> configurerFactory)
        {
            return
                bindingSyntax.OnActivation(
                                           (c, t) =>
                                               configurerFactory(c)
                                               .Configure(t, bindingSyntax.BindingConfiguration.Metadata.Name));
        }
    }
}