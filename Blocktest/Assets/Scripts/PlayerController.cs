using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    /// Move speed of the player
    [SerializeField] float moveSpeed = 1;
    /// Jump strength of the player
    [SerializeField] float jumpStrength = 7.5f;
    [SerializeField] LayerMask groundLayer;
    /// Rigidbody component of the player
    private Rigidbody2D playerRB;

    private void Start() {
        playerRB = GetComponent<Rigidbody2D>();
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) { return; } // Don't execute if not the linked person

        float horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput != 0) 
        {
            transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerRB.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
    }
}
