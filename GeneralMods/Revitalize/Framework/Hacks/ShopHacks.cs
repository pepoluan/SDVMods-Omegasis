using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Menus;

namespace Revitalize.Framework.Hacks
{
    /// <summary>
    /// Deals with modifications for SDV shops.
    /// </summary>
    public class ShopHacks
    {

        public static void AddInCustomItemsToShops()
        {
            AddItemsToRobinsShop();
            AddItemsToClintsShop();
        }


        /// <summary>
        /// Adds a specific amount of an item to a shop.
        /// </summary>
        /// <param name="I"></param>
        /// <param name="Price"></param>
        /// <param name="AmountAvailable">How much is in stock. -1 should be infinite.</param>
        public static void AddItemToShop(Item I, int Price, int AmountAvailable =-1)
        {
            if (Game1.activeClickableMenu is StardewValley.Menus.ShopMenu)
            {
                ShopMenu shopMenu = (Game1.activeClickableMenu as ShopMenu);

                shopMenu.forSale.Add(I);
                shopMenu.itemPriceAndStock.Add(I, new int[2] { Price, AmountAvailable });
            }
            else
            {
                throw new Exception("Game1 Activeclickable menu is not a shop menu!");
            }
        }


        public static void AddItemsToRobinsShop()
        {
            if(Game1.activeClickableMenu is StardewValley.Menus.ShopMenu)
            {
                ShopMenu shopMenu = (Game1.activeClickableMenu as ShopMenu);
                if (shopMenu.portraitPerson != null)
                {
                    if (shopMenu.portraitPerson.Name.Equals("Robin"))
                    {
                        AddItemToShop(ModCore.ObjectManager.getItem("Workbench", 1), 500, 1);
                        AddItemToShop(new StardewValley.Object((int)Enums.SDVObject.Clay, 1), 50,-1);
                        AddItemToShop(ModCore.ObjectManager.getItem("Sand", 1), 500, 1);
                    }
                }
            }
        }
        /// <summary>
        /// Adds in ore to clint's shop.
        /// </summary>
        private static void AddItemsToClintsShop()
        {
            if (Game1.activeClickableMenu is StardewValley.Menus.ShopMenu)
            {
                ShopMenu shopMenu = (Game1.activeClickableMenu as ShopMenu);
                if (shopMenu.portraitPerson != null)
                {
                    if (shopMenu.portraitPerson.Name.Equals("Robin"))
                    {
                        AddItemToShop(ModCore.ObjectManager.getItem("Tin", 1), ModCore.Configs.shops_blacksmithConfig.tinOreSellPrice);
                        AddItemToShop(ModCore.ObjectManager.getItem("Bauxite", 1), ModCore.Configs.shops_blacksmithConfig.bauxiteOreSellPrice, -1);
                        AddItemToShop(ModCore.ObjectManager.getItem("Lead", 1), ModCore.Configs.shops_blacksmithConfig.leadOreSellPrice, 1);
                        AddItemToShop(ModCore.ObjectManager.getItem("Silver", 1), ModCore.Configs.shops_blacksmithConfig.silverOreSellPrice, 1);
                        AddItemToShop(ModCore.ObjectManager.getItem("Titanium", 1), ModCore.Configs.shops_blacksmithConfig.titaniumOreSellPrice, 1);

                    }
                }
            }
        }
    }
}
