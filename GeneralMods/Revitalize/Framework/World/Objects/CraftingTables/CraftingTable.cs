using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.Crafting;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.CraftingTables
{
    public class CraftingTable : CustomObject
    {
        public string craftingBookName;


        public CraftingTable()
        {

        }

        public CraftingTable(BasicItemInformation Info,string CraftingRecipeBookName):base(Info)
        {
            this.craftingBookName = CraftingRecipeBookName;
        }

        public CraftingTable(BasicItemInformation Info,Vector2 TilePosition ,string CraftingRecipeBookName) : base(Info,TilePosition)
        {
            this.craftingBookName = CraftingRecipeBookName;
        }

        /// <summary>
        /// When the chair is right clicked ensure that all pieces associated with it are also rotated.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool rightClicked(Farmer who)
        {
            if (CraftingRecipeBook.CraftingRecipesByGroup.ContainsKey(this.craftingBookName))
            {
                CraftingRecipeBook.CraftingRecipesByGroup[this.craftingBookName].openCraftingMenu();
                return true;
            }
            else
            {
                return true;
            }
        }


        public override Item getOne()
        {
            CraftingTable component = new CraftingTable(this.getItemInformation().Copy(),this.craftingBookName);
            return component;
        }
    }
}
