// See the LICENSE.TXT file in the project root for full license information.

namespace Microsoft.AspNetCore.Mvc
{
    public class BoundedContextAreaAttribute : AreaAttribute
    {
        public static readonly string Prefix = "IngredientsSelection";

        public BoundedContextAreaAttribute(string areaName)
            : base($"{BoundedContextAreaAttribute.Prefix}/{areaName}")
        {
        }
    }
}
