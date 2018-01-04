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
    internal class BarnMachine: IMachine
    {
        private readonly AnimalHouse AnimalHouse = null;
        private List<FarmAnimal> UnprocessedAnimals;

        private static readonly Dictionary<string, string> HarvestTextures = new Dictionary<string, string>
        {
            { "Sheep", "ShearedSheep" }
        };

        public BarnMachine(Barn barn)
        {
            if (barn.indoors is AnimalHouse)
                this.AnimalHouse = (AnimalHouse)barn.indoors;

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
            this.UnprocessedAnimals.RemoveAt(0);
            this.ShowHarvestedTexture(animal);

            var produce = new SObject(animal.currentProduce, 1, false, -1, animal.produceQuality);
            return new TrackedItem(produce, _ => animal.currentProduce = -1);
        }

        public bool SetInput(IStorage input)
        {
            return false; // no inputs
        }

        private void ShowHarvestedTexture(FarmAnimal animal)
        {
            HarvestTextures.TryGetValue(animal.type, out string textureName);
            if (textureName != default(string) && animal.showDifferentTextureWhenReadyForHarvest)
            {
                string texturePath = Path.Combine("Animals", textureName);
                animal.Sprite.Texture = SGame.content.Load<Texture2D>(texturePath);
            }
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
