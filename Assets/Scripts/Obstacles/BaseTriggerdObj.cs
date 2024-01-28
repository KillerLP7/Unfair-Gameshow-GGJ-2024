using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTriggerdObj : MonoBehaviour, ITriggerdObstacle
{
    public abstract void TriggerEffect();
}
