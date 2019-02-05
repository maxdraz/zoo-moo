using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpMan : MonoBehaviour
{

    [SerializeField] private int _flyTime;
    private Rigidbody rb;

    [SerializeField] private PowerUpManager _powerUpManager;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fly")
        {
            StartCoroutine("Fly", _flyTime);
            other.gameObject.SetActive(false);

        }
    }

    IEnumerator Fly(float t)
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        yield return new WaitForSeconds(t);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        Invoke("PowerUpManagerCaller", 10);
    }

    private void PowerUpManagerCaller()
    {
        _powerUpManager.NewPowerUp();
    }
}
