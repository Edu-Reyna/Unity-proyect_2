using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDist;
    public Animator animator;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;

        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null) 
            { 
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0, y);
        rb.velocity = moveDir * speed;

        float currentSpeed = rb.velocity.magnitude;

        animator.SetFloat("movement", currentSpeed);

        if (x != 0 && x < 0)
        {
            sr.flipX = true;

        }
        else if (x != 0 && x > 0) 
        {
            sr.flipX = false;
        }

        Atacar();
    }

    private void Atacar()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("ActionTrigger");
        }
    }

}
