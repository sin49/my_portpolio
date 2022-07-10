using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_bullet_type3 : MonoBehaviour//보스2 특수 패턴
{
    public float RotateSpeed = 4f;
    public float Radius = 0.1f;
    public float RadiusAdd = 0.08f;
    public float Zpos = 10;
    private Vector3 _centre;
    private float _angle;
    public float spawn_bullet_time;
    public GameObject explosion_bullet;

    private void Start()
    {
        _centre = transform.position;
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        transform.position = pos;
    }

    private void Update()
    {
        spawn_bullet_time += Time.deltaTime;
        if (transform.position.x < -14)
        {
            Destroy(this.gameObject);
        }
        
        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset;
        Radius+=RadiusAdd;

        if (spawn_bullet_time >= 0.1)
        {
            GameObject bullet = Instantiate(explosion_bullet, transform.position, transform.rotation);
            spawn_bullet_time = 0;
        }
    }
}
