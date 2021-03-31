using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Revitalize.Framework.Objects
{
    public class CustomItem : StardewValley.Item
    {
        public override string DisplayName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int Stack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int addToStack(Item stack)
        {
            throw new NotImplementedException();
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            throw new NotImplementedException();
        }

        public override string getDescription()
        {
            throw new NotImplementedException();
        }

        public override Item getOne()
        {
            throw new NotImplementedException();
        }

        public override bool isPlaceable()
        {
            throw new NotImplementedException();
        }

        public override int maximumStackSize()
        {
            throw new NotImplementedException();
        }
    }
}
