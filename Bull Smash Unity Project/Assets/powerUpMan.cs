using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpMan : MonoBehaviour
{

    [SerializeField] private int _flyTime;
    [SerializeField] private int _wallTime;
    private Rigidbody rb;

    [SerializeField] private PowerUpManager _powerUpManager;

    [SerializeField] private GameObject _wings;

    private bool holdingItem;
    private int itemIndex = 0;
    private GameObject overheadItem;
    [SerializeField] private Transform overheadItemPos;
    [SerializeField] Color playerColour;
    [SerializeField] GameObject wallPrefab;

    public Component [] wallRenderers;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // populate renderes list
        if(wallPrefab != null)
        {
            wallRenderers = wallPrefab.GetComponentsInChildren(typeof(Renderer));
        }
    }


    


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fly" && !holdingItem)
        {
            holdingItem = true;
            itemIndex = 1;
            other.gameObject.SetActive(false);
            GameObject miniItem = _powerUpManager.indexToOverheadItem(0);
            overheadItem = Instantiate(miniItem, overheadItemPos);
            overheadItem.transform.localScale += new Vector3(-0.35f, -0.35f, -0.35f);
            overheadItem.GetComponent<BoxCollider>().enabled = false;
            Invoke("PowerUpManagerCaller", 10);
        }

        if(other.tag == "wall" && !holdingItem)
        {
            holdingItem = true;
            itemIndex = 2;
            other.gameObject.SetActive(false);
            GameObject miniItem = _powerUpManager.indexToOverheadItem(1);
            overheadItem = Instantiate(miniItem, overheadItemPos);
            //overheadItem.transform.localScale += new Vector3(-0.35f, -0.35f, -0.35f);
            overheadItem.GetComponent<BoxCollider>().enabled = false;
            Invoke("PowerUpManagerCaller", 10);
        }
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") == -1 && holdingItem == true)
        {
            if (itemIndex == 1)
            {
                itemIndex = 0;
                holdingItem = false;
                Destroy(overheadItem);
                StartCoroutine("Fly", _flyTime);
            }

            if (itemIndex == 2)
            {
                itemIndex = 0;
                holdingItem = false;
                Destroy(overheadItem);
                StartCoroutine("SpawnWalls", _wallTime);
            }
        }
    }

    IEnumerator Fly(float t)
    {
        _wings.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        yield return new WaitForSeconds(t);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _wings.SetActive(false);
        
    }

    IEnumerator SpawnWalls(float t)
    {
        wallPrefab.SetActive(true);

        if (gameObject.GetComponent<moveScript>().Player1)
        {
            gameObject.layer = 8;
           foreach(Renderer renderer in wallRenderers)
            {
                renderer.material.color = playerColour;
            }
        }
        else if (gameObject.GetComponent<moveScript>().Player2)
        {
            gameObject.layer = 8;
            foreach (Renderer renderer in wallRenderers)
            {
                renderer.material.color = playerColour;
            }
        }
        yield return new WaitForSeconds(t);

        wallPrefab.SetActive(false);
        if (gameObject.GetComponent<moveScript>().Player1)
        {
            gameObject.layer = 0;
        }
        else if (gameObject.GetComponent<moveScript>().Player2)
        {
            gameObject.layer = 0;
        }

    }

    private void PowerUpManagerCaller()
    {
        _powerUpManager.NewPowerUp();
    }
}
