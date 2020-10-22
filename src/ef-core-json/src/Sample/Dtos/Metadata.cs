// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Drawing;

namespace Sample.Dtos
{
    public class Metadata
    {
        public Metadata(Retailer retailer, Color color, double weight)
        {
            this.Retailer = retailer;
            this.Weight = weight;
            this.Color = color.Name;
        }

        private Metadata()
        {
        }

        public string Color { get; }

        public double Weight { get; }

        public double Width { get; }

        public double Length { get; }

        public double Height { get; }

        public Retailer Retailer { get; }
    }
}
