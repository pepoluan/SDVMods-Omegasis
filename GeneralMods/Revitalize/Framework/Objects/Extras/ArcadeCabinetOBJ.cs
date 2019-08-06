using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PyTK.CustomElementHandler;
using StardewValley;

namespace Revitalize.Framework.Objects.Extras
{
    public class ArcadeCabinetOBJ:MultiTiledObject
    {
        public ArcadeCabinetOBJ() : base()
        {

        }

        public ArcadeCabinetOBJ(CustomObjectData PyTKData, BasicItemInformation Info) : base(PyTKData, Info)
        {

        }

        public ArcadeCabinetOBJ(CustomObjectData PyTKData, BasicItemInformation Info, Vector2 TilePosition) : base(PyTKData, Info, TilePosition)
        {

        }

        public ArcadeCabinetOBJ(CustomObjectData PyTKData, BasicItemInformation Info, Vector2 TilePosition, Dictionary<Vector2, MultiTiledComponent> Objects) : base(PyTKData, Info, TilePosition, Objects)
        {


        }

        public override void rotate()
        {
            base.rotate();
        }

        public override Item getOne()
        {
            Dictionary<Vector2, MultiTiledComponent> objs = new Dictionary<Vector2, MultiTiledComponent>();
            foreach (var pair in this.objects)
            {
                objs.Add(pair.Key, (MultiTiledComponent)pair.Value.getOne());
            }


            return new ArcadeCabinetOBJ(this.data, this.info, this.TileLocation, objs);
        }


        public override ICustomObject recreate(Dictionary<string, string> additionalSaveData, object replacement)
        {
            ArcadeCabinetOBJ obj = (ArcadeCabinetOBJ)Revitalize.ModCore.Serializer.DeserializeGUID<ArcadeCabinetOBJ>(additionalSaveData["GUID"]);
            if (obj == null)
            {
                return null;
            }

            Dictionary<Vector2, Guid> guids = new Dictionary<Vector2, Guid>();

            foreach (KeyValuePair<Vector2, Guid> pair in obj.childrenGuids)
            {
                guids.Add(pair.Key, pair.Value);
            }

            foreach (KeyValuePair<Vector2, Guid> pair in guids)
            {
                obj.childrenGuids.Remove(pair.Key);
                //Revitalize.ModCore.log("DESERIALIZE: " + pair.Value.ToString());
                ArcadeCabinetTile component = Revitalize.ModCore.Serializer.DeserializeGUID<ArcadeCabinetTile>(pair.Value.ToString());
                component.InitNetFields();

                obj.addComponent(pair.Key, component);


            }
            obj.InitNetFields();

            if (!Revitalize.ModCore.ObjectGroups.ContainsKey(additionalSaveData["GUID"]))
            {
                Revitalize.ModCore.ObjectGroups.Add(additionalSaveData["GUID"], obj);
                return obj;
            }
            else
            {
                return Revitalize.ModCore.ObjectGroups[additionalSaveData["GUID"]];
            }


        }


        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            return base.canBePlacedHere(l, tile);
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
            {
                if (!this.isPlaceable())
                    return;
                int x = Game1.getOldMouseX() + Game1.viewport.X + (int)((pair.Value as MultiTiledComponent).offsetKey.X * Game1.tileSize);
                int y = Game1.getOldMouseY() + Game1.viewport.Y + (int)((pair.Value as MultiTiledComponent).offsetKey.Y * Game1.tileSize);
                if ((double)Game1.mouseCursorTransparency == 0.0)
                {
                    x = ((int)Game1.player.GetGrabTile().X + (int)((pair.Value as MultiTiledComponent).offsetKey.X)) * 64;
                    y = ((int)Game1.player.GetGrabTile().Y + (int)((pair.Value as MultiTiledComponent).offsetKey.Y)) * 64;
                }
                if (Game1.player.GetGrabTile().Equals(Game1.player.getTileLocation()) && (double)Game1.mouseCursorTransparency == 0.0)
                {
                    Vector2 translatedVector2 = Utility.getTranslatedVector2(Game1.player.GetGrabTile(), Game1.player.FacingDirection, 1f);
                    translatedVector2 += (pair.Value as MultiTiledComponent).offsetKey;
                    x = (int)translatedVector2.X * 64;
                    y = (int)translatedVector2.Y * 64;
                }
                bool flag = (pair.Value as MultiTiledComponent).canBePlacedHere(location, new Vector2(x / Game1.tileSize, y / Game1.tileSize));
                spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(x / 64 * 64 - Game1.viewport.X), (float)(y / 64 * 64 - Game1.viewport.Y)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(flag ? 194 : 210, 388, 16, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.01f);


                (pair.Value as MultiTiledComponent).draw(spriteBatch, x / Game1.tileSize, y / Game1.tileSize, 0.5f);
                //break;
                //this.draw(spriteBatch, x / 64, y / 64, 0.5f);
            }
        }

        public override void pickUp()
        {

            bool canPickUp = this.removeAndAddToPlayersInventory();
            if (canPickUp)
            {
                foreach (KeyValuePair<Vector2, StardewValley.Object> pair in this.objects)
                {
                    (pair.Value as ArcadeCabinetTile).removeFromLocation((pair.Value as ArcadeCabinetTile).location, pair.Key);
                }
                this.location = null;
            }

        }
    }
}
