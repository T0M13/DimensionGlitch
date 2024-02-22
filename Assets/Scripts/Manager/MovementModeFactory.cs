using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementModeFactory 
{

    public static PackedMovementMode GetDashMovementMode()
    {
        return new PackedMovementMode(false, false, PackedMovementMode.EMovementModes.Dashing);
    }

    public static PackedMovementMode GetDefaultMovementMode()
    {
        return new PackedMovementMode(true, true, PackedMovementMode.EMovementModes.Idle);
    }
    public static PackedMovementMode GetWalkingMovementMode()
    {
        return new PackedMovementMode(true, true, PackedMovementMode.EMovementModes.Walking);
    }
}
