using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    [SerializeField] private AudioSource pistolSound;
    [SerializeField] private AudioSource matterSound;
    [SerializeField] private AudioSource laserSound;
    [SerializeField] private AudioSource flamethrowerSound;

    [SerializeField]
    private WeaponManager[] playerWeapons;

    private int weaponIndex;

    [SerializeField]
    private GameObject[] weaponBullets; 

    private Vector2 targetPos;

    private Vector2 direction;

    private Camera mainCam;

    private Vector2 bulletSpawnPosition;

    private Quaternion bulletRotation;

    private BulletPool bulletPool;

    private void Start(){
        dragDistance = Screen.height * 15 / 100;
    }


    private void Awake()
    {
        pistolSound=GetComponent<AudioSource>();
        matterSound=GetComponent<AudioSource>();
        laserSound=GetComponent<AudioSource>();
        flamethrowerSound=GetComponent<AudioSource>();

        weaponIndex = 0;
        playerWeapons[weaponIndex].gameObject.SetActive(true);

        mainCam = Camera.main;

    }

    private void Update()
    {
        ChangeWeapon();
    }

    public void ActivateGun(int gunIndex)
    {
        playerWeapons[weaponIndex].ActivateGun(gunIndex);

    }

    public void ChangeWeapon()
    {   
        if (Input.touchCount == 1){
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) 
            {

                fp = touch.position;
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Ended) 
            {
                lp = touch.position;  
 
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   
                        if ((lp.x > fp.x))  
                        {   //Right swipe
                            playerWeapons[weaponIndex].gameObject.SetActive(false);

                            weaponIndex++;

                            if(weaponIndex == playerWeapons.Length)
                                weaponIndex = 0;

                            playerWeapons[weaponIndex].gameObject.SetActive(true);
                        }
                        else
                        {   //Left swipe
                            playerWeapons[weaponIndex].gameObject.SetActive(false);

                            weaponIndex++;

                            if(weaponIndex == playerWeapons.Length)
                                weaponIndex = 0;

                            playerWeapons[weaponIndex].gameObject.SetActive(true);
                        }
                    }
                    else
                    {   
                        if (lp.y > fp.y)  
                        {   //Up swipe
                            playerWeapons[weaponIndex].gameObject.SetActive(false);

                            weaponIndex++;

                            if(weaponIndex == playerWeapons.Length)
                                weaponIndex = 0;

                            playerWeapons[weaponIndex].gameObject.SetActive(true);
                        }
                        else
                        {   //Down swipe
                            playerWeapons[weaponIndex].gameObject.SetActive(false);

                            weaponIndex++;

                            if(weaponIndex == playerWeapons.Length)
                                weaponIndex = 0;

                            playerWeapons[weaponIndex].gameObject.SetActive(true);
                        }
                    }
                }
            }
        }  
    }

    public void Shoot(Vector3 spawnPos)
    {

        targetPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        bulletSpawnPosition = new Vector2(spawnPos.x, spawnPos.y);

        direction = (targetPos - bulletSpawnPosition).normalized;

        bulletRotation = Quaternion.Euler(0,0,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        // BulletPool.instance.FireBullet(weaponIndex, spawnPos,
        //     bulletRotation, direction);
        
        GameObject newBullet = Instantiate(weaponBullets[weaponIndex],
            spawnPos, bulletRotation);

        if (weaponIndex==0)
        {
            pistolSound.Play();
        }
        else if (weaponIndex==1)
        {
            matterSound.Play();
        }
        else if (weaponIndex==2)
        {
            laserSound.Play();
        }
        else if (weaponIndex==3)
        {
            flamethrowerSound.Play();
        }

        newBullet.GetComponent<Bullet>().MoveInDirection(direction);

    }

} //class
