using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Revitalize.Framework.Objects
{
    [XmlType("Omegasis.Revitalize.Framework.Objects.CustomObject")]
    public class CustomObject:StardewValley.Object
    {

        //TODO: Fill out with animation manager and custom colors and fields and stuff.
        //Might not need to make a custom furniture class if this goes well with editing the bounding box.
        public CustomObject()
        {
        }

        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            return base.performObjectDropInAction(dropInItem, probe, who);
        }

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            base.performRemoveAction(tileLocation, environment);
        }

        public override bool performToolAction(Tool t, GameLocation location)
        {
            return base.performToolAction(t, location);
        }

        public override bool performUseAction(GameLocation location)
        {
            return base.performUseAction(location);
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            base.draw(spriteBatch, x, y, alpha);
        }

        public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
        {
            base.draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
        }

        public override void drawAsProp(SpriteBatch b)
        {
            base.drawAsProp(b);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
        }

        public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
        {
            base.drawPlacementBounds(spriteBatch, location);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        public override bool performDropDownAction(Farmer who)
        {
            return base.performDropDownAction(who);
        }
    }
}
