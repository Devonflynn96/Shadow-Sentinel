using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;

    [SerializeField] int Hp;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int crouchSpeed;


    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int magCurrent;
    [SerializeField] int magCap;


    [SerializeField] float reloadSpeed;

    bool isShooting;
    bool isCrouching;
    bool magEmpty;

    int HPOrig;
    int jumpCount;
    int seletctedGun;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = Hp;
        updatePlayerUI();
        // Added calls to the MagCap and currentMagCount methods to update UI.
        GameManager.instance.currentMagCount(magCurrent);
        GameManager.instance.MagCap(magCap);
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

            movement();
            crouch();
            if (Input.GetButton("Fire1") && !isShooting)
                StartCoroutine(shoot());
            if (Input.GetButton("Reload"))
            {
                StartCoroutine(reload());
            }
            selectGun();
        }
        sprint();
        
    }

    void movement()
    {

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        if (isCrouching)
        {
            controller.Move(moveDir * crouchSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
            controller.height /= 2;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
            controller.height *= 2;
        }
    }

    //Updated shoot method to do a reload needed check
    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if(magCurrent >0 && !magEmpty)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
            {
                Debug.Log(hit.transform.name);

                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && dmg != null)
                {
                    dmg.takeDamage(shootDamage);
                }

            magCurrent -= 1;
            if(magCurrent <= 0)
                magEmpty = true;
            }
        } else
        {
            StartCoroutine(reload());
        }
        GameManager.instance.currentMagCount(magCurrent);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        Hp -= amount;
        updatePlayerUI();
        if (Hp <= 0)
        {
            GameManager.instance.youLose();
        }

    }

    void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)Hp / HPOrig;
    }

    public void GetGunStats(gunStats gun)
    {
        gunList.Add(gun);

        seletctedGun = gunList.Count - 1;

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && seletctedGun < gunList.Count - 1)
        {
            seletctedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && seletctedGun > 0)
        {
            seletctedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootDamage = gunList[seletctedGun].shootDamage;
        shootDist = gunList[seletctedGun].shootDist;
        shootRate = gunList[seletctedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[seletctedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[seletctedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

    }

    public bool GetCrouch()
    {
        return isCrouching;
    }


    // Added reload method for when mag is empty.
    IEnumerator reload()
    {
        yield return new WaitForSeconds(reloadSpeed);
        magCurrent = magCap;
        GameManager.instance.currentMagCount(magCurrent);
        magEmpty = false;
    }
   
}
