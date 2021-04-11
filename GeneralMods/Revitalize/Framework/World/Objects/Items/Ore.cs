using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.Objects;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Items
{
    public class Ore:CustomObject
    {
        public Ore()
        {

        }

        public Ore(BasicItemInformation BasicItemInfo): base(BasicItemInfo)
        {


        }

        public Ore(BasicItemInformation BasicItemInfo, int StackSize) : base(BasicItemInfo, StackSize)
        {


        }

        public Ore(BasicItemInformation BasicItemInfo, Vector2 TilePosition) : base(BasicItemInfo,TilePosition)
        {


        }

        public Ore(BasicItemInformation BasicItemInfo, Vector2 TilePosition, int StackSize=1) : base(BasicItemInfo,TilePosition,StackSize)
        {


        }

        public override Item getOne()
        {
            Ore component = new Ore(this.basicItemInfo.Copy(),Vector2.Zero,1);
            return component;
        }

    }
}
