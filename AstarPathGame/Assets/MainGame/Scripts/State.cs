using System;
using UnityEngine;

namespace MainGame
{
    [Flags]
    public enum AgentStateKey
    {
        None = 0,
        TargetInSight = 1,
        TargetOutOfSight = 2,
        AgentInSight = 4,
        AgentOutOfSight = 8,
        TargetOutOfRange = 16,
        TargetInRange = 32,
        CanWalk = 64,
        CanNotWalk = 128,
        EnemyIsAlive = 256,
        EnemyIsDead = 512,
        IsNotHungry = 1024,
        AreaExplored = 2048
    }
}
