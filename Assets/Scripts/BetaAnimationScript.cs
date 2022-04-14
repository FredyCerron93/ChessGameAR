using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(Piece))]
public class BetaAnimationScript : MonoBehaviour
{
    private Animator anim;
    bool isDone;
    //private Piece piece;
    [SerializeField] private Piece piece;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Console.Write(piece.hasMoved);

        if (Input.GetKeyDown(KeyCode.W)) {

            anim.CrossFade("Death", 0.1f);
        }





        if (piece.ded == true)
        {
            anim.Play("Death");


            StartCoroutine(DelayedDead(anim.GetCurrentAnimatorStateInfo(0).length));
        }

        if (piece.at == true)
        {
            anim.Play("Attack");


            StartCoroutine(DelayedAttack(anim.GetCurrentAnimatorStateInfo(0).length));
        }



        IEnumerator DelayedAttack(float _delay = 0)
        {
            yield return new WaitForSeconds(_delay);
            piece.at = false;
        }




        IEnumerator DelayedDead(float _delay = 0)
        {
            yield return new WaitForSeconds(_delay);
            piece.ded = false;
        }



        if (piece.anim == true)
        {

            
            //if (!isDone)
            //{
            //    anim.CrossFade("Walking", 0.1f);
            //    isDone = true;
            //}

            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
            anim.SetBool("isAttacking", false);
        }
       
        else if (piece.at == true)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isAttacking", true);

        }
        else 
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
            anim.SetBool("isAttacking", false);

        }


    }
}
