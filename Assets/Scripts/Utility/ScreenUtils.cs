using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public interface IScreenUtils : IUtility
  {
    Vector3 ClampToScreenBounds(Vector3 position);
  }
  public class ScreenUtils : IScreenUtils
  {
    /// <summary>
    /// Clamps the given screen position to the screen boundaries.
    /// </summary>
    /// <param name="position">The screen position to clamp.</param>
    /// <returns>The clamped screen position.</returns>
    public Vector3 ClampToScreenBounds(Vector3 position)
    {
      Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

      // Clamp the x and y position to be within the screen bounds
      position.x = Mathf.Clamp(position.x, screenRect.xMin, screenRect.xMax);
      position.y = Mathf.Clamp(position.y, screenRect.yMin, screenRect.yMax);

      return position;
    }
  }
}