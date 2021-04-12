using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omegasis.Revitalize.Framework.Energy;
using Omegasis.Revitalize.Framework.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Revitalize.Framework;
using Revitalize.Framework.Crafting;
using Revitalize.Framework.Managers;
using Revitalize.Framework.Menus;
using Revitalize.Framework.Menus.Machines;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines
{
    public class Machine : CustomObject, IInventoryManagerProvider, IEnergyManagerProvider, IFluidManagerProvider
    {

        [XmlIgnore]
        public List<ResourceInformation> producedResources
        {
            get
            {
                return MachineUtilities.GetResourcesProducedByThisMachine(this.basicItemInfo.id);
            }
            set
            {
                if (MachineUtilities.ResourcesForMachines == null) MachineUtilities.InitializeResourceList();
                if (MachineUtilities.ResourcesForMachines.ContainsKey(this.basicItemInfo.id)) return;
                MachineUtilities.ResourcesForMachines.Add(this.basicItemInfo.id, value);
            }
        }
        public int energyRequiredPer10Minutes;
        public int timeToProduce;

        public string craftingRecipeBook;

        [XmlIgnore]
        protected AnimationManager machineStatusBubbleBox;

        public Fluid requiredFluidForOperation;
        public int amountOfFluidRequiredForOperation;

        public Machine()
        {

        }


        public Machine(BasicItemInformation info, List<ResourceInformation> ProducedResources = null, int EnergyRequiredPer10Minutes = 0, int TimeToProduce = 0, string CraftingBook = "", Fluid FluidRequiredForOperation = null, int FluidAmountRequiredPerOperation = 0) : base(info)
        {
            this.producedResources = ProducedResources ?? new List<ResourceInformation>();
            this.energyRequiredPer10Minutes = EnergyRequiredPer10Minutes;
            this.timeToProduce = TimeToProduce;
            this.MinutesUntilReady = TimeToProduce;
            this.craftingRecipeBook = CraftingBook;
            this.createStatusBubble();
            this.requiredFluidForOperation = FluidRequiredForOperation;
            this.amountOfFluidRequiredForOperation = FluidAmountRequiredPerOperation;
        }

        public Machine(BasicItemInformation info, Vector2 TileLocation, List<ResourceInformation> ProducedResources = null, int EnergyRequiredPer10Minutes = 0, int TimeToProduce = 0, string CraftingBook = "", Fluid FluidRequiredForOperation = null, int FluidAmountRequiredPerOperation = 0) : base(info, TileLocation)
        {
            this.producedResources = ProducedResources ?? new List<ResourceInformation>();
            this.energyRequiredPer10Minutes = EnergyRequiredPer10Minutes;
            this.timeToProduce = TimeToProduce;
            this.MinutesUntilReady = TimeToProduce;
            this.craftingRecipeBook = CraftingBook;
            this.createStatusBubble();
            this.requiredFluidForOperation = FluidRequiredForOperation;
            this.amountOfFluidRequiredForOperation = FluidAmountRequiredPerOperation;
        }

        public virtual bool doesMachineProduceItems()
        {
            return this.producedResources.Count > 0;
        }

        public virtual bool doesMachineConsumeEnergy()
        {
            if (ModCore.Configs.machinesConfig.doMachinesConsumeEnergy == false)
            {
                //ModCore.log("Machine config disables energy consumption.");
                return false;
            }
            if (this.energyRequiredPer10Minutes == 0)
            {
                //ModCore.log("Machine rquires 0 energy to run.");
                return false;
            }
            if (this.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Consumes)
            {
                //ModCore.log("Machine does consume energy.");
                return true;
            }
            if (this.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Storage)
            {

                return true;
            }
            //ModCore.log("Unknown energy configuration.");
            return false;
        }


        public override bool minutesElapsed(int minutes, GameLocation environment)
        {

                //ModCore.log("Update container object for production!");
                //this.MinutesUntilReady -= minutes;
                int remaining = minutes;
                //ModCore.log("Minutes elapsed: " + remaining);
                List<IEnergyManagerProvider> energySources = new List<IEnergyManagerProvider>();
                if (this.doesMachineConsumeEnergy() || this.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Storage)
                {
                    //ModCore.log("This machine drains energy: " + this.info.name);
                    energySources = this.EnergyGraphSearchSources(); //Only grab the network once.
                }

                if (this.doesMachineProduceItems())
                {
                    while (remaining > 0)
                    {

                        if (this.doesMachineConsumeEnergy())
                        {
                            this.drainEnergyFromNetwork(energySources); //Continually drain from the network.                        
                            if (this.GetEnergyManager().remainingEnergy < this.energyRequiredPer10Minutes) return false;
                            else
                            {
                                this.GetEnergyManager().consumeEnergy(this.energyRequiredPer10Minutes); //Consume the required amount of energy necessary.
                            }
                        }
                        else
                        {
                            //ModCore.log("Does not produce energy or consume energy so do whatever!");
                        }
                        remaining -= 10;
                        this.MinutesUntilReady -= 10;

                        if (this.MinutesUntilReady <= 0 && this.GetInventoryManager().IsFull == false)
                        {
                            this.produceItem();
                            this.MinutesUntilReady = this.timeToProduce;
                        }
                    }
                }
                if (this.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Produces)
                {
                    while (remaining > 0)
                    {
                        remaining -= 10;
                        this.produceEnergy();
                        this.storeEnergyToNetwork();
                    }
                }
                if (this.MinutesUntilReady > 0)
                {
                    this.MinutesUntilReady = Math.Max(0, this.MinutesUntilReady - minutes);

                    if (this.GetInventoryManager().hasItemsInBuffer && this.MinutesUntilReady == 0)
                    {
                        this.GetInventoryManager().dumpBufferToItems();
                    }

                }

                return false;
            

            //return base.minutesElapsed(minutes, environment);
        }


        protected virtual void createStatusBubble()
        {
            this.machineStatusBubbleBox = new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "HUD", "MachineStatusBubble"), new Animation(0, 0, 20, 24), new Dictionary<string, List<Animation>>()
            {
                {"Default",new List<Animation>(){new Animation(0,0,20,24)}},
                {"Empty",new List<Animation>(){new Animation(20,0,20,24)}},
                {"InventoryFull",new List<Animation>(){new Animation(40,0,20,24)}}
            }, "Default", 0);
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);

            this.AnimationManager.prepareForNextUpdateTick();
        }


        public override bool rightClicked(Farmer who)
        {
            /*
            if (this.getCurrentLocation() == null)
                this.location = Game1.player.currentLocation;
            if (Game1.menuUp || Game1.currentMinigame != null) return false;

            //ModCore.playerInfo.sittingInfo.sit(this, Vector2.Zero);
            */
            if (Game1.menuUp || Game1.currentMinigame != null) return false;
            this.createMachineMenu();
            return false;
        }

        /// <summary>
        /// Creates the necessary components to display the machine menu properly.
        /// </summary>
        protected virtual void createMachineMenu()
        {
            MachineMenu machineMenu = new MachineMenu((Game1.viewport.Width / 2) - 400, 0, 800, 600);

            MachineSummaryMenu m = new MachineSummaryMenu((Game1.viewport.Width / 2) - 400, 48, 800, 600, Color.White, this, this.energyRequiredPer10Minutes);
            machineMenu.addInMenuTab("Summary", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("SummaryTab", new Vector2(), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTab"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), m, true);

            if (this.GetInventoryManager().capacity > 0)
            {
                InventoryTransferMenu transferMenu = new InventoryTransferMenu(100, 150, 500, 600, this.GetInventoryManager().items, this.GetInventoryManager().capacity, this.GetInventoryManager().displayRows, this.GetInventoryManager().displayColumns);
                machineMenu.addInMenuTab("Inventory", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Inventory Tab", new Vector2(), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTab"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), transferMenu, false);
            }

            if (string.IsNullOrEmpty(this.craftingRecipeBook) == false)
            {
                CraftingMenuV1 craftingMenu = CraftingRecipeBook.CraftingRecipesByGroup[this.craftingRecipeBook].getCraftingMenuForMachine(100, 100, 400, 700, ref this.GetInventoryManager().items, ref this.GetInventoryManager().bufferItems, this);
                machineMenu.addInMenuTab("Crafting", new AnimatedButton(new StardustCore.Animations.AnimatedSprite("Crafting Tab", new Vector2(), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Menus", "MenuTab"), new Animation(0, 0, 24, 24)), Color.White), new Rectangle(0, 0, 24, 24), 2f), craftingMenu, false);
            }

            if (Game1.activeClickableMenu == null) Game1.activeClickableMenu = machineMenu;
        }

        public override Item getOne()
        {
            Machine component = new Machine(this.basicItemInfo.Copy(), this.producedResources, this.energyRequiredPer10Minutes, this.timeToProduce, this.craftingRecipeBook, this.requiredFluidForOperation, this.amountOfFluidRequiredForOperation);
            return component;
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (x <= -1)
            {
                return;
            }

            //The actual planter box being drawn.
            if (this.AnimationManager == null)
            {
                spriteBatch.Draw(this.CurrentTextureToDisplay, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), new Rectangle?(this.AnimationManager.currentAnimation.sourceRectangle), this.basicItemInfo.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)(y * Game1.tileSize) / 10000f));
                // Log.AsyncG("ANIMATION IS NULL?!?!?!?!");
            }

            else
            {
                //Log.AsyncC("Animation Manager is working!");

                float addedDepth = 0;
                this.AnimationManager.draw(spriteBatch, this.CurrentTextureToDisplay, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.TileLocation.X * Game1.tileSize), this.TileLocation.Y * Game1.tileSize)), new Rectangle?(this.AnimationManager.currentAnimation.sourceRectangle), this.basicItemInfo.DrawColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, (float)((this.TileLocation.Y + addedDepth) * Game1.tileSize) / 10000f) + .00001f);
                this.drawStatusBubble(spriteBatch,(int) this.TileLocation.X, (int)this.TileLocation.Y-1, alpha);

                try
                {
                    if (this.AnimationManager.canTickAnimation())
                    {
                        this.AnimationManager.tickAnimation();
                    }
                    // Log.AsyncC("Tick animation");
                }
                catch (Exception err)
                {
                    ModCore.ModMonitor.Log(err.ToString());
                }
            }

            // spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((double)tileLocation.X * (double)Game1.tileSize + (((double)tileLocation.X * 11.0 + (double)tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2), (float)((double)tileLocation.Y * (double)Game1.tileSize + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)((double)tileLocation.X * 51.0 + (double)tileLocation.Y * 77.0) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(((double)tileLocation.Y * (double)Game1.tileSize + (double)(Game1.tileSize / 2) + (((double)tileLocation.Y * 11.0 + (double)tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0));

        }

        public virtual void produceItem()
        {
            foreach (ResourceInformation r in this.producedResources)
            {
                if (r.shouldDropResource())
                {
                    Item i = r.getItemDrops();
                    this.GetInventoryManager().addItem(i);
                    //ModCore.log("Produced an item!");
                }
            }

        }

        public virtual void produceEnergy()
        {
            if (this.GetEnergyManager().canReceieveEnergy)
            {
                this.GetEnergyManager().produceEnergy(this.energyRequiredPer10Minutes);
            }

        }

        public virtual void produceEnergy(double ratio)
        {

            if (this.GetEnergyManager().canReceieveEnergy)
            {
                this.GetEnergyManager().produceEnergy((int)(this.energyRequiredPer10Minutes * ratio));
            }

        }

        protected virtual void drawStatusBubble(SpriteBatch b, int x, int y, float Alpha)
        {
            if (this.machineStatusBubbleBox == null) this.createStatusBubble();
            if (this.GetInventoryManager() == null) return;
            if (this.GetInventoryManager().IsFull && this.doesMachineProduceItems() && ModCore.Configs.machinesConfig.showMachineNotificationBubble_InventoryFull)
            {
                y--;
                float num = (float)(4.0 * Math.Round(Math.Sin(DateTime.UtcNow.TimeOfDay.TotalMilliseconds / 250.0), 2));
                this.machineStatusBubbleBox.playAnimation("InventoryFull");
                this.machineStatusBubbleBox.draw(b, this.machineStatusBubbleBox.getTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize + num)), new Rectangle?(this.machineStatusBubbleBox.currentAnimation.sourceRectangle), Color.White * ModCore.Configs.machinesConfig.machineNotificationBubbleAlpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 2) * Game1.tileSize) / 10000f) + .00001f);
            }
            else
            {

            }
        }


        public virtual ref EnergyManager GetEnergyManager()
        {
            if (this.basicItemInfo == null)
            {
            }

            return ref this.basicItemInfo.EnergyManager;
        }

        public virtual void SetEnergyManager(EnergyManager Manager)
        {
            this.basicItemInfo.EnergyManager = Manager;
        }

        public virtual ref InventoryManager GetInventoryManager()
        {
            if (this.basicItemInfo == null)
            {
                return ref this.basicItemInfo.inventory;
            }
            return ref this.basicItemInfo.inventory;
        }

        public virtual void SetInventoryManager(InventoryManager Manager)
        {
            this.basicItemInfo.inventory = Manager;
        }

        public virtual ref FluidManagerV2 GetFluidManager()
        {
            return ref this.basicItemInfo.fluidManager;
        }

        public virtual void SetFluidManager(FluidManagerV2 FluidManager)
        {
            this.basicItemInfo.fluidManager = FluidManager;
        }

        /// <summary>
        /// Gets corresponding neighbor objects that can interact with fluid.
        /// </summary>
        /// <returns></returns>
        public virtual List<IFluidManagerProvider> GetNeighboringFluidManagers()
        {
            Vector2 tileLocation = this.TileLocation;
            List<IFluidManagerProvider> customObjects = new List<IFluidManagerProvider>();
            GameLocation loc = this.getCurrentLocation();
            if (loc != null)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == j || i == (-j)) continue;

                        Vector2 neighborTile = tileLocation + new Vector2(i, j);
                        if (loc.isObjectAtTile((int)neighborTile.X, (int)neighborTile.Y))
                        {
                            StardewValley.Object obj = loc.getObjectAtTile((int)neighborTile.X, (int)neighborTile.Y);
                            if (obj is IFluidManagerProvider)
                            {

                                if ((obj as IFluidManagerProvider).GetFluidManager().InteractsWithFluids)
                                {
                                    customObjects.Add((IFluidManagerProvider)obj);
                                    //ModCore.log("Found a neighboring fluid manager");
                                }
                                else
                                {
                                    //ModCore.log("Found a neighboring object but it isn't a valid fluid manager.");
                                }
                            }
                            else continue;
                        }
                    }
                }
            }
            return customObjects;
        }

        /// <summary>
        /// Searches a network of fluid managers to see if any of these fluid managers have an output tank with the corresponding fluid.
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        protected virtual List<IFluidManagerProvider> FluidGraphSearchForFluidFromOutputTanks(Fluid L)
        {
            List<IFluidManagerProvider> fluidSources = new List<IFluidManagerProvider>();
            HashSet<IFluidManagerProvider> searchedComponents = new HashSet<IFluidManagerProvider>();
            Queue<IFluidManagerProvider> entitiesToSearch = new Queue<IFluidManagerProvider>();
            foreach (IFluidManagerProvider tile in this.GetNeighboringFluidManagers())
            {
                entitiesToSearch.Enqueue(tile);
            }
            //entitiesToSearch.AddRange(this.GetNeighboringFluidManagers());
            searchedComponents.Add(this);
            while (entitiesToSearch.Count > 0)
            {
                IFluidManagerProvider searchComponent = entitiesToSearch.Dequeue();
                //entitiesToSearch.Remove(searchComponent);
                if (searchedComponents.Contains(searchComponent))
                {
                    continue;
                }
                else
                {
                    searchedComponents.Add(searchComponent);

                    List<IFluidManagerProvider> neighbors = searchComponent.GetNeighboringFluidManagers();

                    foreach (IFluidManagerProvider tile in neighbors)
                    {
                        if ( searchedComponents.Contains(tile)) continue;
                        else
                        {
                            entitiesToSearch.Enqueue(tile);
                        }
                    }

                    if (searchComponent.GetFluidManager().doesThisOutputTankContainThisFluid(L))
                    {
                        fluidSources.Add(searchComponent);
                        //ModCore.log("Found a tank that contains this fluid!");
                    }
                }

            }
            return fluidSources;
        }

        protected virtual List<IFluidManagerProvider> FluidGraphSearchInputTanksThatCanAcceptThisFluid(Fluid L)
        {
            List<IFluidManagerProvider> fluidSources = new List<IFluidManagerProvider>();
            List<IFluidManagerProvider> searchedComponents = new List<IFluidManagerProvider>();
            List<IFluidManagerProvider> entitiesToSearch = new List<IFluidManagerProvider>();
            entitiesToSearch.AddRange(this.GetNeighboringFluidManagers());
            searchedComponents.Add(this);
            while (entitiesToSearch.Count > 0)
            {
                IFluidManagerProvider searchComponent = entitiesToSearch[0];
                entitiesToSearch.Remove(searchComponent);
                if (searchedComponents.Contains(searchComponent))
                {
                    continue;
                }
                else
                {
                    searchedComponents.Add(searchComponent);
                    entitiesToSearch.AddRange(searchComponent.GetNeighboringFluidManagers());

                    if ((searchComponent).GetFluidManager().canRecieveThisFluid(L))
                    {
                        fluidSources.Add(searchComponent);
                    }
                }

            }
            return fluidSources;
        }

        /// <summary>
        /// Searches for output tanks that have the corresponding fluid and tries to drain from them.
        /// </summary>
        /// <param name="L"></param>
        public void pullFluidFromNetworkOutputs(Fluid L)
        {
            List<IFluidManagerProvider> energySources = this.FluidGraphSearchForFluidFromOutputTanks(L);

            int index = 0;

            for (int i = 0; i < energySources.Count; i++)
            {
                FluidManagerV2 other = energySources[i].GetFluidManager();
                other.outputFluidToOtherSources(this.GetFluidManager());
                if (this.GetFluidManager().canRecieveThisFluid(L) == false) break; //Since we already check for valid tanks this will basically check again to see if the tanks are full.
            }
        }

        /// <summary>
        /// Searches for output tanks that have the corresponding fluid and tries to drain from them.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="FluidSources"></param>
        public void pullFluidFromNetworkOutputs(List<IFluidManagerProvider> FluidSources, Fluid L)
        {
            List<IFluidManagerProvider> energySources = FluidSources;

            int index = 0;

            for (int i = 0; i < energySources.Count; i++)
            {
                FluidManagerV2 other = energySources[i].GetFluidManager();
                other.outputFluidToOtherSources(this.GetFluidManager());
                if (this.GetFluidManager().canRecieveThisFluid(L) == false) break; //Since we already check for valid tanks this will basically check again to see if the tanks are full.
            }
        }



        /// <summary>
        /// Gets a list of neighboring tiled objects that produce or transfer energy. This should be used for machines/objects that consume or transfer energy
        /// </summary>
        /// <returns></returns>
        protected virtual List<IEnergyManagerProvider> GetNeighboringOutputEnergySources()
        {
            Vector2 tileLocation = this.TileLocation;
            List<IEnergyManagerProvider> customObjects = new List<IEnergyManagerProvider>();
            GameLocation loc = this.getCurrentLocation();
            if (loc != null)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == j || i == (-j)) continue;

                        Vector2 neighborTile = tileLocation + new Vector2(i, j);
                        if (loc.isObjectAtTile((int)neighborTile.X, (int)neighborTile.Y))
                        {
                            StardewValley.Object obj = loc.getObjectAtTile((int)neighborTile.X, (int)neighborTile.Y);
                            if (obj is IEnergyManagerProvider)
                            {
                                EnergyManager eManager = (obj as IEnergyManagerProvider).GetEnergyManager();
                                if (eManager.energyInteractionType == Enums.EnergyInteractionType.Produces || eManager.energyInteractionType == Enums.EnergyInteractionType.Transfers || eManager.energyInteractionType == Enums.EnergyInteractionType.Storage)
                                {
                                    customObjects.Add((obj as IEnergyManagerProvider));
                                }
                            }
                            else continue;
                        }
                    }
                }
            }


            return customObjects;

        }

        /// <summary>
        /// Gets a list of neighboring tiled objects that consume or transfer energy. This should be used for machines/objects that produce or transfer energy
        /// </summary>
        /// <returns></returns>
        protected virtual List<IEnergyManagerProvider> GetNeighboringInputEnergySources()
        {
            Vector2 tileLocation = this.TileLocation;
            List<IEnergyManagerProvider> customObjects = new List<IEnergyManagerProvider>();
            GameLocation loc = this.getCurrentLocation();
            if (loc != null)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == j || i == (-j)) continue;

                        Vector2 neighborTile = tileLocation + new Vector2(i, j);
                        if (loc.isObjectAtTile((int)neighborTile.X, (int)neighborTile.Y))
                        {
                            StardewValley.Object obj = loc.getObjectAtTile((int)neighborTile.X, (int)neighborTile.Y);
                            if (obj is IEnergyManagerProvider)
                            {
                                EnergyManager eManager = (obj as IEnergyManagerProvider).GetEnergyManager();
                                if (eManager.energyInteractionType == Enums.EnergyInteractionType.Consumes || eManager.energyInteractionType == Enums.EnergyInteractionType.Transfers || eManager.energyInteractionType == Enums.EnergyInteractionType.Storage)
                                {
                                    customObjects.Add((obj as IEnergyManagerProvider));
                                }
                            }
                            else continue;
                        }
                    }
                }
            }


            return customObjects;
        }

        /// <summary>
        /// Gets the appropriate energy neighbors to move energy around from/to.
        /// </summary>
        /// <returns></returns>
        public virtual List<IEnergyManagerProvider> getAppropriateEnergyNeighbors()
        {
            if (this.GetEnergyManager().consumesEnergy)
            {
                return this.GetNeighboringOutputEnergySources();
            }
            else if (this.GetEnergyManager().producesEnergy)
            {
                return this.GetNeighboringInputEnergySources();
            }
            else if (this.GetEnergyManager().transfersEnergy)
            {
                List<IEnergyManagerProvider> objs = new List<IEnergyManagerProvider>();
                objs.AddRange(this.GetNeighboringInputEnergySources());
                objs.AddRange(this.GetNeighboringOutputEnergySources());
                return objs;
            }
            return new List<IEnergyManagerProvider>();
        }

        /// <summary>
        /// Gets all of the energy nodes in a network that are either producers or is storage.
        /// </summary>
        /// <returns></returns>
        protected virtual List<IEnergyManagerProvider> EnergyGraphSearchSources()
        {
            List<IEnergyManagerProvider> energySources = new List<IEnergyManagerProvider>();
            List<IEnergyManagerProvider> searchedComponents = new List<IEnergyManagerProvider>();
            List<IEnergyManagerProvider> entitiesToSearch = new List<IEnergyManagerProvider>();
            entitiesToSearch.AddRange(this.getAppropriateEnergyNeighbors());
            searchedComponents.Add(this);
            while (entitiesToSearch.Count > 0)
            {
                IEnergyManagerProvider searchComponent = entitiesToSearch[0];
                entitiesToSearch.Remove(searchComponent);
                if (searchedComponents.Contains(searchComponent))
                {
                    continue;
                }
                else
                {
                    searchedComponents.Add(searchComponent);
                    entitiesToSearch.AddRange(searchComponent.getAppropriateEnergyNeighbors());

                    if (searchComponent.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Produces || searchComponent.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Storage)
                    {
                        energySources.Add(searchComponent);
                    }
                }

            }
            return energySources;
        }

        /// <summary>
        /// Gets all of the energy nodes in a network that are either consumers or storage. This should ALWAYS be ran after EnergyGraphSearchSources
        /// </summary>
        /// <returns></returns>
        protected virtual List<IEnergyManagerProvider> EnergyGraphSearchConsumers()
        {
            List<IEnergyManagerProvider> energySources = new List<IEnergyManagerProvider>();
            List<IEnergyManagerProvider> searchedComponents = new List<IEnergyManagerProvider>();
            List<IEnergyManagerProvider> entitiesToSearch = new List<IEnergyManagerProvider>();
            entitiesToSearch.AddRange(this.getAppropriateEnergyNeighbors());
            searchedComponents.Add(this);
            while (entitiesToSearch.Count > 0)
            {
                IEnergyManagerProvider searchComponent = entitiesToSearch[0];
                entitiesToSearch.Remove(searchComponent);
                if (searchedComponents.Contains(searchComponent))
                {
                    continue;
                }
                else
                {
                    searchedComponents.Add(searchComponent);
                    entitiesToSearch.AddRange(searchComponent.getAppropriateEnergyNeighbors());

                    if (searchComponent.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Consumes || searchComponent.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Storage)
                    {
                        energySources.Add(searchComponent);
                    }
                }

            }
            return energySources;
        }

        /// <summary>
        /// Gets all nodes in a connected energy network and tries to drain the necessary amount of energy from the network.
        /// </summary>
        public void drainEnergyFromNetwork()
        {
            //Machines that consume should ALWAYS try to drain energy from a network first.
            //Then producers should always try to store energy to a network.
            //Storage should never try to push or pull energy from a network as consumers will pull from storage and producers will push to storage.
            //Transfer nodes are used just to connect the network.
            List<IEnergyManagerProvider> energySources = this.EnergyGraphSearchSources();

            int index = 0;

            for (int i = 0; i < energySources.Count; i++)
            {
                this.GetEnergyManager().transferEnergyFromAnother(energySources[i].GetEnergyManager(), this.GetEnergyManager().capacityRemaining);
                if (this.GetEnergyManager().hasMaxEnergy) break;
            }
        }

        public void drainEnergyFromNetwork(List<IEnergyManagerProvider> energySources)
        {
            int index = 0;

            for (int i = 0; i < energySources.Count; i++)
            {
                this.GetEnergyManager().transferEnergyFromAnother(energySources[i].GetEnergyManager(), this.GetEnergyManager().capacityRemaining);
                if (this.GetEnergyManager().hasMaxEnergy) break;
            }
        }

        /// <summary>
        /// Gets all of the nodes in a connected energy network and tries to store the necessary amount of energy from the network.
        /// </summary>
        public void storeEnergyToNetwork()
        {
            List<IEnergyManagerProvider> energySources = this.EnergyGraphSearchConsumers();

            int index = 0;

            for (int i = 0; i < energySources.Count; i++)
            {
                this.GetEnergyManager().transferEnergyToAnother(energySources[i].GetEnergyManager(), this.GetEnergyManager().capacityRemaining);
                if (this.GetEnergyManager().hasEnergy == false) break;
            }
        }

        public void storeEnergyToNetwork(List<IEnergyManagerProvider> energySources)
        {

            int index = 0;

            for (int i = 0; i < energySources.Count; i++)
            {
                this.GetEnergyManager().transferEnergyToAnother(energySources[i].GetEnergyManager(), this.GetEnergyManager().capacityRemaining);
                if (this.GetEnergyManager().hasEnergy == false) break;
            }
        }
    }
}
