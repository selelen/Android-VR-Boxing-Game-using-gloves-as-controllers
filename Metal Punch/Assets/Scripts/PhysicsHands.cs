using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHand : MonoBehaviour
{
     public Transform trackedTransform = null;
     public Rigidbody body = null;

     public float positionStrength = 15;

     void FixedUpdate()
     {
          var vel = (trackedTransform.position - body.position).normalized * positionStrength * Vector3.Distance(trackedTransform.position, body.position);
          body.velocity = vel;
     }
}