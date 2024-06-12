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

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = Hp;
        updatePlayerUI();
        GameManager.instance.currentMagCount(magCurrent);
        GameManager.instance.MagCap(magCap);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        movement();
        sprint();
        crouch();
        if (Input.GetButton("Fire1") && !isShooting)
            StartCoroutine(shoot());
        if(Input.GetButton("Reload"))
        {
            StartCoroutine(reload());
        }
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

    public bool GetCrouch()
    {
        return isCrouching;
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(reloadSpeed);
        magCurrent = magCap;
        GameManager.instance.currentMagCount(magCurrent);
        magEmpty = false;

    }
}
