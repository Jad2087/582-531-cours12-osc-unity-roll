using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class Player : MonoBehaviour
{
    public OSCReceiver oscReceiver;

    public float torqueForce = 1f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private int etatEnMemoire = 1;
    private int valeurTorque = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Bind OSC
        oscReceiver.Bind("/changement", changementEncodeur);
        oscReceiver.Bind("/saut", etatBouton);
    }

    private void changementEncodeur(OSCMessage message)
{

    int AngleValue = message.Values[0].IntValue;

    int value = AngleValue;
    if (valeurTorque != value)
    {
        valeurTorque = value;
        if (value < 0)
        {
            rb.AddTorque(torqueForce);
                Debug.Log("ca tourne");
            }
        else if (value > 0)
        {
            rb.AddTorque(-torqueForce);
                Debug.Log("ca tourne");
            }
    }
}

    private void etatBouton(OSCMessage message)
    {
        int nouveauEtat = message.Values[0].IntValue;

        if (etatEnMemoire != nouveauEtat)
        {
            etatEnMemoire = nouveauEtat;

            if (nouveauEtat == 0)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        // Roll left/right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddTorque(-torqueForce); // clockwise
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddTorque(torqueForce); // counter-clockwise
        }


    }

    void Update()
    {
        // Jump
        // GetKeyDown() does not work in FixedUpdate()
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    public bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            GetComponent<CircleCollider2D>().radius + extraHeight,
            groundLayer
        );
        return hit.collider != null;
    }
}