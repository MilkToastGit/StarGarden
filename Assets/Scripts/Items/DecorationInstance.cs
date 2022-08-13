using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;

namespace StarGarden.Items
{
    public class DecorationInstances : ItemInstances
    {
        public override int InventoryCount => totalCount - placedInstances.Count;
        public List<Vector3Int> placedInstances { get; private set; } = new List<Vector3Int>();
        private Decoration decor => Item as Decoration;

        public DecorationInstances() { }
        
        public DecorationInstances(Core.SaveData.DecorationSaveData data, Decoration item)
        {
            Item = item;
            totalCount = data.TotalCount;
            placedInstances = new List<Vector3Int>();
            foreach (SerializableVector3Int v in data.PlacedInstances)
                placedInstances.Add(v);
        }

        public void Place(Vector2Int position, int island)
        {
            placedInstances.Add(new Vector3Int(position.x, position.y, island));
            SaveDataManager.SaveItemData();
            //Debug.Log(placedInstances.Count);
        }

        public void Unplace(Vector2Int position, int island)
        {
            placedInstances.Remove(new Vector3Int(position.x, position.y, island));
            SaveDataManager.SaveItemData();
        }

        public void Move(Vector2Int initialPosition, Vector2Int newPosition, int island)
        {
            Vector3Int initPos = new Vector3Int(initialPosition.x, initialPosition.y, island);
            Vector3Int newPos = new Vector3Int(newPosition.x, newPosition.y, island);
            placedInstances[placedInstances.FindIndex(v => v == initPos)] = newPos;
            SaveDataManager.SaveItemData();
        }

        public void GetGridRange(Vector2Int pivot, out Vector2Int min, out Vector2Int max)
        {
            min = new Vector2Int(
                pivot.x - Mathf.FloorToInt((decor.Size.x - 1f) / 2f), // -1
                pivot.y - Mathf.FloorToInt((decor.Size.y - 1f) / 2f)); // 1
            max = new Vector2Int(
                pivot.x + Mathf.CeilToInt((decor.Size.x - 1f) / 2f), // 0
                pivot.y + Mathf.CeilToInt((decor.Size.y - 1f) / 2f)); // 2
        }

        public Vector2Int[] GetGridPoints(Vector2Int pivot)
        {
            GetGridRange(pivot, out Vector2Int min, out Vector2Int max);

            Vector2Int[] points = new Vector2Int[decor.Size.x * decor.Size.y];
            int i = 0;

            for (int y = min.y; y < max.y + 1; y++)
                for (int x = min.x; x < max.x + 1; x++)
                    points[i++] = new Vector2Int(x, y);

            return points;
        }

        public bool IsPositionTaken(Vector2Int position, int island)
        {
            foreach (Vector3Int origin in placedInstances)
            {
                if (origin.z != island) continue;
                GetGridRange((Vector2Int)origin, out Vector2Int min, out Vector2Int max);
                if (position.Between(min, max))
                    return true;
            }
            return false;
        }

        public bool DoDecorOverlap(Vector2Int decorMin, Vector2Int decorMax, Vector2Int lastPlacedPoint, int island, bool hasBeenPlaced)
        {
            Vector2Int[] corners = new Vector2Int[] {
                decorMin, new Vector2Int(decorMin.x, decorMax.y), decorMax, new Vector2Int(decorMax.x, decorMin.y) };

            foreach (Vector3Int origin in placedInstances)
            {
                if (origin.z != island) continue;
                if (hasBeenPlaced && (Vector2Int)origin == lastPlacedPoint) continue;

                GetGridRange((Vector2Int)origin, out Vector2Int min, out Vector2Int max);
                //Debug.Log($"{decorMin}, {decorMax} | {min}, {max}");

                foreach (Vector2Int corner in corners)
                    if (corner.Between(min, max))
                        return true;
            }
            return false;
        }

        //public bool IsPositionTaken(Vector2Int position)
        //{
        //    Debug.Log(placedInstances.FindIndex(p => p == position));
        //    return placedInstances.FindIndex(p => p == position) < 0;
        //}
    }
}