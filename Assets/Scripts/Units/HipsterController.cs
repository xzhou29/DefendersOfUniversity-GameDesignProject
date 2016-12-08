using UnityEngine;
using System.Collections;

public class HipsterController : MonoBehaviour {

    private Renderer render;
    public bool cantBeHurt;
    public Color shieldColor = Color.red;
    public Color normalColor = Color.white;

    // Use this for initialization
    void Awake ()
    {
        cantBeHurt = true;
        render = GetComponent<Renderer>(); 
    }

    // Update is called once per frame
    void Update ()
    {
            StartCoroutine(Shield());     
    }

    IEnumerator Shield()
    {
        for(int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(4f);
            render.material.color = shieldColor;
            cantBeHurt = false;

            yield return new WaitForSeconds(2f);
            render.material.color = normalColor;
            cantBeHurt = true;

        }
    }
}
