using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize;
using Revitalize.Framework.Objects;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Utilities
{
    /// <summary>
    /// Deals with syncing objects in multiplayer.
    /// </summary>
    public static class MultiplayerUtilities
    {

        public static Multiplayer GameMultiplayer;

        /// <summary>
        /// Handles receiving mod messages.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public static void GetModMessage(object o, StardewModdingAPI.Events.ModMessageReceivedEventArgs e)
        {
        }


        public static Multiplayer GetMultiplayer()
        {
            if (GameMultiplayer == null)
            {
                Multiplayer multiplayer = ModCore.ModHelper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer", true).GetValue();
                if (multiplayer == null) return null;
                else
                {
                    GameMultiplayer = multiplayer;
                    return GameMultiplayer;
                }
            }
            else
                return GameMultiplayer;
        }

    }
}
