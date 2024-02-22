using UnityEngine;
using UnityEngine.Serialization;


public struct PackedMovementMode
{
   public enum EMovementModes
   {
      Walking,
      Idle,
      Dashing
   }
   public PackedMovementMode(bool bCanWalk, bool bCanDash, EMovementModes currentMovementState)
   {
      this.bCanWalk = bCanWalk;
      this.bCanDash = bCanDash;
      this.MovementState = currentMovementState;
   }

   public bool bCanWalk;
   public bool bCanDash;
   public EMovementModes MovementState;
}
