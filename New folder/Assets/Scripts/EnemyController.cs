using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator _anim;
    bool dead = false;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        
        if (other.transform.CompareTag("Player"))
        {
            dead = true;
            _anim.SetBool("IsDead", true);
            _anim.SetBool("Alive", false);
        }
        else
        {
            dead = false;
            _anim.SetBool("IsDead", false);
            _anim.SetBool("Alive", true);
        }
    }
}
