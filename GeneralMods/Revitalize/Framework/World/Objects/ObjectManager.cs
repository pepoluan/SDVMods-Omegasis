using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.Revitalize.Framework.Objects;
using Omegasis.Revitalize.Framework.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.Objects.CraftingTables;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.Revitalize.Framework.World.Objects.Machines;
using Omegasis.Revitalize.Framework.World.Objects.Machines.EnergyGeneration;
using Revitalize.Framework.Managers;
using Revitalize.Framework.Objects.Items.Tools;
using Revitalize.Framework.Objects.Machines;
using Revitalize.Framework.Utilities;
using StardewModdingAPI;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;

namespace Omegasis.Revitalize.Framework.Objects
{
    /// <summary>
    /// Deals with handling all objects for the mod.
    /// </summary>
    public class ObjectManager
    {
        /// <summary>
        /// All of the object managers id'd by a mod's or content pack's unique id.
        /// </summary>
        public static Dictionary<string, ObjectManager> ObjectPools;


        /// <summary>
        /// The name of this object manager.
        /// </summary>
        public string name;

        public ResourceManager resources;

        public Dictionary<string, CustomObject> ItemsByName;

        public Dictionary<string, StardewValley.Tool> Tools;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ObjectManager()
        {
            this.initialize();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manifest"></param>
        public ObjectManager(IManifest manifest)
        {
            this.name = manifest.UniqueID;
            this.initialize();
        }

        /// <summary>
        /// Initialize all objects used to manage this class.
        /// </summary>
        private void initialize()
        {

            this.resources = new ResourceManager();
            this.ItemsByName = new Dictionary<string, CustomObject>();

            this.Tools = new Dictionary<string, Tool>();

            //Load in furniture again!
        }

        /// <summary>
        /// Loads in the items for the object and resource managers.
        /// </summary>
        public void loadInItems()
        {
            this.resources.loadInItems(); //Must be first.
            this.loadInCraftingTables();
            this.loadInMachines();
            this.loadInTools();
            this.loadInAestheticsObjects();
        }

        private void loadInAestheticsObjects()
        {
            /*
            LampMultiTiledObject lighthouse = new LampMultiTiledObject(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(MultiTiledObject), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(), Color.White, false, null, null));
            LampTileComponent lighthouse_0_0 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(0, 0, 16, 16)), Color.White, true, null, new Illuminate.LightManager(),null,true));
            LampTileComponent lighthouse_1_0 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(16, 0, 16, 16)), Color.White, true, null, new Illuminate.LightManager(), null, true));
            LampTileComponent lighthouse_0_1 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(0, 16, 16, 16)), Color.White, true, null, new Illuminate.LightManager(), null, true));
            LampTileComponent lighthouse_1_1 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(16, 16, 16, 16)), Color.White, true, null, new Illuminate.LightManager(), null, true));
            LampTileComponent lighthouse_0_2 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(0, 32, 16, 16)), Color.White, true, null, new Illuminate.LightManager(), null, false));
            LampTileComponent lighthouse_1_2 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(16, 32, 16, 16)), Color.White, true, null, new Illuminate.LightManager(), null, false));
            LampTileComponent lighthouse_0_3 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(0, 48, 16, 16)), Color.White, false, null, new Illuminate.LightManager()));
            LampTileComponent lighthouse_1_3 = new LampTileComponent(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), typeof(LampTileComponent), Color.White, true), new BasicItemInformation("LightHouse", "Omegasis.Revitalize.Objects.Furniture.Misc.Lighthouse", "A minuture lighthouse that provides a decent amount of light.", "Furniture", Color.Brown, -300, 0, false, 2500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "Lighthouse"), new Animation(16, 48, 16, 16)), Color.White, false, null, new Illuminate.LightManager()));
            lighthouse_0_0.lightManager.addLight(new Vector2(16, 16), LightManager.CreateLightSource(10f, Color.White), lighthouse_0_0);
            lighthouse.addComponent(new Vector2(0,-3),lighthouse_0_0);
            lighthouse.addComponent(new Vector2(1, -3), lighthouse_1_0);
            lighthouse.addComponent(new Vector2(0, -2), lighthouse_0_1);
            lighthouse.addComponent(new Vector2(1, -2), lighthouse_1_1);
            lighthouse.addComponent(new Vector2(0, -1), lighthouse_0_2);
            lighthouse.addComponent(new Vector2(1, -1), lighthouse_1_2);
            lighthouse.addComponent(new Vector2(0, 0), lighthouse_0_3);
            lighthouse.addComponent(new Vector2(1, 0), lighthouse_1_3);

            this.AddItem("Lighthouse", lighthouse);
            */
        }

        private void loadInCraftingTables()
        {
            CraftingTable WorkbenchObj = new CraftingTable( new BasicItemInformation("Workbench", "Omegasis.Revitalize.Objects.Crafting.Workbench", "A workbench that can be used for crafting different objects.", "Crafting", Color.Brown, -300,-300 ,0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Workbench"), new AnimationManager(), Color.White, false, null, null),"Workbench");
            CraftingTable AnvilObj = new CraftingTable(new BasicItemInformation("Anvil", "Omegasis.Revitalize.Objects.Crafting.Anvil", "An anvil that can be used for crafting different machines and other metalic objects.", "Crafting", Color.Brown, -300, -300,0, false, 2000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Objects.Crafting", "Anvil"), new AnimationManager(), Color.White, false, null, null),"Anvil");

            this.AddItem("Workbench", WorkbenchObj);
            this.AddItem("Anvil", AnvilObj);
        }

        private void loadInMachines()
        {
            this.loadInConnectionComponents();

           
            TrashCan trashCan = new TrashCan(new BasicItemInformation("Trash Can", "Omegasis.Revitalize.Furniture.Misc.TrashCan", "A trash can where you can throw away unnecessary objects. It empties out at the beginning of each new day.", "Machine", Color.SteelBlue, -300, -300,0, false, 650, true, true, TextureManager.GetTexture(ModCore.Manifest, "Furniture", "TrashCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Furniture", "TrashCan"), new Animation(0, 0, 16, 16)), Color.White, true, new InventoryManager(36), null, null));
            this.AddItem("TrashCan", trashCan);
    
            Machine sandBox = new Machine(new BasicItemInformation("Sandbox", "Omegasis.Revitalize.Objects.Machines.Sandbox", "A sandbox which slowly produces sand. Unfortunately you can't sit in this one.", "Machine", Color.SteelBlue, -300,-300, 0, false, 750, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Sandbox"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Sandbox"),new Animation(0,0,16,16)), Color.White, false, new InventoryManager(36), null, null), new List<InformationFiles.ResourceInformation>()
            {
                new ResourceInformation(this.resources.getResource("Sand",1),1,1,1,1,1,1,0,0,0,0)

            }, 0, TimeUtilities.GetMinutesFromTime(0, 1, 0),"Workbench");
            this.AddItem("SandBox", sandBox);


           
            SolarPanel solarP1 = new SolarPanel( new BasicItemInformation("Solar Panel", "Omegasis.Revitalize.Objects.Machines.SolarPanelV1", "Generates energy while the sun is up.", "Machine", Color.SteelBlue, -300,-300, 0, false, 1000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "SolarPanelTier1"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "SolarPanelTier1"), new Animation(0, 0, 16, 16)), Color.White, false, null, null, new Energy.EnergyManager(100, Enums.EnergyInteractionType.Produces)),2,0);
            SolarPanel solarA1V1 = new SolarPanel(new BasicItemInformation("Solar Array", "Omegasis.Revitalize.Objects.Machines.SolarArrayV1", "A collection of solar panels that generates even more energy while the sun is up.", "Machine", Color.SteelBlue, -300,-300 ,0, false, 1000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "SolarArrayTier1"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "SolarArrayTier1"), new Animation(0, 0, 16, 16)), Color.White, false, null, null, new Energy.EnergyManager(100, Enums.EnergyInteractionType.Produces)), 8, 0);

