using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    float dirX, dirY;
    public float movementVal;
    Rigidbody rb;
    public float grounDistance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxis("Horizontal") * movementVal;
        dirY = Input.GetAxis("Vertical") * movementVal;
    }

    void FixedUpdate()
    {
        if(IsGround() && GameManager.gameStatus == 1)
        rb.velocity = new Vector3(dirX,0,dirY);
    }

    bool IsGround()
    {
       return Physics.Raycast(transform.position, Vector3.down, grounDistance);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("cube"))
        {
            Material mat = collider.gameObject.GetComponent<MeshRenderer>().material;
            GameManager._instance.GetScore(mat);
            Destroy(collider.gameObject);
        }
    }
}
