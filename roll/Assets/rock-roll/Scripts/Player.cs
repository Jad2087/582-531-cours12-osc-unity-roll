using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
 
public class Player : MonoBehaviour
{
    public float torqueForce = 1f;
    public float jumpForce = 5f;
 
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;
 
    private Rigidbody2D rb;
 
    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;
 
 
 
    private int etatEnMemoire = 1;
    private int etatEnMemoire2 = 1;// Le code initalise l'état initial du bouton comme relâché
 
      private void maLectureKey(OSCMessage message)
    {
 
        // Si le message n'a pas d'argument ou l'argument n'est pas un Int on l'ignore
        if (message.Values.Count == 0)
        {
            Debug.Log("No value in OSC message");
            return;
        }
 
        if (message.Values[0].Type != OSCValueType.Int)
        {
            Debug.Log("Value in message is not an Int");
            return;
           
        }
 
           int nouveauEtat = message.Values[0].IntValue; // REMPLACER ici les ... par le code qui permet de récuérer la nouvelle donnée du flux
        if (etatEnMemoire != nouveauEtat)
        { // Le code compare le nouvel etat avec l'etat en mémoire
            etatEnMemoire = nouveauEtat; // Le code met à jour l'état mémorisé
            if (nouveauEtat == 1)
            {
                if (IsGrounded())
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
 
                        var oSCMessage = new OSCMessage("/color");  // CHANGER l'adresse /pixel pour l'adresse désirée
                        oSCMessage.AddValue( OSCValue.Int(0) ); // Ajoute l'entier 255
                        oSCMessage.AddValue( OSCValue.Int(0) ); // Ajoute un autre 255
                        oSCMessage.AddValue( OSCValue.Int(0) ); // Ajoute un troisième 255
                        oscTransmitter.Send(oSCMessage);
                       
                }  
               
               
            }
            else if (nouveauEtat == 0)
            {
                     var oSCMessage = new OSCMessage("/color");  // CHANGER l'adresse /pixel pour l'adresse désirée
                        oSCMessage.AddValue( OSCValue.Int(250) ); // Ajoute l'entier 255
                        oSCMessage.AddValue( OSCValue.Int(0) ); // Ajoute un autre 255
                        oSCMessage.AddValue( OSCValue.Int(0) ); // Ajoute un troisième 255
                        oscTransmitter.Send(oSCMessage);
            }
        }
 
    }
 
    private void maLectureAnalogique(OSCMessage message)
    {
 
        // Si le message n'a pas d'argument ou l'argument n'est pas un Int on l'ignore
        if (message.Values.Count == 0)
        {
            Debug.Log("No value in OSC message");
            return;
        }
 
        if (message.Values[0].Type != OSCValueType.Int)
        {
            Debug.Log("Value in message is not an Int");
            return;
        }
 
 
        int AngleValue = message.Values[0].IntValue; // REMPLACER ici les ... par le code qui permet de récuérer la nouvelle donnée du flux
 
            if (AngleValue > 0)
            {
                rb.AddTorque(-torqueForce); // clockwise
           
            }
            else if (AngleValue < 0)
            {
                rb.AddTorque(torqueForce); // counter-clockwise
         
            }
 
    }
 
 
 
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
 
        oscReceiver.Bind("/but", maLectureKey);
        oscReceiver.Bind("/angle", maLectureAnalogique);
        if (oscReceiver == null) return;
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
 
