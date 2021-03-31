using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Energy
{
    public interface IEnergyInterface
    {
        ref EnergyManager getEnergyManager();
        void setEnergyManager(ref EnergyManager Manager);
    }
}
