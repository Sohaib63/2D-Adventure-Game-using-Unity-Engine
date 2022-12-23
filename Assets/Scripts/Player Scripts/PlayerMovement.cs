using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : CharacterMovement
{
    private float moveX, moveY;
    public Joystick joystick;
    private Camera mainCam;

    private Vector2 mousePosition;
    private Vector2 direction;
    private Vector3 tempScale;

    private Animator anim;

    private PlayerWeaponManager playerWeaponManager;

    protected override void Awake()
    {
        base.Awake();

        mainCam = Camera.main;

        anim = GetComponent<Animator>();

        playerWeaponManager = GetComponent<PlayerWeaponManager>();
    }

    private void FixedUpdate()
    {
        // moveX = Input.GetAxisRaw(TagManager.HORIZONTAL_AXIS);
        // moveY = Input.GetAxisRaw(TagManager.VERTICAL_AXIS);
        moveX=joystick.Horizontal;
        moveY=joystick.Vertical;

        HandlePlayerTurning();

        HandleMovement(moveX, moveY);
    }

    void HandlePlayerTurning()
    {
        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        
        direction = new Vector2(mousePosition.x - transform.position.x, 
            mousePosition.y - transform.position.y).normalized;

        HandlePlayerAnimation(direction.x, direction.y);
    
    }

    void HandlePlayerAnimation(float x, float y)
    {

        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        tempScale = transform.localScale;

        if (x > 0)
            tempScale.x = Mathf.Abs(tempScale.x);
        else if(x < 0)
            tempScale.x = -Mathf.Abs(tempScale.x);
        
        transform.localScale = tempScale;

        x = Mathf.Abs(x);

        anim.SetFloat(TagManager.FACE_X_ANIMATION_PARAMETER, x);
        anim.SetFloat(TagManager.FACE_Y_ANIMATION_PARAMETER, y);

        ActivateWeaponForSide(x, y);
    }

    void ActivateWeaponForSide(float x, float y)
    {
        // side
        if(x == 1f && y == 0f)
            playerWeaponManager.ActivateGun(0);

        // up
        if(x == 0f && y == 1f)
            playerWeaponManager.ActivateGun(1);

        // down
        if(x == 0f && y == -1f)
            playerWeaponManager.ActivateGun(2);

        // side up
        if(x == 1f && y == 1f)
            playerWeaponManager.ActivateGun(3);

        // side down
        if(x == 1f && y == -1f)
            playerWeaponManager.ActivateGun(4);
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if(target.gameObject.tag == TagManager.ENEMY_TAG)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
} // class
