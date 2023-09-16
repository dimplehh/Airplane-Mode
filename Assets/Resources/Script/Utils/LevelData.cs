using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField] public float[] time;
    [SerializeField] public float[] speed;
    [SerializeField] public float[] shotDelay;
}
