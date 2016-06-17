using Mantle.Extensions;
using Newtonsoft.Json;
using Ninject.Syntax;

namespace Mantle.Ninject
{
    public static class IBindingToSyntaxExtensions
    {
        public static IBindingWhenInNamedWithOrOnSyntax<T> ToJson<T>(this IBindingToSyntax<T> bindingToSyntax,
                                                                     string jsonSource)
        {
            bindingToSyntax.Require(nameof(bindingToSyntax));
            jsonSource.Require(nameof(jsonSource));

            return bindingToSyntax.ToMethod(c => JsonConvert.DeserializeObject<T>(jsonSource));
        }
    }
}