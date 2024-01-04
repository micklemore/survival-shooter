using SmartMVC;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
	[SerializeField]
	Transform weaponSocketRightTransform;

	[SerializeField]
	Transform weaponSocketLeftTransform;

	[SerializeField]
	float speed = 10f;

	[SerializeField]
	float totalHealth = 300f;

	[SerializeField]
	FactionEnum faction = FactionEnum.PLAYER_FACTION;

	[SerializeField]
	Animator animator;

	float actualHealth;

	bool isDead = false;

	bool hasARechargableWeapon => actualEquippedWeapon != null && actualEquippedWeapon.Id != 0;

	Weapon actualEquippedWeapon;

	PlayerFSM playerFSM;

	List<IPickable> overlappedPickableObjects = new List<IPickable>();

	Dictionary<int, int> ammunitions = new Dictionary<int, int>();

	string lastAnimationLookDirection = "_front";

	string currentStateName;

	public FactionEnum GetFaction() => faction;

	void Update()
	{
		UpdateLookDirection();
	}

	void Start()
	{
		playerFSM = GetComponent<PlayerFSM>();
		
		actualHealth = totalHealth;
		BaseApplication.Notify((int)EventsEnum.PLAYER_HEALTH_MODIFY, this, actualHealth, totalHealth);

		EquipWeapon(0); //0 is melee weapon id
	}

	public void Move(Vector2 direction)
    {
		transform.position += (Vector3)direction * Time.fixedDeltaTime * speed;
		if (direction.x == 0 && direction.y == 0 && playerFSM.GetCurrentState() == PlayerStates.MOVING)
		{
			playerFSM.ChangeState(playerFSM.idleState);
		}
		else if ((direction.x != 0 || direction.y != 0) && playerFSM.GetCurrentState() == PlayerStates.IDLE)
		{
			playerFSM.ChangeState(playerFSM.movingState);
		}
	}

	void EquipWeapon(WeaponSpawn weaponSpawn)
	{
		if (EquipWeapon(weaponSpawn.Id))
		{
			Destroy(weaponSpawn.gameObject);
		}
	}

	bool EquipWeapon(int weaponId)
	{
		WeaponConstructor weaponSO = WeaponGateway.instance.GetWeaponById(weaponId);
		
		if (weaponSO)
		{
			Weapon weapon = Instantiate(weaponSO.Weapon, weaponSocketRightTransform.position, Quaternion.identity);

			if (weapon != null)
			{
				if (actualEquippedWeapon != null)
				{
					Destroy(actualEquippedWeapon.gameObject);
				}

				weapon.Equip(weaponSocketRightTransform, weaponSocketLeftTransform, weaponSO, this);
				actualEquippedWeapon = weapon;

				return true;
			}
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		IPickable pickableObject = collision.GetComponent<IPickable>();
		
		if (pickableObject != null)
		{
			overlappedPickableObjects.Add(pickableObject);
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		IPickable pickableObject = collision.GetComponent<IPickable>();

		if (pickableObject != null)
		{
			overlappedPickableObjects.Remove(pickableObject);
		}
	}

	public HitResult TakeDamage(DamageObject damageObject)
	{
		HitResult hitResult = new HitResult();
		hitResult.SetDamage(damageObject.DamageAmount);
		actualHealth -= damageObject.DamageAmount;

		BaseApplication.Notify((int)EventsEnum.PLAYER_HEALTH_MODIFY, this, actualHealth, totalHealth);

		Debug.Log("Player healt: " + actualHealth);

		if (!isDead && actualHealth <= 0)
		{
			isDead = true;
			HandleDeath();
		}
		hitResult.SetDead(isDead);
		return hitResult;
	}

	private void HandleDeath()
	{
		Debug.Log("Player is dead");
		EventHandler.instance.EndGameNotify();
	}

	public Weapon GetEquippedWeapon()
	{
		return actualEquippedWeapon;
	}

	public void PickNearestItem()
	{
		IPickable pickable = GetNearestItem();

		if (pickable != null)
		{
			WeaponSpawn weaponSpawn = (pickable as WeaponSpawn);
			if (weaponSpawn)
			{
				EquipWeapon(weaponSpawn);
			}
			else
			{
				Ammunition ammunition = (pickable as Ammunition);
				if (ammunition)
				{
					PickAmmunition(ammunition);
				}
			}
		}
	}

	void PickAmmunition(Ammunition ammunition)
	{
		if (ammunitions.ContainsKey(ammunition.WeaponIdCorrespondingToThisAmmo))
		{
			ammunitions[ammunition.WeaponIdCorrespondingToThisAmmo] += ammunition.Amount;
		}
		else
		{
			ammunitions.Add(ammunition.WeaponIdCorrespondingToThisAmmo, ammunition.Amount);
		}
		Destroy(ammunition.gameObject);

		Debug.Log("ho raccolto " + ammunition.Amount + " munizioni di tipo " + ammunition.WeaponIdCorrespondingToThisAmmo);
		Debug.Log("totale munizioni : ");

		foreach (int key in ammunitions.Keys)
		{
			Debug.Log("id " + key + " amount " + ammunitions[key]);
		}
	}

	IPickable GetNearestItem()
	{
		IPickable nearestPickable = null;

		if (overlappedPickableObjects.Count > 0)
		{
			float lowerDistance = float.MaxValue;

			foreach (IPickable pickable in overlappedPickableObjects)
			{
				float distance = (pickable.GetGameObjectTransform().position - transform.position).sqrMagnitude;

				if (distance < lowerDistance)
				{
					lowerDistance = distance;
					nearestPickable = pickable;
				}
			}
		}

		return nearestPickable;
	}

	public void Reload()
	{
		if (CanReload())
		{
			int reloadedProjectiles = actualEquippedWeapon.Reload(ammunitions[actualEquippedWeapon.Id]);
			ammunitions[actualEquippedWeapon.Id] = Mathf.Clamp(ammunitions[actualEquippedWeapon.Id] - reloadedProjectiles, 0, ammunitions[actualEquippedWeapon.Id]);

			Debug.Log("ho ricaricato, quindi totale munizioni : ");
			foreach (int key in ammunitions.Keys)
			{
				Debug.Log("id " + key + " amount " + ammunitions[key]);
			}
		}
	}

	bool CanReload()
	{
		return hasARechargableWeapon && !actualEquippedWeapon.IsChargerFull()
			&& ammunitions.ContainsKey(actualEquippedWeapon.Id) && ammunitions[actualEquippedWeapon.Id] > 0;
	}

	public void Attack()
	{
		if (actualEquippedWeapon != null)
		{
			actualEquippedWeapon.Shoot(faction);
		}
	}

	void UpdateLookDirection() 
	{
		string actualAnimationLook;
		Vector3 lookDirectionAtMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		
		if (lookDirectionAtMousePosition.y < 0)
		{
			actualAnimationLook = "_front";
		}
		else
		{
			actualAnimationLook = "_back";
		}

		if (lookDirectionAtMousePosition.x < 0)
		{
			actualAnimationLook += "_left";
		}
		else
		{
			actualAnimationLook += "_right";
		}

		if (!string.Equals(actualAnimationLook, lastAnimationLookDirection))
		{
			PlayAnimation(currentStateName + actualAnimationLook);
			lastAnimationLookDirection = actualAnimationLook;
		}
	}

	public void RefreshAnimationForNewState(string stateName)
	{
		currentStateName = stateName;
		string animationName = stateName + lastAnimationLookDirection;
		PlayAnimation(animationName);
	}

	public void PlayAnimation(string animationName)
	{
		animator.Play(animationName);
	}
}
