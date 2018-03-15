using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Forge.Forms.FormBuilding
{
    public interface IEnvironment : IEnumerable<string>
    {
        /// <summary>
        /// Returns whether the specified key is contained in this environment.
        /// </summary>
        [IndexerName("Item")]
        bool this[string key] { get; }

        /// <summary>
        /// Returns true if the flag exists in the environment.
        /// </summary>
        /// <param name="key">Flag key.</param>
        bool Has(string key);

        /// <summary>
        /// Adds multiple flags to the environment.
        /// Offers a performance benefit for bulk insertions.
        /// </summary>
        /// <returns>Number of flags added.</returns>
        int Add(IEnumerable<string> values);

        /// <summary>
        /// Adds a flag to the environment.
        /// </summary>
        /// <returns>True if the item has been added, false if it already existed.</returns>
        bool Add(string value);

        /// <summary>
        /// Remove multiple flags from the environment.
        /// Offers a performance benefit for bulk deletions.
        /// </summary>
        /// <returns>Number of flags removed.</returns>
        int Remove(IEnumerable<string> values);

        /// <summary>
        /// Removes a flag from the environment.
        /// </summary>
        /// <returns>True if the item has been removed, false if it did not exist.</returns>
        bool Remove(string value);

        /// <summary>
        /// Clears all flags from the environment.
        /// </summary>
        void Clear();
    }
}