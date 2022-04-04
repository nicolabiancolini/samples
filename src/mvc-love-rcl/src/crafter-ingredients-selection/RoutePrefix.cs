// See the LICENSE.TXT file in the project root for full license information.

namespace Microsoft.AspNetCore.Routing
{
    public sealed class RoutePrefix
    {
        public static readonly RoutePrefix CustomerFacing = new ("/ingredients-selection");
        public static readonly RoutePrefix BackOffice = new ($"/back-office{CustomerFacing.Value}");

        public RoutePrefix(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public static implicit operator string(RoutePrefix prefix)
        {
            return prefix.Value;
        }
    }
}
