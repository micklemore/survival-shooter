using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageObject
{
    public float DamageAmount => damageAmount;
    float damageAmount;

    public Vector3 PushDirection => pushDirection;
    Vector3 pushDirection;

    public float PushForce => pushForce;
    float pushForce;

    public float TimerUntilNextShoot => timerUntilNextShoot;
    float timerUntilNextShoot;


    public DamageObject(float damageAmount, Vector3 pushDirection, float pushForce, float timerUntilNextPushback)
    {
        this.damageAmount = damageAmount;
        this.pushDirection = pushDirection;
        this.pushForce = pushForce;
        this.timerUntilNextShoot = timerUntilNextPushback;
    }
}
