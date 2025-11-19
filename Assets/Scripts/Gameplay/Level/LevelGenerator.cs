using System.Collections.Generic;
using Gameplay.Bricks;
using UnityEngine;

namespace Gameplay.Level
{
    /// <summary>
    /// Generates a grid-based randomized brick layout with guaranteed minimum bricks.
    /// </summary>
    public class LevelGenerator
    {
        private readonly IReadOnlyList<BrickBase> _brickPrefabs;
        private readonly int _rows;
        private readonly int _cols;
        private readonly float _cellWidth;
        private readonly float _cellHeight;
        private readonly float _startZ;
        private readonly float _spawnChance;
        private readonly int _minBricks;

        public LevelGenerator(
            IReadOnlyList<BrickBase> brickPrefabs,
            int rows = 6,
            int cols = 10,
            float width = 11f,
            float height = 7f,
            float startZ = -3f,
            float spawnChance = 0.6f,
            int minBricks = 5)
        {
            _brickPrefabs = brickPrefabs;
            _rows = rows;
            _cols = cols;
            _cellWidth = width / cols;
            _cellHeight = height / rows;
            _startZ = startZ;
            _spawnChance = spawnChance;
            _minBricks = minBricks;
        }

        public List<BrickBase> GenerateLevel(Transform parent)
        {
            List<BrickBase> bricks = new List<BrickBase>();
            bool[,] occupied = new bool[_rows, _cols];

            float startX = -(_cols * _cellWidth) * 0.5f + _cellWidth * 0.5f;
            float startZ = _startZ + _cellHeight * 0.5f;
            
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (Random.value > _spawnChance)
                    {
                        continue;
                    }

                    CreateBrick(r, c);
                }
            }
            
            while (bricks.Count < _minBricks)
            {
                int r = Random.Range(0, _rows);
                int c = Random.Range(0, _cols);

                if (!occupied[r, c])
                {
                    CreateBrick(r, c);
                }
            }

            return bricks;
            
            void CreateBrick(int r, int c)
            {
                occupied[r, c] = true;

                Vector3 position = new Vector3(
                    startX + c * _cellWidth,
                    0f,
                    startZ + r * _cellHeight
                );

                BrickBase prefab = _brickPrefabs[Random.Range(0, _brickPrefabs.Count)];

                BrickBase brick = Object.Instantiate(
                    prefab,
                    position,
                    Quaternion.identity,
                    parent
                );

                bricks.Add(brick);
            }
        }
    }
}