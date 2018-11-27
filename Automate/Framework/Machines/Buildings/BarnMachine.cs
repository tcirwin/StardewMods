using System.Collections.Generic;
using System.Linq;
using System.IO;
using StardewValley;
using StardewValley.Buildings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SObject = StardewValley.Object;
using SGame = StardewValley.Game1;
using StardewValley.Tools;
using StardewValley.Characters;
using System.Reflection;
using Netcode;

namespace Pathoschild.Stardew.Automate.Framework.Machines.Buildings
{
    internal class BarnMachine: IMachine
    {
        private readonly AnimalHouse AnimalHouse = null;
        protected List<FarmAnimal> UnprocessedAnimals;
        private int produceCount = 0;

        public BarnMachine(Barn barn)
        {
            if (barn.indoors.Value is AnimalHouse)
                this.AnimalHouse = (AnimalHouse)barn.indoors.Value;
            
            this.UnprocessedAnimals = new List<FarmAnimal>();
            this.FindUnprocessedAnimals();
        }

        public MachineState GetState()
        {
            if (this.UnprocessedAnimals.Count > 0)
                return MachineState.Done;
            return MachineState.Processing;
        }

        public ITrackedStack GetOutput()
        {
            FarmAnimal animal = this.UnprocessedAnimals.FirstOrDefault();
            if (animal == null)
            {
                return null;
            }
            this.UnprocessedAnimals.RemoveAt(0);
            Tool tool = new Shears();

            if (animal.toolUsedForHarvest == "Milk Pail")
                tool = new MilkPail();

            var produce = new SObject(animal.currentProduce, 1, false, -1, animal.produceQuality);

            FieldInfo info = typeof(FarmAnimal).GetField("currentProduce", BindingFlags.Instance | BindingFlags.Public);
            NetInt netint = new NetInt(-1);
            info.SetValue(animal, netint);

            this.produceCount += 1;

            return new TrackedItem(produce);
        }

        public bool SetInput(IStorage input)
        {
            return false; // no inputs
        }

        private void FindUnprocessedAnimals()
        {
            if (this.AnimalHouse == null)
                return;

            foreach (FarmAnimal animal in this.AnimalHouse.animals.Values)
            {
                if (animal.currentProduce > 0)
                    this.UnprocessedAnimals.Add(animal);
            }
        }
    }
}