            this.AddItem("SolarPanelTier1", solarP1);
            this.AddItem("SolarArrayTier1", solarA1V1);


            Machine batteryBin_0_0 = new Machine(new BasicItemInformation("Battery Bin", "Omegasis.Revitalize.Objects.Machines.BatteryBin", "Consumes energy over time to produce battery packs.", "Machine", Color.SteelBlue, -300,-300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "BatteryBin"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "BatteryBin"), new Animation(0, 0, 16, 16)), Color.White, false, new InventoryManager(9,3,3), null, new Energy.EnergyManager(500, Enums.EnergyInteractionType.Consumes)), new List<InformationFiles.ResourceInformation>()
            {
                new ResourceInformation(new StardewValley.Object((int)Enums.SDVObject.BatteryPack,1),1,1,1,1,1,1,0,0,0,0)

            }, 1, TimeUtilities.GetMinutesFromTime(0, 1, 0), "");
            this.AddItem("BatteryBin", batteryBin_0_0);

            
            Machine capacitor_0_0 = new Machine(new BasicItemInformation("Capacitor", "Omegasis.Revitalize.Objects.Machines.Capacitor", "A box which stores energy for use over time.", "Machine", Color.SteelBlue, -300,-300 ,0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Capacitor"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Capacitor"), new Animation(0, 0, 16, 16)), Color.White, false, null, null, new Energy.EnergyManager(2000, Enums.EnergyInteractionType.Storage)),null,0,0,"");
            this.AddItem("Capacitor", capacitor_0_0);


          
            ChargingStation chargingStation_0_0 = new ChargingStation( new BasicItemInformation("Charging Station", "Omegasis.Revitalize.Objects.Machines.ChargingStation", "A place to charge your tools and other electrical components.", "Machine", Color.SteelBlue, -300,-300 ,0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "ChargingStation"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "ChargingStation"), new Animation(0, 0, 16, 16)), Color.White, true, null, null, new Energy.EnergyManager(2000, Enums.EnergyInteractionType.Storage)), null, 0, 0, "");
            this.AddItem("ChargingStation", chargingStation_0_0);

            Grinder grinder_0_0 = new Grinder( new BasicItemInformation("Grinder", "Omegasis.Revitalize.Objects.Machines.Grinder", "Grinds up ores and rocks.", "Machine", Color.SteelBlue, -300,-300, 0, false, 4000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Grinder"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Grinder"), new Animation(0, 0, 16, 16)), Color.White, false, new InventoryManager(18, 3, 6), null, new Energy.EnergyManager(1000, Enums.EnergyInteractionType.Consumes)),null,ModCore.Configs.machinesConfig.grinderEnergyConsumption,ModCore.Configs.machinesConfig.grinderTimeToGrind,"");
            this.AddItem("Grinder", grinder_0_0);

           
            Machine miningDrillMachine_0_0 = new Machine(new BasicItemInformation("Mining Drill", "Omegasis.Revitalize.Objects.Machines.MiningDrill", "Digs up rocks and ores. Requires energy to run.", "Machine", Color.SteelBlue, -300,-300 ,0, false, 4000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "MiningDrillMachine"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "MiningDrillMachine"), new Animation(0, 0, 16, 16), new Dictionary<string, List<Animation>>() {
                {"Default",new List<Animation>(){new Animation(0,0,16,16) } },
                { "Mining",new List<Animation>(){
                    new Animation(0,0,16,32,30),
                    new Animation(16,0,16,32,30),
                    new Animation(32,0,16,32,30),
                    new Animation(48,0,16,32,30),
                } }
            }, "Mining"), Color.White, false, new InventoryManager(18, 3, 6), null, new Energy.EnergyManager(1000, Enums.EnergyInteractionType.Consumes)), ModCore.ObjectManager.resources.miningDrillResources.Values.ToList(), ModCore.Configs.machinesConfig.miningDrillEnergyConsumption, ModCore.Configs.machinesConfig.miningDrillTimeToMine, "");

            this.AddItem("MiningDrillMachineV1", miningDrillMachine_0_0);

           
            AlloyFurnace alloyFurnace_0_0 = new AlloyFurnace(new BasicItemInformation("Alloy Furnace", "Omegasis.Revitalize.Objects.Machines.AlloyFurnace", "Smelts bars into ingots. Works twice as fast as a traditional furnace.", "Machine", Color.SteelBlue, -300,-300, 0, false, 250, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "AlloyFurnace"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "AlloyFurnace"), new Animation(0, 0, 16, 16), new Dictionary<string, List<Animation>>()
            {
                {"Default",new List<Animation>()
                    {
                        new Animation(0,0,16,16)
                    }
                },
                {"Working",new List<Animation>()
                    {
                    new Animation(0,32,16,32,30),
                    new Animation(16,32,16,32,30)
                    }
                }

            },"Default"), Color.White, true, new InventoryManager(6, 3, 6), null, null), null, 0, 0, "AlloyFurnace");
            this.AddItem("AlloyFurnace", alloyFurnace_0_0);

            WaterPump waterPumpV1_0_0 = new WaterPump(new BasicItemInformation("Water Pump", "Omegasis.Revitalize.Objects.Machines.WaterPump", "Pumps up water from a water source.", "Machine", Color.SteelBlue, -300,-300 ,0, false, 350, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "WaterPump"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "WaterPump"), new Animation(0, 0, 16, 16)), Color.White, true, null, null, null, false, null, null, new FluidManagerV2(5000, true, Enums.FluidInteractionType.Machine, false)), null, 0, 0, "");
            this.AddItem("WaterPumpV1", waterPumpV1_0_0);

            
            SteamBoiler steamBoilerV1_0_0 = new SteamBoiler( new BasicItemInformation("Steam Boiler", "Omegasis.Revitalize.Objects.Machines.SteamBoiler", "Burns coal and wood. Consumes water to produce steam which can be used in a steam generator.", "Machine", Color.SteelBlue, -300,-300, 0, false, 1000, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "SteamBoiler"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "SteamBoiler"), new Animation(0, 0, 16, 16),new Dictionary<string, List<Animation>>()
            {
                {"Default",new List<Animation>(){
                    new Animation(0,0,32,48)
                } },
                {"Working",new List<Animation>(){
                    new Animation(32,0,32,48)
                } },

            },"Default"), Color.White, true, new InventoryManager(9, 3, 3), null, null, false, null, null, new FluidManagerV2(4000, false, Enums.FluidInteractionType.Machine, false, false, 1)), null, 0, 0, "");
            this.AddItem("SteamBoilerV1", steamBoilerV1_0_0);

            SteamEngine steamEngineV1_0_0 = new SteamEngine(new BasicItemInformation("Steam Engine", "Omegasis.Revitalize.Objects.Machines.SteamEngine", "Consumes steam in order to produce power.", "Machine", Color.SteelBlue, -300,-300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "SteamEngine"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "SteamEngine"), new Animation(0, 0, 16, 16)), Color.White, false, new InventoryManager(9, 3, 3), null, new Energy.EnergyManager(500, Enums.EnergyInteractionType.Produces), false, null, null, new FluidManagerV2(2000, false, Enums.FluidInteractionType.Machine, false, true, 1)), null, ModCore.Configs.machinesConfig.steamEngineV1_powerGeneratedPerOperation, 0, "", ModCore.ObjectManager.resources.getFluid("Steam"), ModCore.Configs.machinesConfig.steamEngineV1_requiredSteamPerOperation);
            this.AddItem("SteamEngineV1", steamEngineV1_0_0);


           
            Windmill windMillV1_0_0 = new Windmill( new BasicItemInformation("Windmill", "Omegasis.Revitalize.Objects.Machines.WindmillV1", "Generates power from the wind.", "Machine", Color.SteelBlue, -300, -300,0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Windmill"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Windmill"), new Animation(0, 0, 16, 16),new Dictionary<string, List<Animation>>() {

                {"Default",new List<Animation>()
                {
                    new Animation(0,0,16,32)
                } },
                {"Working",new List<Animation>()
                {
                    new Animation(0,0,16,32,20),
                    new Animation(16,0,16,32,20)
                } }
            },"Working"), Color.White, false, null, null, new Energy.EnergyManager(500, Enums.EnergyInteractionType.Produces), false, null, null, null), null, ModCore.Configs.machinesConfig.windmillV1_basePowerProduction, 0, "", null, 0);

            this.AddItem("WindmillV1", windMillV1_0_0);


            Windmill windMillV2 = new Windmill(new BasicItemInformation("Windmill", "Omegasis.Revitalize.Objects.Machines.WindmillV2", "Generates power from the wind.", "Machine", Color.SteelBlue, -300, -300, 0, false, 500, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "Windmill"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "Windmill"), new Animation(0, 0, 16, 16), new Dictionary<string, List<Animation>>() {

                {"Default",new List<Animation>()
                {
                    new Animation(0,0,16,32)
                } },
                {"Working",new List<Animation>()
                {
                    new Animation(0,0,16,32,20),
                    new Animation(16,0,16,32,20)
                } }
            }, "Working"), Color.White, false, null, null, new Energy.EnergyManager(500, Enums.EnergyInteractionType.Produces), false, null, null, null), null, ModCore.Configs.machinesConfig.windmillV2_basePowerProduction, 0, "", null, 0);

            this.AddItem("WindmillV2", windMillV2);
        }

        private void loadInConnectionComponents()
        {
            Wire copperWire_0_0 = new Wire(new BasicItemInformation("Copper Wire", "Omegasis.Revitalize.Objects.Machines.Wire.CopperWire", "Wire made from copper bars. Transfers energy between sources.", "Machine", Color.SteelBlue, -300, -300,0, false, 15, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "CopperWire"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "CopperWire"), new Animation(0, 0, 16, 16)), Color.White, true, null, null, new Energy.EnergyManager(100, Enums.EnergyInteractionType.Transfers),false));
            this.AddItem("CopperWire", copperWire_0_0);  
            Pipe ironPipe_0_0 = new Pipe(new BasicItemInformation("Iron Pipe", "Omegasis.Revitalize.Objects.Machines.Wire.Pipe", "Pipes made from iron. Transfers fluids between machines.", "Machine", Color.SteelBlue, -300,-300, 0, false, 25, true, true, TextureManager.GetTexture(ModCore.Manifest, "Machines", "IronPipe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Machines", "IronPipe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null, null, false, null, null, new FluidManagerV2(0, false, Enums.FluidInteractionType.Transfers, false)));
            this.AddItem("IronPipe", ironPipe_0_0);
        }

        private void loadInTools()
        {
            PickaxeExtended bronzePick = new PickaxeExtended(new BasicItemInformation("Bronze Pickaxe", "Omegasis.Revitalize.Items.Tools.BronzePickaxe", "A sturdy pickaxe made from bronze.", "Tool", Color.SlateGray, 0, 0, 0,false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzePickaxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzePickaxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzePickaxeWorking"));
            PickaxeExtended steelPick = new PickaxeExtended(new BasicItemInformation("Hardened Pickaxe", "Omegasis.Revitalize.Items.Tools.HardenedPickaxe", "A sturdy pickaxe made from hardened alloy.", "Tool", Color.SlateGray, 0, 0,0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedPickaxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedPickaxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 3, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedPickaxeWorking"));
            PickaxeExtended titaniumPick = new PickaxeExtended(new BasicItemInformation("Titanium Pickaxe", "Omegasis.Revitalize.Items.Tools.TitaniumPickaxe", "A sturdy pickaxe made from titanium.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumPickaxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumPickaxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 4, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumPickaxeWorking"));

            AxeExtended bronzeAxe= new AxeExtended(new BasicItemInformation("Bronze Axe", "Omegasis.Revitalize.Items.Tools.BronzeAxe", "A sturdy axe made from bronze.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzeAxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeAxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeAxeWorking"));
            AxeExtended steelAxe = new AxeExtended(new BasicItemInformation("Hardened Axe", "Omegasis.Revitalize.Items.Tools.HardenedAxe", "A sturdy axe made from hardened alloy.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedAxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedAxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null),3,TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedAxeWorking"));
            AxeExtended titaniumAxe = new AxeExtended(new BasicItemInformation("Titanium Axe", "Omegasis.Revitalize.Items.Tools.TitaniumAxe", "A sturdy axe made from Titanium.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumAxe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumAxe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 4, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumAxeWorking"));

            HoeExtended bronzeHoe = new HoeExtended(new BasicItemInformation("Bronze Hoe", "Omegasis.Revitalize.Items.Tools.BronzeHoe", "A sturdy hoe made from bronze.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzeHoe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeHoe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeHoeWorking"));
            HoeExtended steelHoe = new HoeExtended(new BasicItemInformation("Hardened Hoe", "Omegasis.Revitalize.Items.Tools.HardenedHoe", "A sturdy hoe made from hardened alloy.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedHoe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedHoe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 3, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedHoeWorking"));
            HoeExtended titaniumHoe = new HoeExtended(new BasicItemInformation("Titanium Hoe", "Omegasis.Revitalize.Items.Tools.TitaniumHoe", "A sturdy hoe made from titanium.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumHoe"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumHoe"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 4, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumHoeWorking"));

            WateringCanExtended bronzeCan = new WateringCanExtended(new BasicItemInformation("Bronze Watering Can", "Omegasis.Revitalize.Items.Tools.BronzeWateringCan", "A sturdy watering can made from bronze.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "BronzeWateringCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeWateringCan"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 1, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "BronzeWateringCanWorking"),70);
            WateringCanExtended steelCan = new WateringCanExtended(new BasicItemInformation("Hardened Watering Can", "Omegasis.Revitalize.Items.Tools.HardenedWateringCan", "A sturdy watering can made from hardened alloy.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "HardenedWateringCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedWateringCan"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "HardenedWateringCanWorking"), 100);
            WateringCanExtended titaniumCan = new WateringCanExtended(new BasicItemInformation("Titanium Watering Can", "Omegasis.Revitalize.Items.Tools.TitaniumWateringCan", "A sturdy watering can made from titanium.", "Tool", Color.SlateGray, 0, 0, 0, false, 500, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "TitaniumWateringCan"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumWateringCan"), new Animation(0, 0, 16, 16)), Color.White, true, null, null), 3, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "TitaniumWateringCanWorking"), 125);

            Drill miningDrillV1 = new Drill(new BasicItemInformation("Simple Mining Drill", "Omegasis.Revitalize.Items.Tools.MiningDrillV1", "A drill used in mining. Consumes energy instead of stamina.", "Tool", Color.SlateGray, 0, 0, 0, false, 1000, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "MiningDrill"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "MiningDrill"), new Animation(0, 0, 16, 16)), Color.White, true, null, null,new Energy.EnergyManager(200, Enums.EnergyInteractionType.Consumes)), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "MiningDrillWorking"));
            Chainsaw chainsawV1 = new Chainsaw(new BasicItemInformation("Simple Chainsaw", "Omegasis.Revitalize.Items.Tools.ChainsawV1", "A chainsaw used to fell trees and chop wood. Consumes energy instead of stamina.", "Tool", Color.SlateGray, 0, 0, 0, false, 1000, false, false, TextureManager.GetTexture(ModCore.Manifest, "Tools", "Chainsaw"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "Chainsaw"), new Animation(0, 0, 16, 16)), Color.White, true, null, null, new Energy.EnergyManager(200, Enums.EnergyInteractionType.Consumes)), 2, TextureManager.GetExtendedTexture(ModCore.Manifest, "Tools", "ChainsawWorking"));

            this.Tools.Add("BronzePickaxe", bronzePick);
            this.Tools.Add("HardenedPickaxe", steelPick);
            this.Tools.Add("TitaniumPickaxe", titaniumPick);

            this.Tools.Add("BronzeAxe", bronzeAxe);
            this.Tools.Add("HardenedAxe", steelAxe);
            this.Tools.Add("TitaniumAxe", titaniumAxe);

            this.Tools.Add("BronzeHoe", bronzeHoe);
            this.Tools.Add("HardenedHoe", steelHoe);
            this.Tools.Add("TitaniumHoe", titaniumHoe);

            this.Tools.Add("BronzeWateringCan", bronzeCan);
            this.Tools.Add("HardenedWateringCan", steelCan);
            this.Tools.Add("TitaniumWateringCan", titaniumCan);

            this.Tools.Add("MiningDrillV1", miningDrillV1);
            this.Tools.Add("ChainsawV1", chainsawV1);
        }

        /// <summary>
        /// Gets a random object from the dictionary passed in.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Item getRandomObject(Dictionary<string,CustomObject> dictionary)
        {
            if (dictionary.Count == 0) return null;
            List<CustomObject> objs = new List<CustomObject>();
            foreach(KeyValuePair<string,CustomObject> pair in dictionary)
            {
                objs.Add(pair.Value);
            }
            int rand = Game1.random.Next(0,objs.Count);
            return objs[rand].getOne();
        }

        
        /// <summary>
        /// Gets an object from the dictionary that is passed in.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public Item getObject(string objectName, Dictionary<string,CustomObject> dictionary)
        {
            if (dictionary.ContainsKey(objectName))
            {
                return dictionary[objectName].getOne();
            }
            else
            {
                throw new Exception("Object pool doesn't contain said object.");
            }
        }

        /// <summary>
        /// Adds in an item to be tracked by the mod's object manager.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="I"></param>
        public void AddItem(string key, CustomObject I)
        {
            if (this.ItemsByName.ContainsKey(key))
            {
                throw new Exception("Item with the same key has already been added into the mod!");
            }
            else
            {
                this.ItemsByName.Add(key, I);
            }
        }

        /// <summary>
        /// Gets an item from the list of modded items.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Stack"></param>
        /// <returns></returns>
        public Item GetItem(string Key,int Stack=1)
        {
            if (this.ItemsByName.ContainsKey(Key))
            {
                Item I= this.ItemsByName[Key].getOne();
                I.Stack = Stack;
                return I;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a tool from the list of managed tools.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Item GetTool(string Name)
        {
            if (this.Tools.ContainsKey(Name)) return this.Tools[Name].getOne();
            else return null;
        }

        /// <summary>
        /// Adds a new object manager to the master pool of managers.
        /// </summary>
        /// <param name="Manifest"></param>
        public static void AddObjectManager(IManifest Manifest)
        {
            if (ObjectPools == null) ObjectPools = new Dictionary<string, ObjectManager>();
            ObjectPools.Add(Manifest.UniqueID, new ObjectManager(Manifest));
        }


        /// <summary>
        /// Cleans up all stored information.
        /// </summary>
        public void returnToTitleCleanUp()
        {

        }

        /// <summary>
        /// Scans all mod items to try to find a match.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public Item getItemByIDAndType(string ID,Type T)
        {

            foreach(var v in this.ItemsByName)
            {
                if (v.Value.GetType() == T && v.Value.basicItemInfo.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }

            foreach(var v in this.resources.ores)
            {
                if (v.Value.GetType() == T && v.Value.basicItemInfo.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }

            }
            foreach(var v in this.resources.oreVeins)
            {
                if (v.Value.GetType() == T && v.Value.basicItemInfo.id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }
            foreach(var v in this.Tools)
            {
                if (v.Value.GetType() == T && (v.Value as IBasicItemInfoProvider).getItemInformation().id == ID)
                {
                    Item I = v.Value.getOne();
                    return I;
                }
            }

            return null;
        }

    }
}
