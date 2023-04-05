using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField] float speed;
    Transform mob_transform;

    // Start is called before the first frame update
    void Start()
    {
        mob_transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        mob_transform.position += mob_transform.TransformDirection(Vector3.left) * speed;
    }
}
