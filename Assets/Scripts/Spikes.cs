using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour, IDamaging
{

    [SerializeField] private float _damage;

    public float Damage => _damage;
}
