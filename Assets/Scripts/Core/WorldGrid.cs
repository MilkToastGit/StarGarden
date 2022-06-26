using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public static class WorldGrid
    {
        public static Vector2 Spacing => spacing;
        //private static readonly Vector2 worldSize = new Vector2 (0.3f, 0.1f);
        private static readonly Vector2 spacing = new Vector2(0.3f, 0.3f);

        //public static int Width => gridSize.x;
        //public static int Height => gridSize.y;

        //private static Vector2Int gridSize
        //{
        //    get
        //    {
        //        if (calculatedGridSize == -Vector2Int.one)
        //        {
        //            calculatedGridSize.x = Mathf.CeilToInt(worldSize.x / spacing.x - 1);
        //            calculatedGridSize.y = Mathf.CeilToInt(worldSize.y / spacing.y - 1);
        //        }

        //        return calculatedGridSize;
        //    }
        //}

        //private static Vector2Int calculatedGridSize = -Vector2Int.one;

        public static Vector2 GridToWorld(Vector2Int point) { return GridToWorld(point.x, point.y); }
        public static Vector2 GridToWorld(int x, int y)
        {
            float posX = x * spacing.x;
            float posY = y * spacing.y;

            return new Vector2(posX, posY);
        }

        public static Vector2Int WorldToGrid(Vector2 position)
        {
            int y = Mathf.RoundToInt(position.y / spacing.y);
            int x = Mathf.RoundToInt(position.x / spacing.x);

            return new Vector2Int(x, y);
        }
    }
}