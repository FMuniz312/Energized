using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement 
{
    Vector3 GetMovingDirection();
    void DashMove(Vector3 dir, float force);

}
