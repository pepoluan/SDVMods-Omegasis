using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using PyTK.CustomElementHandler;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.Objects.InformationFiles.Furniture;

namespace Revitalize.Framework.Objects.Furniture
{
    public class FurnitureTileComponent:MultiTiledComponent
    {

        [JsonIgnore]
        public int framesUntilNextRotation = 0;

        public FurnitureTileComponent():base()
        {

        }

        public FurnitureTileComponent(CustomObjectData PyTKData,BasicItemInformation Info):base(PyTKData,Info)
        {
            this.Price = Info.price;
        }

        public FurnitureTileComponent(CustomObjectData PyTKData, BasicItemInformation Info,Vector2 TileLocation) : base(PyTKData,Info,TileLocation)
        {
            this.Price = Info.price;
        }

    }
}
