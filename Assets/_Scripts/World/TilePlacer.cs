using System.Collections.Generic;
using UnityEngine;

namespace PKFTestAssignment.World
{
    public class TilePlacer : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private Transform _tileContainer;

        private readonly List<GameObject> _placedTiles = new();

        public void PlaceTiles(int rowsToFill, int columnsToFill)
        {
            for (int i = rowsToFill; i >= -rowsToFill; i--) 
            {
                for (int j = columnsToFill; j >= -columnsToFill; j--) 
                {
                    Vector3 spawnPosition = _grid.CellToWorld(new Vector3Int(i, j, -1));

                    _placedTiles.Add(Instantiate(_tilePrefab, spawnPosition, Quaternion.identity, _tileContainer));
                }            
            }
        }

        public void RemovePlacedTile() 
        {
            while (_placedTiles.Count > 0) 
            {
                Destroy(_placedTiles[0]);
                _placedTiles.Remove(_placedTiles[0]);
            }
        }
    }
}