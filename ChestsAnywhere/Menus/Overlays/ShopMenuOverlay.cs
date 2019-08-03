using Pathoschild.Stardew.ChestsAnywhere.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Pathoschild.Stardew.ChestsAnywhere.Menus.Overlays
{
    /// <summary>A <see cref="ShopMenu"/> overlay which lets the player navigate and edit chests.</summary>
    internal class ShopMenuOverlay : BaseChestOverlay
    {
        /*********
        ** Fields
        *********/
        /// <summary>The underlying chest menu.</summary>
        private readonly ShopMenu Menu;

        /// <summary>The default highlight function for the chest items.</summary>
        private readonly InventoryMenu.highlightThisItem DefaultChestHighlighter;

        ///// <summary>The default highlight function for the player inventory items.</summary>
        //private readonly InventoryMenu.highlightThisItem DefaultInventoryHighlighter;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="menu">The underlying chest menu.</param>
        /// <param name="chest">The selected chest.</param>
        /// <param name="chests">The available chests.</param>
        /// <param name="config">The mod configuration.</param>
        /// <param name="keys">The configured key bindings.</param>
        /// <param name="events">The SMAPI events available for mods.</param>
        /// <param name="input">An API for checking and changing input state.</param>
        /// <param name="translations">Provides translations stored in the mod's folder.</param>
        /// <param name="showAutomateOptions">Whether to show Automate options.</param>
        public ShopMenuOverlay(ShopMenu menu, ManagedChest chest, ManagedChest[] chests, ModConfig config, ModConfigKeys keys, IModEvents events, IInputHelper input, ITranslationHelper translations, bool showAutomateOptions)
            : base(menu, chest, chests, config, keys, events, input, translations, showAutomateOptions, keepAlive: () => Game1.activeClickableMenu is ShopMenu, topOffset: Game1.pixelZoom * 6)
        {
            this.Menu = menu;
            this.DefaultChestHighlighter = menu.inventory.highlightMethod;
            //this.DefaultInventoryHighlighter = chest.Container.CanAcceptItem;
        }


        /*********
        ** Protected methods
        *********/
        /// <summary>Set whether the chest or inventory items should be clickable.</summary>
        /// <param name="clickable">Whether items should be clickable.</param>
        protected override void SetItemsClickable(bool clickable)
        {
            if (clickable)
            {
                this.Menu.inventory.highlightMethod = this.DefaultChestHighlighter;
                //this.MenuInventoryMenu.highlightMethod = this.DefaultInventoryHighlighter;
            }
            else
            {
                this.Menu.inventory.highlightMethod = item => false;
                //this.MenuInventoryMenu.highlightMethod = item => false;
            }
        }
    }
}
