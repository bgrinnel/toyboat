
using UnityEngine;

public class EnemyType : ScriptableObject
{
    [SerializeField] public GameObject hitEffect;
    [SerializeField] public float initialHealth;
    [SerializeField] public float collisionDamage;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotSpeed;
    [SerializeField] public float defeatedScore;
    [SerializeField] public float comboScoreMod;
}