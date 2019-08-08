using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArwingCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider c)

    {
        print("Player collided with" + c.gameObject.name);
    }

    void OnTriggerExit(Collider c)

    {
        print("Player stopped colliding with" + c.gameObject.name);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
