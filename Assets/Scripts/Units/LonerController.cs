using UnityEngine;
using System.Collections;

public class LonerController : MonoBehaviour {

    public bool isInvisible;
    private Renderer render;
	// Use this for initialization
	void Start () {
        render = GetComponent<Renderer>();
        isInvisible = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (isInvisible)
            render.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "ArchitectureTower")
        {
            
            render.enabled = true;
            isInvisible = false;         
        }
    }

    void OnTriggerStay2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "ArchitectureTower")
        {
            render.enabled = true;
            isInvisible = false;
        }
    }

    void OnTriggerExit2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "ArchitectureTower")
        {
            isInvisible = true;
        }
    }
}
