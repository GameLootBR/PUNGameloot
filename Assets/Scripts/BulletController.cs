using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //--------------------------------------------------------
    void Start()
    {
        Destroy(gameObject, 3);
    }

    //--------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("BazucaObject"))
        {
            Destroy(other.gameObject);
        } 
        else if (other.name.Contains("PlayerCube"))
        {
            other.GetComponent<PlayerController>().Died();
        }

        Destroy(gameObject);
    }

    //--------------------------------------------------------
    public void Shoot(float lag)
    {
        Rigidbody rbody = GetComponent<Rigidbody>();
        rbody.velocity = transform.forward * 10;
        rbody.position += rbody.velocity * lag;
    }

}
