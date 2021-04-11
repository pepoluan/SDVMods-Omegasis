using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omegasis.Revitalize.Framework.Illuminate
{
    public class NamedColor
    {
        public string name;
        public Color color;

        public NamedColor()
        {

        }

        public NamedColor(string Name, Color Color)
        {
            this.name = Name;
            this.color = Color;
        }

        public NamedColor(Color Color)
        {
            this.name = "";
            this.color = Color;
        }

        public NamedColor(string Name, int r, int g, int b, int a = 255, bool Invert = false)
        {
            this.name = Name;
            this.color = new Color(r, g, b, a);
            if (Invert)
                this.color = this.color.Invert();
        }

        public Color getColor()
        {
            return this.color;
        }

        public Color getInvertedColor()
        {
            return this.color.Invert();
        }
    }
}
