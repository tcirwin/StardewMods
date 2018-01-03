using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Buildings;
using Microsoft.Xna.Framework;
using SObject = StardewValley.Object;

namespace Pathoschild.Stardew.Automate.Framework.Machines.Buildings
{
    internal class CoopMachine: IMachine
    {
        private static readonly KeyValuePair<Vector2, SObject> EmptyValue =
            new KeyValuePair<Vector2, SObject>(Vector2.Zero, new SObject());
        private readonly Coop Coop;

        private readonly List<string> Products = new List<string>
        {
            "Egg",
            "Large Egg",
            "Duck Egg",
            "Wool",
            "Duck Feather",
            "Rabbit's Foot",
            "Void Egg",
            "Dinosaur Egg"
        };

        private KeyValuePair<Vector2, SObject> NextOutputProduct = EmptyValue;

        public CoopMachine(Coop coop)
        {
            this.Coop = coop;
        }

        public MachineState GetState()
        {
            this.ProcessFloorItems();
            if (!this.NextOutputProduct.Equals(EmptyValue))
                return MachineState.Done;
            return MachineState.Processing;
        }

        public ITrackedStack GetOutput()
        {
            SerializableDictionary<Vector2, SObject> floorItems = this.Coop.indoors.objects;
            System.Action<Item> emptyAction = _ => floorItems.Remove(this.NextOutputProduct.Key);

            return new TrackedItem(this.NextOutputProduct.Value, emptyAction);
        }

        public bool SetInput(IStorage input)
        {
            return false; // no inputs
        }

        private void ProcessFloorItems()
        {
            SerializableDictionary<Vector2, SObject> floorItems = this.Coop.indoors.objects;

            foreach (var floorItem in floorItems)
            {
                if (this.Products.Contains(floorItem.Value.DisplayName))
                {
                    this.NextOutputProduct = floorItem;
                    break;
                }
            }
        }
    }
}
