using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] float health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f; //ranges from 0 to 1
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f; //ranges from 0 to 1

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;

    float yMin;
    float yMax;
    

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }



    // Update is called once per frame
    void Update()
    {
        // movement should happen in Update

        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D otherThing)
    {
        DamageDealer damageDealer = otherThing.gameObject.GetComponent<DamageDealer>();

        if(! damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume); //play it at the camera
    }

    public float GetHealth()
    {
        return health;
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1")) // for when pushing the button
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        if(Input.GetButtonUp("Fire1"))
        {
            //StopAllCoroutines();
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            //want laser to start where player is
            GameObject laser = Instantiate(laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject; //Quaternion means no rotation
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        // means we can now use the left and right buttons to move the ship

        //matches up to Edit -> Project Settings -> Input in Unity
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        //Debug.Log(deltaX); //goes from -1 to 1

        //wanting to clamp our player so that it can't go beyond the min and max

        var newXPosition = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPosition, newYPosition);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        //min position for x is 0, max is 1

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding; // y and z values don't matter here as we are not accessing them 
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
