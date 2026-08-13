using System;
using Zenject;
using Services;
using Entities;
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public class ObstaclesManager : IObstaclesManager
    {
        public const float Height = -22f;
        public const float InitialPoint = -5f;

        public const float MinObstacleDistance = 0.5f;

        [Inject] private ILevelManager _levelManager;
        [Inject] private IPoolService _poolService;

        private readonly List<Obstacle> _obstacles = new();

        public void SpawnObstacles(LevelConfigData levelConfig)
        {
            Clear();

            if (levelConfig == null)
                throw new ArgumentNullException(nameof(levelConfig));

            if (levelConfig.Density <= 0f)
                return;

            var camera = Camera.main;

            if (camera == null)
                throw new InvalidOperationException("Main camera not found.");

            if (!TryGetVisibleBounds(camera, out var minX, out var maxX))
                return;

            var height = Mathf.Abs(Height - InitialPoint);
            var width = maxX - minX;

            if (width <= 0f || height <= 0f)
                return;

            var area = width * height;
            var obstacleCount = Mathf.RoundToInt(area * levelConfig.Density);

            if (obstacleCount <= 0)
                return;

            var minDistanceSqr = MinObstacleDistance * MinObstacleDistance;

            var spawnPositions = new List<Vector3>(obstacleCount);

            const int maxAttemptsPerObstacle = 100;

            for (var i = 0; i < obstacleCount; i++)
            {
                var spawned = false;

                for (var attempt = 0; attempt < maxAttemptsPerObstacle; attempt++)
                {
                    var position = new Vector3(
                        UnityEngine.Random.Range(minX, maxX),
                        0f,
                        UnityEngine.Random.Range(Height, InitialPoint));

                    if (!IsPositionVisible(camera, position))
                        continue;

                    if (!IsPositionValid(position, spawnPositions, minDistanceSqr))
                        continue;

                    spawnPositions.Add(position);
                    spawned = true;
                    break;
                }

                if (!spawned)
                {
                    Debug.LogWarning(
                        $"Could not find a valid position for obstacle {i + 1}/{obstacleCount}. " +
                        $"Spawned {spawnPositions.Count} obstacles.");

                    break;
                }
            }

            foreach (var position in spawnPositions)
            {
                var obstacle = _poolService.Spawn<Obstacle>(
                    position,
                    Quaternion.identity);

                _obstacles.Add(obstacle);
            }
        }

        public void Clear()
        {
            foreach (var obstacle in _obstacles)
            {
                if (obstacle != null)
                    _poolService.Despawn(obstacle);
            }

            _obstacles.Clear();
        }

        private bool TryGetVisibleBounds(
            Camera camera,
            out float minX,
            out float maxX)
        {
            minX = 0f;
            maxX = 0f;

            var plane = new Plane(Vector3.up, Vector3.zero);

            var screenPoints = new[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(Screen.width, 0f, 0f),
                new Vector3(0f, Screen.height, 0f),
                new Vector3(Screen.width, Screen.height, 0f)
            };

            var hasIntersection = false;

            foreach (var screenPoint in screenPoints)
            {
                var ray = camera.ScreenPointToRay(screenPoint);

                if (!plane.Raycast(ray, out var distance))
                    continue;

                var worldPoint = ray.GetPoint(distance);

                if (!hasIntersection)
                {
                    minX = worldPoint.x;
                    maxX = worldPoint.x;
                    hasIntersection = true;
                }
                else
                {
                    minX = Mathf.Min(minX, worldPoint.x);
                    maxX = Mathf.Max(maxX, worldPoint.x);
                }
            }

            return hasIntersection;
        }

        private bool IsPositionVisible(
            Camera camera,
            Vector3 position)
        {
            var viewportPosition = camera.WorldToViewportPoint(position);

            return viewportPosition.z > 0f &&
                   viewportPosition.x >= 0f &&
                   viewportPosition.x <= 1f &&
                   viewportPosition.y >= 0f &&
                   viewportPosition.y <= 1f;
        }

        private bool IsPositionValid(
            Vector3 position,
            List<Vector3> existingPositions,
            float minDistanceSqr)
        {
            foreach (var existingPosition in existingPositions)
            {
                if ((position - existingPosition).sqrMagnitude < minDistanceSqr)
                    return false;
            }

            return true;
        }
    }
}