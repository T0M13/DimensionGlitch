using UnityEngine;


public struct PackedMovementMode
{
   public enum EMovementModes
   {
      Walking,
      Idle,
      Dashing
   }
   public PackedMovementMode(bool bCanWalk, bool bCanDash, EMovementModes CurrentMovementMode)
   {
      this.bCanWalk = bCanWalk;
      this.bCanDash = bCanDash;
      this.MovementMode = CurrentMovementMode;
   }

   public bool bCanWalk;
   public bool bCanDash;
   public EMovementModes MovementMode;
}
