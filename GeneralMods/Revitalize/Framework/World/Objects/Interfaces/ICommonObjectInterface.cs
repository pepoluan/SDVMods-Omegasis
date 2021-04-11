using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.World.Objects.Interfaces
{
    public interface ICommonObjectInterface
    {
        public string Name { get; set; }
        public int Stack { get; set; }

        public int sellToStorePrice(long specificPlayerID = -1);
    }
}
