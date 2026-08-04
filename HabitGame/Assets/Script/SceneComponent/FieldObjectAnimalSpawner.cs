using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public sealed class FieldObjectAnimalSpawner : MonoBehaviour
{
    private sealed class SpawnPositionProvider
    {
        // TEST
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
    [SerializeField, Range(0, 50)] private int _spawnPositionCount = 50;
    [SerializeField] private SerializedDictionary<EFieldObject, FieldObjectAnimalBase> _animalPrefabDict = new();

    #endregion

    #region 2. Properties

    //

    #endregion

    #region 3. Constructor

    private void Start()
    {
        _spawnPositionProvider = new SpawnPositionProvider(_spawnPositionCount);
        SpawnAnimals();
    }

    #endregion

    #region 4. EventHandlers

    //

    #endregion

    #region 5. Request Methods

    //

    #endregion

    #region 6. Methods

    // Note
    // Pooling 고려 없음.  ->  로딩 씬에서 해결
    private void SpawnAnimals()
    {
        for (var idx = 0; idx < _spawnPositionProvider.SpawnPositionCount; idx++)
        {
            if (!_spawnPositionProvider.TryGetSpawnPositionAndRotation(
                    idx, out var position, out var rotation))
            {
                Debug.LogWarning($"Spawn position or rotation at index {idx} is not set.", this);
                continue;
            }

            Instantiate(_animalPrefabDict[EFieldObject.SPARROW], position, Quaternion.Euler(rotation));
        }
    }

    #endregion
}
