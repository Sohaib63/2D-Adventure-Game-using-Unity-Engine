using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;
    public float grater = 38f;
    public float lower = 32.3f;
    bool left = true;
    public Vector2 direction = Vector2.left;

    private new Rigidbody2D rigidbody;
    private Vector2 velocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        #if UNITY_EDITOR
            enabled = !EditorApplication.isPaused;
        #else
            enabled = true;
        #endif
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (enabled)
        {
            if (left)
            {
                transform.Translate(-Vector2.right * speed * Time.deltaTime);

            }
            else
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);

            }
            if (transform.position.x > grater)
            {
                left = true;

                GetComponent<SpriteRenderer>().flipX = true;

                // transform.localEulerAngles = Vector3.zero;

            }
            else if (transform.position.x < lower)
            {
                left = false;
                GetComponent<SpriteRenderer>().flipX = false;

                // transform.localEulerAngles = new Vector3(0f, 180f, 0f);

            }

        }

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == TagManager.BULLET_TAG)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.tag == TagManager.BULLET_TAG)
        {
            Destroy(gameObject);
        }
    }
}