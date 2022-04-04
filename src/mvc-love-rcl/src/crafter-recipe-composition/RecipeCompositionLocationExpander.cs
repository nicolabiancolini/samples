// See the LICENSE.TXT file in the project root for full license information.

namespace Microsoft.AspNetCore.Mvc.Razor
{
    internal class RecipeCompositionLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            return !(context.AreaName ?? string.Empty).Equals(BoundedContextAreaAttribute.Prefix)
                ? this.ExpandViewLocationsCore(viewLocations, value)
                : return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values[BoundedContextAreaAttribute.Prefix] = context.ActionContext.RouteData.Values[BoundedContextAreaAttribute.Prefix]?.ToString();
        }

        private IEnumerable<string> ExpandViewLocationsCore(IEnumerable<string> viewLocations, string value)
        {
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{0}", value + "/{0}");
                yield return location;
            }
        }
    }
}
