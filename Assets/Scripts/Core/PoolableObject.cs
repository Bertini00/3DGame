using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    //Setup: add possible variation depending on the object
    public virtual void Setup() { }

    //Clear: return to a default state of the object
    public virtual void Clear() { }
}
