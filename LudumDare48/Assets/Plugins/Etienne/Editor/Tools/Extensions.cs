using UnityEngine;

namespace Etienne {
    public static class Extensions {

        #region Vector3
        /// <summary>
        /// Get a direction from start to end
        /// </summary>
        /// <param name="start">The start of the direction</param>
        /// <param name="end">The end of the direction</param>
        /// <returns>end - start</returns>
        public static Vector3 Direction(this Vector3 start, Vector3 end) {
            return end - start;
        }

        /// <summary>
        /// Get a direction from start to end
        /// </summary>
        /// <param name="start">The start of the direction</param>
        /// <param name="end">The end of the direction</param>
        /// <returns>end - start</returns>
        public static Vector3 Direction(this Transform start, Transform end) {
            return start.position.Direction(end.position);
        }
        #endregion
    }
}