using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("------ Components --------")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("------ Stats --------")]
    [SerializeField] int Hp;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int crouchSpeed;
    [SerializeField] float invisDuration;
    [SerializeField] float invisCD;
    [SerializeField] float invisRecharge;

    [Header("------ Guns --------")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int magCurrent;
    [SerializeField] int magCap;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] float reloadSpeed;


    [Header("------ Audio --------")]
    [SerializeField] AudioClip[] audSteps;
    [SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audJump;
    [SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audReload;
    [SerializeField] float audRelodVol;


    bool isSprinting;
    bool isPlayingHurt;
    bool isPlayingSteps;
    bool isShooting;
    bool isCrouching;
    public bool isInvisible;
    bool isReloading;

    int HPOrig;
    int jumpCount;
    int selectedGun;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = Hp;
        updatePlayerUI();
        // Added calls to the MagCap and currentMagCount methods to update UI. ** Removed from start and moved to updatePlayerUI.
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

            movement();
            crouch();
            if (Input.GetButton("Fire1") && gunList.Count > 0 && !isShooting)
                StartCoroutine(shoot());
            if (Input.GetButton("Reload") && gunList.Count > 0 && !isReloading)
            {
                StartCoroutine(reload());
    
            }
            selectGun();
            if (Input.GetButton("Ability 1") && invisRecharge == invisCD)
               {
                GameManager.instance.activateAbilityTxt.SetActive(false);
                StartCoroutine(goInvisible());
            }
            if (invisRecharge < invisCD)
            {
                if (invisRecharge + 1 * Time.deltaTime > invisCD)
                {
                    invisRecharge = invisCD;
                }
                else
                {
                    invisRecharge += 1 * Time.deltaTime;
                }
            }
        }
        sprint();
        updatePlayerUI();
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
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (controller.isGrounded && moveDir.magnitude > 0.3f && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
        }
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }
    IEnumerator playSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
            yield return new WaitForSeconds(0.1f);


        isPlayingSteps = false;
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
        if (!isReloading && gunList[selectedGun].ammoCur > 0)
        {

            aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootVol);
        }

        RaycastHit hit;
        if (gunList[selectedGun].ammoCur > 0)
        {
            StartCoroutine(flashMuzzle());
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
            {
                Debug.Log(hit.transform.name);

                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && dmg != null)
                {
                    dmg.takeDamage(shootDamage);
                }
                else
                {
                    Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
                }

            }
            gunList[selectedGun].ammoCur -= 1;
            updatePlayerUI();
        } else if(!isReloading && gunList[selectedGun].ammoCur >= 0)
        {
            StartCoroutine(reload());
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    IEnumerator flashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }

    public void takeDamage(int amount)
    {
        Hp -= amount;

        if (!isPlayingHurt)
        {
            StartCoroutine(isHurtSoundPlaying());
        }

        updatePlayerUI();
        if (Hp <= 0)
        {
            GameManager.instance.youLose();
        }

    }

    public void AddHealth(int amount)
    {
        Hp += amount;
        if (Hp > HPOrig)
        {
            Hp = HPOrig;
        }
        updatePlayerUI();
    }

    IEnumerator isHurtSoundPlaying()
    {
        isPlayingHurt = true;
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
        yield return new WaitForSeconds(0.1f);
        isPlayingHurt = false;
    }

    void updatePlayerUI()
    {
            GameManager.instance.playerHPBar.fillAmount = (float)Hp / HPOrig;
        if (gunList.Count > 0)
        {
            GameManager.instance.currentMagCount(gunList[selectedGun].ammoCur);
            GameManager.instance.MagCap(gunList[selectedGun].ammoMax);
        }
        GameManager.instance.invisCooldownBar.fillAmount = invisRecharge / invisCD;
        if (!isInvisible && invisRecharge == invisCD)
          { 
            GameManager.instance.activateAbilityTxt.SetActive(true);
            GameManager.instance.SetInvisText("Ready!");
        }
    }

    public void GetGunStats(gunStats gun)
    {
        gunList.Add(gun);

        selectedGun = gunList.Count - 1;

        updatePlayerUI();

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        //Added updateplayerui call to update gun ammo count
        updatePlayerUI();
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

    }

    public bool GetCrouch()
    {
        return isCrouching;
    }


    // Added reload method for when mag is empty.
    IEnumerator reload()
    {
        aud.PlayOneShot(audReload[Random.Range(0, audReload.Length)], audRelodVol);
        isReloading = true;
        GameManager.instance.reloadingTxt.SetActive(true);
        yield return new WaitForSeconds(gunList[selectedGun].reloadSpeed);
        gunList[selectedGun].ammoCur = gunList[selectedGun].ammoMax;
        GameManager.instance.reloadingTxt.SetActive(false);
        updatePlayerUI();
        isReloading = false;
        
    }

    //Added code to toggle isInvisible, allowing player to go invisible.
    IEnumerator goInvisible()
    {
        isInvisible = true;
        GameManager.instance.SetInvisText("Active!");
        yield return new WaitForSeconds(invisDuration);
        isInvisible = false;
        GameManager.instance.SetInvisText("Recharging...");
        invisRecharge = 0;
    }

    
   
    public int GetHPMax()
    {
        return HPOrig;
    }

    public int GetHPCurrent()
    {
        return Hp;
    }

}
