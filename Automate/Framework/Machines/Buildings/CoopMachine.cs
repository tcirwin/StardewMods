using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Buildings;
using Microsoft.Xna.Framework;
using SObject = StardewValley.Object;
using SGame = StardewValley.Game1;
using StardewValley.Network;

namespace Pathoschild.Stardew.Automate.Framework.Machines.Buildings
{
    internal class CoopMachine: BaseMachine<Coop>
    {
        private static readonly KeyValuePair<Vector2, SObject> EmptyValue =
            new KeyValuePair<Vector2, SObject>(Vector2.Zero, new SObject());

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

        public CoopMachine(Coop coop, GameLocation location)
            : base(coop, location, BaseMachine.GetTileAreaFor(coop)) { }

        public override MachineState GetState()
        {
            this.ProcessFloorItems();
            if (!this.NextOutputProduct.Equals(EmptyValue))
                return MachineState.Done;
            return MachineState.Processing;
        }

        public override ITrackedStack GetOutput()
        {
            void emptyAction(Item _) => this.Machine.indoors.Value.removeObject(this.NextOutputProduct.Key, false);

            return new TrackedItem(this.NextOutputProduct.Value, emptyAction);
        }

        public override bool SetInput(IStorage input)
        {
            return false; // no inputs
        }

        private void ProcessFloorItems()
        {
            Dictionary<Vector2, SObject> floorItems = this.Machine.indoors.Value.objects.FirstOrDefault();

            foreach (var floorItem in floorItems)
            {
                if (this.Products.Contains(floorItem.Value.DisplayName))
                {
                    this.NextOutputProduct = floorItem;
                    return;
                }
            }

            this.NextOutputProduct = EmptyValue;
        }
    }
}
