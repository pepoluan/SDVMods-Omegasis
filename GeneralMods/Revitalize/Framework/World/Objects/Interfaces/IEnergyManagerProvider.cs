using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Energy;

namespace Omegasis.Revitalize.Framework.World.Objects.Interfaces
{
    public interface IEnergyManagerProvider
    {
        public ref EnergyManager GetEnergyManager();
        public void SetEnergyManager(EnergyManager Manager);

        public List<IEnergyManagerProvider> getAppropriateEnergyNeighbors();
    }
}
