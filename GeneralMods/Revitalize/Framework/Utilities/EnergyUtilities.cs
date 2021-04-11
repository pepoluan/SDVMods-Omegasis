using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omegasis.Revitalize.Framework.Utilities
{
    public class EnergyUtilities
    {

        public static Color GetEnergyRemainingColor(Omegasis.Revitalize.Framework.Energy.EnergyManager Energy)
        {
            Color col = new Color();
            //ModCore.log("Energy is: " + this.energy.energyPercentRemaining);
            if (Energy.energyPercentRemaining > .75d)
            {
                col = Color.Green;
            }
            else if (Energy.energyPercentRemaining > .5d && Energy.energyPercentRemaining <= .75d)
            {
                col = Color.GreenYellow;
            }
            else if (Energy.energyPercentRemaining > .25d && Energy.energyPercentRemaining <= .5d)
            {
                col = Color.Yellow;
            }
            else if (Energy.energyPercentRemaining > .10d && Energy.energyPercentRemaining <= .25d)
            {
                col = Color.Orange;
            }
            else
            {
                col = Color.Red;
            }
            return col;
        }


    }
}
