using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ContentPatcher.Framework.Conditions;
using ContentPatcher.Framework.ConfigModels;
using Pathoschild.Stardew.Common.Utilities;
using StardewModdingAPI;

namespace ContentPatcher.Framework.Migrations
{
    /// <summary>Migrate patches to format version 1.8.</summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Named for clarity.")]
    internal class Migration_1_8 : BaseMigration
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public Migration_1_8()
            : base(new SemanticVersion(1, 8, 0))
        {
            this.AddedTokens = new InvariantHashSet
            {
                ConditionType.IsOutdoors.ToString(),
                ConditionType.LocationName.ToString(),
                ConditionType.Target.ToString(),
                ConditionType.TargetWithoutPath.ToString()
            };
        }

        /// <summary>Migrate a content pack.</summary>
        /// <param name="content">The content pack data to migrate.</param>
        /// <param name="error">An error message which indicates why migration failed.</param>
        /// <returns>Returns whether the content pack was successfully migrated.</returns>
        public override bool TryMigrate(ContentConfig content, out string error)
        {
            if (!base.TryMigrate(content, out error))
                return false;

            if (content.Changes?.Any() == true)
            {
                foreach (PatchConfig patch in content.Changes)
                {
                    // 1.8 adds EditMap
                    if (Enum.TryParse(patch.Action, true, out PatchType action) && action == PatchType.EditMap)
                    {
                        error = this.GetNounPhraseError($"using action {nameof(PatchType.EditMap)}");
                        return false;
                    }

                    // 1.8 adds MoveEntries
                    if (patch.MoveEntries?.Any() == true)
                    {
                        error = this.GetNounPhraseError($"using {nameof(PatchConfig.MoveEntries)}");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
