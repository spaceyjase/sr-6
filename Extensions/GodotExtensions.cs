using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Extensions
{
  public static class GodotExtensions
  {
    private static readonly Vector2[] directions =
    {
      Vector2.Left,
      Vector2.Right,
      Vector2.Up,
      Vector2.Down
    };

    public static IEnumerable<T> GetChildren<T>(this Node node)
    {
      foreach (var child in node.GetChildren())
      {
        if (!(child is T t)) continue;

        yield return t;
      }
    }

    public static Vector2 CalculateTilePosition(this TileMap grid, Vector2 position)
    {
      var tilePosition = grid.WorldToMap(position);
      return tilePosition;
    }

    // Returns the position of a cell's center in pixels.
    public static Vector2 CalculateMapPosition(this TileMap grid, Vector2 position)
    {
      if (!grid.IsWithinBounds(position))
      {
        GD.PrintErr($"Position {position} is out of bounds");
        return Vector2.Zero;
      }

      var localPosition = grid.MapToWorld(position);
      return grid.ToGlobal(localPosition) + grid.CellSize / 2f;
    }

    // Returns true if the position is within the grid.
    public static bool IsWithinBounds(this TileMap grid, Vector2 position)
    {
      return grid.GetCellv(position) != -1; // INVALID_CELL
    }

    public static int AsIndex(this TileMap grid, Vector2 cell)
    {
      //return Guid.NewGuid().GetHashCode();
      return ((int)cell.x + (int)grid.CellSize.x * (int)grid.CellSize.y) * (int)cell.y;
    }

    public static IEnumerable<Vector2> Directions => directions;

    public static bool IsWall(this TileMap level, Vector2 position)
    {
      return Directions.Any(direction => !level.IsWithinBounds(position + direction));
    }
  }
}