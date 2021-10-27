using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAnimation : MonoBehaviour
{
    Animator animator;
    int attackHash = Animator.StringToHash("Attack");
    bool wantAttack = false;
  
  
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void triggerAttack() {
        wantAttack = true;
        animator.SetTrigger(attackHash);
    }
}
