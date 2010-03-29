using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="UserPermissions"/> enum.
    /// </summary>
    public static class UserPermissionsExtensions
    {
        /// <summary>
        /// Gets if the given <see cref="UserPermissions"/> is set.
        /// </summary>
        /// <param name="p">The <see cref="UserPermissions"/>.</param>
        /// <param name="level">The <see cref="UserPermissions"/> to check if set.</param>
        /// <returns>
        /// True if the <paramref name="level"/> is set; otherwise false.
        /// </returns>
        public static bool IsSet(this UserPermissions p, UserPermissions level)
        {
            return (p & level) != 0;
        }

        /// <summary>
        /// Gets if the given <see cref="UserPermissions"/> is not set.
        /// </summary>
        /// <param name="p">The <see cref="UserPermissions"/>.</param>
        /// <param name="level">The <see cref="UserPermissions"/> to check if not set.</param>
        /// <returns>
        /// True if the <paramref name="level"/> is not set; otherwise false.
        /// </returns>
        public static bool IsNotSet(this UserPermissions p, UserPermissions level)
        {
            return !p.IsSet(level);
        }
    }
}