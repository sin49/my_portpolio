using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchSpawner : MonoBehaviour
{
    public GameObject door;
    public GameObject spawn;
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") //�÷��̾ ������ �浹���� ��
        {
            if (GameObject.Find("spawn")) //���ȿ� �����ʰ� ������
            {
                door.SetActive(true); //���� Ȱ��ȭ
            }
        }
    }



}
