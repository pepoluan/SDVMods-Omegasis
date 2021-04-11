using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Omegasis.Revitalize.Framework.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Revitalize.Framework.Managers;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.EnergyGeneration
{
    public class Windmill : Machine
    {

        public Windmill() { }

        public Windmill(BasicItemInformation info, List<ResourceInformation> ProducedResources = null, int EnergyRequiredPer10Minutes = 0, int TimeToProduce = 0, string CraftingBook = "", Fluid FluidRequiredForOperation = null, int FluidAmountRequiredPerOperation = 0) : base(info)
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

        public Windmill(BasicItemInformation info, Vector2 TileLocation, List<ResourceInformation> ProducedResources = null, int EnergyRequiredPer10Minutes = 0, int TimeToProduce = 0, string CraftingBook = "", Fluid FluidRequiredForOperation = null, int FluidAmountRequiredPerOperation = 0) : base(info, TileLocation)
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

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);

            this.AnimationManager.prepareForNextUpdateTick();
        }


        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            this.AnimationManager.playAnimation("Working");
            int remaining = minutes;

            if (this.GetEnergyManager().energyInteractionType == Enums.EnergyInteractionType.Produces)
            {
                while (remaining > 0)
                {
                    remaining -= 10;
                    this.produceEnergy();
                    this.storeEnergyToNetwork();
                }
            }
            return false;
        }


        public override Item getOne()
        {
            Windmill component = new Windmill(this.getItemInformation().Copy(), this.producedResources, this.energyRequiredPer10Minutes, this.timeToProduce, this.craftingRecipeBook, this.requiredFluidForOperation, this.amountOfFluidRequiredForOperation);
            //component.containerObject = this.containerObject;
            //component.offsetKey = this.offsetKey;
            return component;
        }

        public override void produceEnergy()
        {
            GameLocation loc = this.getCurrentLocation();
            if (loc != null)
            {
                if (loc.IsOutdoors == false) return;
            }
            if (this.GetEnergyManager().canReceieveEnergy)
            {
                if (WeatherUtilities.IsWeatherGoodForWindmills())
                {
                    this.GetEnergyManager().produceEnergy((int)(this.energyRequiredPer10Minutes * ModCore.Configs.machinesConfig.windmill_windyDayPowerMultiplier));
                }
                else
                {
                    this.GetEnergyManager().produceEnergy(this.energyRequiredPer10Minutes);
                }
            }

        }
    }
}
