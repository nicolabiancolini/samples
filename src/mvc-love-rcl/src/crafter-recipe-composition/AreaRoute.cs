// See the LICENSE.TXT file in the project root for full license information.

namespace Microsoft.AspNetCore.Mvc
{
    internal sealed class AreaRoute
    {
        public static readonly AreaRoute CustomerFacing = new ("RecipeComposition/CustomerFacing", "/recipe-composition");
        public static readonly AreaRoute BackOffice = new ("RecipeComposition/BackOffice", $"/back-office{CustomerFacing.UrlPrefix}");

        public AreaRoute(string name, string urlPrefix)
        {
            this.Name = name;
            this.AreaName = name;
            this.UrlPrefix = urlPrefix;
        }

        public string Name { get; }

        public string AreaName { get; }

        public string UrlPrefix { get; }
    }
}
