using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Revitalize.Framework.Objects
{
    public interface IObjectManager
    {

        Item getItem(string Id, int StackSize = 1);

    }
}
