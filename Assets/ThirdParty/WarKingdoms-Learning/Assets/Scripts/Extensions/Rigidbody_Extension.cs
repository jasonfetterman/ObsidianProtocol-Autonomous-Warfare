using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class adds some extension methods for Rigidbody and Rigidbody2D
/// </summary>
public static class Rigidbody_Extension
{
    /// <summary>
    /// Stops physic Updates for a rigidbody
    /// </summary>
    /// <param name="rigidbody2D"></param>
    /// <param name="kinematic"></param>
    public static void Freeze(this Rigidbody2D rigidbody2D, bool kinematic = true)
    {
        rigidbody2D.linearVelocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;

        rigidbody2D.bodyType = kinematic
            ? RigidbodyType2D.Kinematic
            : RigidbodyType2D.Dynamic;
    }

    /// <summary>
    /// Stops physic Updates for a rigidbody
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="kinematic"></param>
    public static void Freeze(this Rigidbody rigidbody, bool kinematic = true)
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = kinematic;
    }
}