using System;
using System.IO;
using StardewModdingAPI;

namespace ContentPatcher.Framework
{
    /// <summary>Handles loading assets from content packs.</summary>
    internal class ManagedContentPack
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The managed content pack.</summary>
        public IContentPack Pack { get; }

        /// <summary>The content pack's manifest.</summary>
        public IManifest Manifest => this.Pack.Manifest;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="pack">The content pack to manage.</param>
        public ManagedContentPack(IContentPack pack)
        {
            this.Pack = pack;
        }

        /// <summary>Get whether a file exists in the content pack.</summary>
        /// <param name="key">The asset key.</param>
        public bool FileExists(string key)
        {
            return this.GetRealPath(key) != null;
        }

        /// <summary>Get an asset from the content pack.</summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <param name="key">The asset key.</param>
        public T Load<T>(string key)
        {
            key = this.GetRealPath(key) ?? throw new FileNotFoundException($"The file '{key}' does not exist in the {this.Pack.Manifest.Name} content patch folder.");
            return this.Pack.LoadAsset<T>(key);
        }

        /// <summary>Read a JSON file from the content pack folder.</summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="path">The file path relative to the content pack directory.</param>
        /// <returns>Returns the deserialised model, or <c>null</c> if the file doesn't exist or is empty.</returns>
        public TModel ReadJsonFile<TModel>(string path) where TModel : class
        {
            return this.Pack.ReadJsonFile<TModel>(path);
        }

        /// <summary>Save data to a JSON file in the content pack's folder.</summary>
        /// <typeparam name="TModel">The model type. This should be a plain class that has public properties for the data you want. The properties can be complex types.</typeparam>
        /// <param name="path">The file path relative to the mod folder.</param>
        /// <param name="data">The arbitrary data to save.</param>
        public void WriteJsonFile<TModel>(string path, TModel data) where TModel : class
        {
            this.Pack.WriteJsonFile(path, data);
        }

        /// <summary>Get the raw absolute path for a path within the content pack.</summary>
        /// <param name="relativePath">The path relative to the content pack folder.</param>
        public string GetFullPath(string relativePath)
        {
            return Path.Combine(this.Pack.DirectoryPath, relativePath);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the actual relative path within the content pack for a file, matched case-insensitively, or <c>null</c> if not found.</summary>
        /// <param name="key">The case-insensitive asset key.</param>
        private string GetRealPath(string key)
        {
            // try file match first
            var exactMatch = new FileInfo(Path.Combine(this.Pack.DirectoryPath, key));
            if (exactMatch.Exists)
                return exactMatch.FullName.Substring(this.Pack.DirectoryPath.Length + 1);

            // search for a case-insensitive file match (Linux/Mac are case-sensitive)
            foreach (string path in Directory.EnumerateFiles(this.Pack.DirectoryPath, "*", SearchOption.AllDirectories))
            {
                if (!path.StartsWith(this.Pack.DirectoryPath))
                    throw new InvalidOperationException("File search failed, contained files aren't in the searched folder (???).");

                string relativePath = path.Substring(this.Pack.DirectoryPath.Length + 1);
                if (relativePath.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                    return relativePath;
            }

            return null;
        }
    }
}
