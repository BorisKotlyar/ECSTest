using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MoverSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach(
            (
            ref Translation translation, 
            ref MoveSpeedComponent moveSpeedComponent,
            ref LevelComponent levelComponent
            ) =>
        {
            translation.Value.y += moveSpeedComponent.MoveSpeed * Time.deltaTime;

            // if top bottom -> revert
            if (translation.Value.y > levelComponent.LevelTop)
            {
                moveSpeedComponent.MoveSpeed = -math.abs(moveSpeedComponent.MoveSpeed);
            }

            // if reach bottom -> revert
            if (translation.Value.y < levelComponent.LevelBottom)
            {
                moveSpeedComponent.MoveSpeed = +math.abs(moveSpeedComponent.MoveSpeed);
            }
        });
    }
}
