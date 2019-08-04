using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class Testing : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;

    [Header("Properties")]
    [SerializeField] private int _spawnCount;
    [SerializeField] private float _levelTop;
    [SerializeField] private float _levelBottom;
    [SerializeField] private Vector2Int _levelBorders;

    protected void Start()
    {
        var entityManager = World.Active.EntityManager;

        var entityArchetype = entityManager.CreateArchetype(
            typeof(LevelComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(MoveSpeedComponent)
        );

        var entityArray = new NativeArray<Entity>(_spawnCount, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);

        var zPos = 0;
        var xStart = _levelBorders.x;
        var xEnd = _levelBorders.y;
        var iterator = 0;

        for (int i = 0; i < entityArray.Length; i++)
        {
            var entity = entityArray[i];

            // setup start level
            entityManager.SetComponentData(entity, new LevelComponent
            {
                LevelTop = _levelTop,
                LevelBottom = _levelBottom
            });

            // setup start speed
            entityManager.SetComponentData(entity, new MoveSpeedComponent()
            {
                MoveSpeed = Random.Range(1.0f, 3.0f)
            });

            var xPos = xStart + iterator;

            // setup start position
            entityManager.SetComponentData(entity, new Translation()
            {
                Value = new float3(
                    xPos,
                    Random.Range(_levelBottom, _levelTop),
                    zPos)
            });

            // setup start material
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });

            iterator++;

            if (xPos == xEnd)
            {
                iterator = 0;
                zPos++;
            }
        }

        entityArray.Dispose();
    }
}
