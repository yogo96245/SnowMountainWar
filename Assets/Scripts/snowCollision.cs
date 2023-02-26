using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter (Collider other) {
        if (other.tag != "player1" && other.tag != "player2")
            Destroy (this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
