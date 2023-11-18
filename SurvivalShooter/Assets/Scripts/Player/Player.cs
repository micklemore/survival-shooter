using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
	[SerializeField]
	Transform weaponSocketRightTransform;

	[SerializeField]
	Transform weaponSocketLeftTransform;

	[SerializeField]
	float playerSpeed = 10f;

	[SerializeField]
	FactionEnum faction = FactionEnum.PLAYER_FACTION;

	Weapon actualEquippedWeapon;

	MovementFSM movementFSM;
	AttackFSM attackFSM;

	List<IPickable> overlappedPickableObjects = new List<IPickable>();

	Dictionary<int, int> ammunitions = new Dictionary<int, int>();

	public FactionEnum GetFaction() => faction;

	void Start()
	{
		movementFSM = GetComponent<MovementFSM>();
		attackFSM = GetComponent<AttackFSM>();
		EquipWeapon(0); //0 is id of melee weapon
	}

	public void Move(Vector2 direction)
    {
		transform.position += (Vector3)direction * Time.fixedDeltaTime * playerSpeed;
		if (direction.x == 0 && direction.y == 0 && movementFSM.GetCurrentState() == PlayerStates.MOVING)
		{
			movementFSM.ChangeState(movementFSM.idleState);
		}
		else if ((direction.x != 0 || direction.y != 0) && movementFSM.GetCurrentState() == PlayerStates.IDLE)
		{
			movementFSM.ChangeState(movementFSM.movingState);
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
		return hitResult;
		//health -= damageAmount
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
		return actualEquippedWeapon != null && actualEquippedWeapon.Id != 0 && !actualEquippedWeapon.IsChargerFull()
			&& ammunitions.ContainsKey(actualEquippedWeapon.Id) && ammunitions[actualEquippedWeapon.Id] > 0;
	}

	public void Attack()
	{
		if (actualEquippedWeapon != null)
		{
			attackFSM.ChangeState(attackFSM.attackingState);
			actualEquippedWeapon.Shoot(faction);
			attackFSM.ChangeState(attackFSM.notAttackingState); //questo non va definito prima sul player, ma soprattutto è lo stato a sapere quando l'anim ha finito di playare
		}
	}
}
