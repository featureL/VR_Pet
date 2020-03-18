using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int _Health { get;  private set; }
    void Start()
    {
        _Health = 100;
    }

    public void TakeDamage(int damage)
    {
        if (damage > _Health)
            _Health = 0;
        _Health -= damage;
    }

}
