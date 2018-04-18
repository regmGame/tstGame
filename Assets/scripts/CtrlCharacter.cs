using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlCharacter : MonoBehaviour
{
    /*
     * PlayerController
     */
    public float velocidadMovimiento = 10.0f;
    private Rigidbody2D rb;
    private bool isgrounded = true;
    private bool isWhatchingRight = true;

    /*
     * FireController
     */
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity =20.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         *La siguiente linea permite disparar a izquierda o derecha en base a el sprite "BulletOrigin".
         */
        bulletSpawn = GameObject.Find("BulletOrigin").transform;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
        /*
         *Control de salto.
         */
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.A) && isgrounded)
        {
            rb.AddForce(new Vector2(-2000, 1500));
        }
        else if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.D) && isgrounded)
        {
            rb.AddForce(new Vector2(2000, 1500));
        }
        else if (Input.GetKey(KeyCode.Space) && isgrounded)
        {
            rb.AddForce(new Vector2(0, 1500));
        }

        /*
         *Control de movimiento.
         */
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {

        }
        else if (Input.GetKey(KeyCode.A))
        {
            isWhatchingRight = false;
            rb.velocity = new Vector2(-velocidadMovimiento, 0);
            Vector3 rotationPlayer = rb.transform.eulerAngles;
            rotationPlayer.y = -180.0f;
            rb.transform.eulerAngles = rotationPlayer;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            isWhatchingRight = true;
            rb.velocity = new Vector2(velocidadMovimiento, 0);
            Vector3 rotationPlayer = rb.transform.eulerAngles;
            rotationPlayer.y = 0f;
            rb.transform.eulerAngles = rotationPlayer;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        Camera.main.transform.position = new Vector3(rb.position.x, 0, -10);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Terreno")
        {
            isgrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        isgrounded = false;
    }

    void Fire()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletVelocity;
        Destroy(bullet, 2.0f);
    }
}
