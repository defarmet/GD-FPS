using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamageable
{
	[Header("---------- Components -----------")]
	[SerializeField] public CharacterController controller;
	[SerializeField] Rigidbody           rb;
	[SerializeField] GameObject          hitEffect;

	[Header("---------- Player Attributes -----------")]
	[Range(1, 10)]   [SerializeField] public float playerSpeed;
	[Range(1.1f, 2)] [SerializeField] float        slideMult;
	[SerializeField]                  int          slideTime;
	[SerializeField]                  float        wallRunSpeed;
	[Range(8, 18)]   [SerializeField] float        jumpHeight;
	[Range(15, 30)]  [SerializeField] public float gravityValue;
	[Range(1, 3)]	 [SerializeField] int          jumpMax;
	[Range(0, 310)]  [SerializeField] public int   hp;
	[Range(0.1f, 2)] [SerializeField] float        switchTime;
	[Range(1, 2)]    [SerializeField] float        doubleJumpHeightMult;
	[Range(0.1f, 1)] [SerializeField] float        doubleJumpSpeedMult;

	[Header("---------- Gun Stats -----------")]
	[SerializeField]                  GameObject  gunModel;
	[Range(0.1f, 5)] [SerializeField] float       shootRate;
	[Range(1, 30)]   [SerializeField] float       shootDistance;
	[Range(1, 10)]   [SerializeField] int         shootDmg;
	public                            AudioSource gunfire;
	public                            int[]       currentAmmoCount = new int[6];
	                                  int         selectedWeapon;
					 [SerializeField] float       reloadTimer;

	bool isShooting = false;
	bool alreadyReloadedUI = false;
	
	Vector3 playerVelocity;
	Vector3 move = Vector3.zero;
	
	List<gunStats> gunstat = new List<gunStats>();
	int            weapIndx;
	
	public int timesJumps;
	
	float playerSpeedOG;
	int   hpOriginal;
	int   ammoCountOrig;
	public float gravityValueOG;
	bool canSlide = true;
	bool isSliding = false;
	bool isOnAir = false;
	bool canWallRun = true;
	public bool isSameWall = false;


	private void Start()
	{
		playerSpeedOG = playerSpeed;
		gravityValueOG = gravityValue;
		hpOriginal = hp;
		ammoCountOrig = 0;
		gunfire = GetComponent<AudioSource>();
		controller.enabled = true;
		updatePlayerHp();
	}

	void Update()
	{
		if (transform.position.y < -30)
			respawn();

		playerMovement();
		slide();

		StartCoroutine(reload());

		StartCoroutine(gunSwitch());
		StartCoroutine(shoot());
	}

	/*
	 * Player movement is at a constant running speed.
	 */
	void playerMovement()
	{
		if (controller.isGrounded && playerVelocity.y < 0) {
			isOnAir = false;
			playerVelocity.y = 0f;
			timesJumps = 0;
		}

		move = ((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical")));
		controller.Move(move * Time.deltaTime * playerSpeed);

		//if(isSameWall == false)
        //{
			if (Input.GetButtonDown("Jump") && timesJumps < jumpMax)
			{
				isOnAir = true;
				playerVelocity.y = jumpHeight;
				timesJumps++;

				if (timesJumps > 1)
				{
					playerVelocity.y = jumpHeight * doubleJumpHeightMult;
				}
			}
		//}

		playerVelocity.y -= gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}

	/*
	 * Sliding allows the player to temporarily move faster.
	 */
	void slide()
	{
		if (canSlide)
		{
			if (Input.GetButtonDown("Sprint"))
			{
				isSliding = true;
				StartCoroutine(slowSlide());

			}
			else if (Input.GetButtonUp("Sprint"))
			{
				isSliding = false;
				transform.localScale = new Vector3(1, 1, 1);
				playerSpeed = playerSpeedOG;
				StartCoroutine(standUp());
			}
		}
	}

	IEnumerator slowSlide()
	{

		//transform.localScale = new Vector3(1, .5f, 1);
		controller.transform.localScale = new Vector3(1, 0.5f, 1);
		//gunModel.transform.localScale = new Vector3(1, 1, 1);

		if (timesJumps > 0)
		{
			playerSpeed = playerSpeed * slideMult + 0.5f;
		}
		else
		{
			playerSpeed = playerSpeed * slideMult;
		}
		while (isSliding)
		{
			yield return new WaitForSeconds(0.2f);
			playerSpeed -= 0.3f;

			if (playerSpeed <= playerSpeedOG / 1.5)
			{
				break;
			}
		}
	}

	IEnumerator standUp()
	{
		canSlide = false;
		yield return new WaitForSeconds(0.5f);
		canSlide = true;
	}

	IEnumerator shoot()
	{
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red, 0.0000001f);

		if (gunstat.Count != 0 && Input.GetButton("Shoot") && currentAmmoCount[selectedWeapon] > 0 && isShooting == false && !gameManager.instance.isPaused) {
			isShooting = true;
			gunfire.Play();
			gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(currentAmmoCount[selectedWeapon] - 1).gameObject.SetActive(false);
			currentAmmoCount[selectedWeapon]--;

			if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, shootDistance)) {
				Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
				
				if (hit.collider.GetComponent<IDamageable>() != null) {
					IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();
					
					/* Headshot handler */
					if (hit.collider is SphereCollider)
						isDamageable.takeDamage(shootDmg * 2);
					else
						isDamageable.takeDamage(shootDmg);
				}
			}

			yield return new WaitForSeconds(shootRate);
			isShooting = false;
		}
	}

	/*
	 * Guns are added to a list for easy swapping between weapons
	 */
	public void gunPickup(gunStats _gunStat, int _currentGunHUD)
	{
		if (alreadyReloadedUI) {
			gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
			alreadyReloadedUI = false;
		}
		gunstat.Add(_gunStat);

		if (gunstat.Count != 0)
			selectedWeapon = gunstat.Count - 1;

		shootRate = _gunStat.shootRate;
		shootDistance = _gunStat.shootDist;
		shootDmg = _gunStat.shootDmg;
		currentAmmoCount[selectedWeapon] = _gunStat.ammoCapacity;
		ammoCountOrig = _gunStat.ammoCapacity;
		gunModel.GetComponent<MeshFilter>().sharedMesh = _gunStat.model.GetComponent<MeshFilter>().sharedMesh;
		gunModel.GetComponent<MeshRenderer>().sharedMaterial = _gunStat.model.GetComponent<MeshRenderer>().sharedMaterial;

		if (gameManager.instance.currentGunHUD != null)
			gameManager.instance.currentGunHUD.SetActive(false);
		
		gameManager.instance.currentGunHUD = gameManager.instance.gunHUD[_currentGunHUD];
		gameManager.instance.currentGunHUD.SetActive(true);

		for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
			gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
	}

	/*
	 * Switching guns takes time, which removes potential glitches involving fast gun switching.
	 */
	IEnumerator gunSwitch()
	{
		if (gunstat.Count > 0 && !gameManager.instance.isPaused) {
			if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < gunstat.Count - 1) {
				if (alreadyReloadedUI) {
					gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
					alreadyReloadedUI = false;
				}
				selectedWeapon++;
				
			} else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0) {
				if (alreadyReloadedUI) {
					gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
					alreadyReloadedUI = false;
				}
				selectedWeapon--;
				
			}
				
			shootRate = gunstat[selectedWeapon].shootRate;
			shootDistance = gunstat[selectedWeapon].shootDist;
			shootDmg = gunstat[selectedWeapon].shootDmg;
			ammoCountOrig = gunstat[selectedWeapon].ammoCapacity;
			reloadTimer = gunstat[selectedWeapon].reloadTime;
			gunModel.GetComponent<MeshFilter>().sharedMesh = gunstat[selectedWeapon].model.GetComponent<MeshFilter>().sharedMesh;
			gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunstat[selectedWeapon].model.GetComponent<MeshRenderer>().sharedMaterial;

			gameManager.instance.currentGunHUD.SetActive(false);
			gameManager.instance.currentGunHUD = gameManager.instance.gunHUD[gunstat[selectedWeapon].gunHUD];
			gameManager.instance.currentGunHUD.SetActive(true);
			for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
				gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

			yield return new WaitForSeconds(switchTime);


		}
	}

	public void takeDamage(int dmg)
	{
		if (dmg > 0)
		{
			StartCoroutine(damageFlash());
			//terry added
			damageArrow();
		}
		hp -= dmg;
		updatePlayerHp();
		if (hp < 1) {
			death();
			resetHP();
		}
	}

	public void respawn()
	{
		controller.enabled = false;
		transform.position = gameManager.instance.playerSpawnPoint.transform.position;
		controller.enabled = true;
		gameManager.instance.isPaused = false;
		updatePlayerHp();
		resetPlayerAmmo();
	}

	public void death()
	{
		gameManager.instance.cursorLockPause();
		gameManager.instance.currentMenuOpen = gameManager.instance.playerDeadMenu;
		gameManager.instance.currentMenuOpen.SetActive(true);
	}

	IEnumerator damageFlash()
	{
		gameManager.instance.playerDamageFlash.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		gameManager.instance.playerDamageFlash.SetActive(false);
	}

	public void resetHP()
	{
		hp = hpOriginal;
	}

	/*
	 * All guns are reloaded when a player respawns
	 */
	public void resetPlayerAmmo()
	{
		for (int i = 0; i < gunstat.Count; ++i) {
			currentAmmoCount[i] = gunstat[i].ammoCapacity;
			for (int j = 0; j < gunstat[i].ammoCapacity; ++j)
				gameManager.instance.gunHUD[gunstat[i].gunHUD].transform.GetChild(0).GetChild(j).gameObject.SetActive(true);
		}
	}

	public void updatePlayerHp()
	{
		gameManager.instance.playerHpBar.fillAmount = (float)hp / (float)hpOriginal;
	}

	public IEnumerator reload()
	{
		if (Input.GetButtonDown("Reload") && gunstat.Count != 0) {
			if (currentAmmoCount[selectedWeapon] == ammoCountOrig && !alreadyReloadedUI) {
				StartCoroutine(alreadyReloaded());
			} else if (currentAmmoCount[selectedWeapon] != ammoCountOrig) {
				//yield return new WaitForSeconds(gameManager.instance.gunStatsScript.reloadTime);
				yield return new WaitForSeconds(reloadTimer);
				currentAmmoCount[selectedWeapon] = ammoCountOrig;
				for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
					gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
				
			}
		}
	}

	/*
	 * The player should know if they have a full magazine
	 */
	IEnumerator alreadyReloaded()
	{
		alreadyReloadedUI = true;
		gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(true);
		yield return new WaitForSeconds(2);
		gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
		alreadyReloadedUI = false;
	}

	IEnumerator slowJumpMovement()
	{
		playerSpeed = playerSpeed * doubleJumpSpeedMult;
		yield return new WaitForSeconds(1.8f);
		playerSpeed = playerSpeedOG;
	}

	//terry changes
    #region TerryAdded
    private void damageArrow()
    {
		//      gameManager.instance.d_arrow.SetActive(true);
		float angle = HitAngle(gameManager.instance.d_angle.normalized);
		//gameManager.instance.d_arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);
		//      gameManager.instance.d_arrow.SetActive(false);
		GameObject d_indicator = Instantiate(gameManager.instance.d_arrow, Vector3.zero, Quaternion.Euler(0,0,-angle));
	}

    public float HitAngle(Vector3 direction)
    {
        var otherDir = new Vector3(-direction.x, 0f, -direction.z);
        var playerFwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        var angle = Vector3.SignedAngle(playerFwd, otherDir, Vector3.up);

        return angle;
    }
    #endregion
}