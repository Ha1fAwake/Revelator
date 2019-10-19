using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "MyScripatbleObjects/Enemy")]
public class EnemyDataContainer : ScriptableObject
{
    public string enemyName;
    public string description;
    public int attack;
    public int health;
    public Sprite artwork;
}
