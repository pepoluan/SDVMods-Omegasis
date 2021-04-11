using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Illuminate;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Items
{
    public class Dye : CustomObject
    {

        public NamedColor dyeColor;

        public Dye() { }

        public Dye(NamedColor Color)
        {

        }

        public override Item getOne()
        {
            Dye component = new Dye(this.dyeColor);
            return component;
        }
    }
}
