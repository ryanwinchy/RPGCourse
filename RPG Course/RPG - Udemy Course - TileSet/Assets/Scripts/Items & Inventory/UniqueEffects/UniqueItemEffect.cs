using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UniqueItemEffect : ScriptableObject       //This is a scriptable object, must inherit from scriptable object.
{

    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("Effect executed!");
    }



}
