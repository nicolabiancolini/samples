// See the LICENSE.TXT file in the project root for full license information.

namespace Microsoft.AspNetCore.Mvc.Razor
{
    internal class RecipeCompositionLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            return (context.AreaName ?? string.Empty).StartsWith(BoundedContextAreaAttribute.Prefix)
                ? this.ExpandViewLocationsCore(viewLocations, context.AreaName!.Replace(BoundedContextAreaAttribute.Prefix, string.Empty))
                : viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values[BoundedContextAreaAttribute.Prefix] = context.ActionContext.RouteData.Values[BoundedContextAreaAttribute.Prefix]?.ToString();
        }

        private IEnumerable<string> ExpandViewLocationsCore(IEnumerable<string> viewLocations, string boundedContextInnerArea)
        {
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{2}", boundedContextInnerArea);
                yield return location;
            }
        }
    }
}
