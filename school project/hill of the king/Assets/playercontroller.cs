using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class playercontroller : NetworkBehaviour
{
    Animator Anima;
    [SyncVar]
    int health;

    

    public GameObject hit;
    public bool hitanima;
    public int playernum;
    GameObject[] hitcheck;
    // Start is called before the first frame update
    private void Awake()
    {
        Anima = GetComponent<Animator>();
        hit.SetActive(false);
    }

    void Update()
    {
        Vector3 vec3=new Vector3(2, 0, 0);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += vec3*Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cmdattack();
        }
        if (Anima.GetCurrentAnimatorStateInfo(0).length <= Anima.GetCurrentAnimatorStateInfo(0).normalizedTime && hitanima == true)
        {
            //Cmdresetattack();
            Anima.SetBool("Hit", false);
            hitanima = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("hit"))
        {
            if (collision.gameObject.GetComponent<hitmanager>().playernum == playernum)
                return;

            health -= collision.gameObject.GetComponent<hitmanager>().damage;
            Debug.Log("hit! " + health);
            collision.gameObject.SetActive(false);

        }
    }
    void Cmdattack()
    {
        Anima.SetBool("Hit", true);
        hitanima = true;
    }
    /*[Command]
    void Cmdresetattack()
    {
        Anima.SetBool("Hit", false);
        hitanima = false;
    }*/
}
