using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Illuminate;

namespace Omegasis.Revitalize.Framework.World.Objects.Interfaces
{
    public interface ILightManagerProvider
    {

        LightManager GetLightManager();
    }
}
