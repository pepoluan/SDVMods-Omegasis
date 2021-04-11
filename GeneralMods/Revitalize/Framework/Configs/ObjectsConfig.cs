using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize;

namespace Omegasis.Revitalize.Framework.Configs
{
    public class ObjectsConfig
    {
        public bool showDyedColorName;
        public ObjectsConfig()
        {
            this.showDyedColorName = true;
        }

        public static ObjectsConfig InitializeConfig()
        {
            if (File.Exists(Path.Combine(ModCore.ModHelper.DirectoryPath, "Configs", "ObjectsConfig.json")))
                return ModCore.ModHelper.Data.ReadJsonFile<ObjectsConfig>(Path.Combine("Configs", "ObjectsConfig.json"));
            else
            {
                ObjectsConfig Config = new ObjectsConfig();
                ModCore.ModHelper.Data.WriteJsonFile(Path.Combine("Configs", "ObjectsConfig.json"), Config);
                return Config;
            }
        }

    }
}
