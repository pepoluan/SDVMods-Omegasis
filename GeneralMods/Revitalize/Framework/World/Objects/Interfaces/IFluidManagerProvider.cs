using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Managers;

namespace Omegasis.Revitalize.Framework.World.Objects.Interfaces
{
    public interface IFluidManagerProvider
    {
        public ref FluidManagerV2 GetFluidManager();
        public void SetFluidManager(FluidManagerV2 Manager);

        public List<IFluidManagerProvider> GetNeighboringFluidManagers();
    }
}
