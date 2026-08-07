using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

// NOTE
// 책임_1 몇 마리를 맵에 스폰 시킬 건지
// 책임_2 맵의 동물들의 비율을 어떻게 만들 건지
public sealed class FieldObjectAnimalSpawner : MonoBehaviour
{
    private sealed class SpawnPositionProvider
    {
        // TODO
        // 아직 Manager로 부터 받지 않음.
        private const int TEST_SPAWN_DATA_COUNT = 50;
        private const float TEST_POSITION_Y = 0.5f;
        private const int TEST_POSITION_MIN = 5;
        private const int TEST_POSITION_RANGE = 60;

        private readonly int _spawnPositionCount;
        private readonly List<Vector3> _spawnPositionList = new(TEST_SPAWN_DATA_COUNT);
        private readonly List<Vector3> _spawnRotationList = new(TEST_SPAWN_DATA_COUNT);

        public int SpawnPositionCount => _spawnPositionCount;

        public SpawnPositionProvider(int spawnPositionCount)
        {
            _spawnPositionCount = Mathf.Clamp(spawnPositionCount, 0, TEST_SPAWN_DATA_COUNT);

            InitializeTestSpawnData();
        }

        public bool TryGetSpawnPositionAndRotation(int index, out Vector3 position, out Vector3 rotation)
        {
            if (index < 0 || index >= _spawnPositionCount ||
                index >= _spawnPositionList.Count || index >= _spawnRotationList.Count)
            {
                position = default;
                rotation = default;
                return false;
            }

            position = _spawnPositionList[index];
            rotation = _spawnRotationList[index];
            return true;
        }

        private void InitializeTestSpawnData()
        {
            for (var idx = 0; idx < TEST_SPAWN_DATA_COUNT; idx++)
            {
                var positionX = TEST_POSITION_MIN + idx * 17 % TEST_POSITION_RANGE;
                var positionZ = TEST_POSITION_MIN + idx * 29 % TEST_POSITION_RANGE;
                var rotationY = idx * 47 % 360;

                _spawnPositionList.Add(new Vector3(positionX, TEST_POSITION_Y, positionZ));
                _spawnRotationList.Add(new Vector3(0f, rotationY, 0f));
            }
        }
    }
    #region 1. Fields

    private SpawnPositionProvider _spawnPositionProvider;

    [SerializeField] [Tooltip("초기에 스폰되는 모든 동물의 개수")]
    private int _spawnAnimalsCount;

    [SerializeField] private SerializedDictionary<EFieldObject, FieldObjectAnimalBase> _animalPrefabDict = new();
    [SerializeField] private SerializedDictionary<EFieldObject, int> _animalSpawnWeightDict = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Start()
    {
        _spawnPositionProvider = new SpawnPositionProvider(_spawnAnimalsCount);
        
        SpawnAnimals();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Methods

    //


    // NOTE
    // 가중치를 통해 동물별로 스폰 개수를 구현
    private Dictionary<EFieldObject, int> CalculateSpawnCountsByWeight(int totalSpawnCount)
    {
        if (totalSpawnCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSpawnCount), totalSpawnCount,
                "Total spawn count cannot be negative.");
        }

        if (_animalSpawnWeightDict == null || _animalSpawnWeightDict.Count == 0)
        {
            throw new InvalidOperationException("Animal spawn settings are not configured.");
        }

        long totalWeight = 0;

        foreach (var animalSpawnWeightPair in _animalSpawnWeightDict)
        {
            if (animalSpawnWeightPair.Value <= 0)
            {
                throw new InvalidOperationException(
                    $"Animal spawn weight must be greater than zero. Key: {animalSpawnWeightPair.Key}");
            }

            totalWeight += animalSpawnWeightPair.Value;
        }

        var spawnCountDict = new Dictionary<EFieldObject, int>(_animalSpawnWeightDict.Count);
        var remainderList = new List<(EFieldObject Key, double Remainder)>(_animalSpawnWeightDict.Count);
        var allocatedSpawnCount = 0;

        foreach (var animalSpawnWeightPair in _animalSpawnWeightDict)
        {
            var exactSpawnCount =
                (double)totalSpawnCount * animalSpawnWeightPair.Value / totalWeight;
            var spawnCount = (int)Math.Floor(exactSpawnCount);

            spawnCountDict.Add(animalSpawnWeightPair.Key, spawnCount);
            remainderList.Add((animalSpawnWeightPair.Key, exactSpawnCount - spawnCount));
            allocatedSpawnCount += spawnCount;
        }

        remainderList.Sort((left, right) =>
        {
            var remainderComparison = right.Remainder.CompareTo(left.Remainder);
            return remainderComparison != 0
                ? remainderComparison
                : left.Key.CompareTo(right.Key);
        });

        var remainingSpawnCount = totalSpawnCount - allocatedSpawnCount;

        for (var idx = 0; idx < remainingSpawnCount; idx++)
        {
            var fieldObjectKey = remainderList[idx].Key;
            spawnCountDict[fieldObjectKey]++;
        }

        return spawnCountDict;
    }

    // Note
    // Pooling 고려 없음.  ->  로딩 씬에서 해결
    private void SpawnAnimals()
    {
        var spawnCountDict = CalculateSpawnCountsByWeight(_spawnAnimalsCount);
        var spawnPositionIndex = 0;

        foreach (var pair in spawnCountDict)
        {
            var eFieldObject = pair.Key;
            var spawnCount = pair.Value;
            var animalPrefab = _animalPrefabDict[eFieldObject];

            for (var count = 0; count < spawnCount; count++)
            {
                var currentSpawnPositionIndex = spawnPositionIndex;
                spawnPositionIndex++;

                if (!_spawnPositionProvider.TryGetSpawnPositionAndRotation(
                        currentSpawnPositionIndex, out var position, out var rotation))
                {
                    Debug.LogWarning(
                        $"Spawn position or rotation at index {currentSpawnPositionIndex} is not set.",
                        this);
                    {
                        continue;
                    }
                }

                Instantiate(animalPrefab, position, Quaternion.Euler(rotation));
            }
        }
    }

    #endregion
}
