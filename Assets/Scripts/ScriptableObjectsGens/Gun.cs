using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string name;
    public float firerate;
    public float aimSpeed;
    public float bloom;
    public float recoil;
    public float kickback;
    public GameObject prefab;
}
