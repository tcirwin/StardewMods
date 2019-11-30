using System.Collections.Generic;
using System.Linq;
using System.IO;
using StardewValley;
using StardewValley.Buildings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SObject = StardewValley.Object;
using SGame = StardewValley.Game1;

namespace Pathoschild.Stardew.Automate.Framework.Machines.Buildings
{
    internal class FarmAnimalMachine: BarnMachine
    {
        private readonly Farm Farm = null;
        private readonly Barn Barn = null;

        public FarmAnimalMachine(Farm farm, Barn barn, GameLocation location) : base(barn, location)
        {
            this.Farm = farm;
            this.Barn = barn;
            this.FindUnprocessedAnimals();
        }

        private void FindUnprocessedAnimals()
        {
            // Find animals belonging to the barn that are outside during processing
            foreach (FarmAnimal animal in this.Farm.animals.Values)
            {
                if (animal.currentProduce > 0 && animal.home == this.Barn)
                    this.UnprocessedAnimals.Add(animal);
            }
        }
    }
}
