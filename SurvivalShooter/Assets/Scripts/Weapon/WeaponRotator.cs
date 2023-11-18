using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotator : MonoBehaviour
{
	bool isFacingRight = true;

	Weapon weapon;

	void Start()
	{
		weapon = gameObject.GetComponent<Weapon>();		
	}

	void Update()
    {
        RotateWeaponToMousePosition();
	}

    void RotateWeaponToMousePosition()
    {
		Vector3 relativePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(-relativePos.y, relativePos.x));
		transform.rotation = rotation;

		Vector3 relativePositionToOwner = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weapon.Owner.transform.position;
		if ((relativePositionToOwner.x < 0.001 && isFacingRight) || (relativePositionToOwner.x > 0.001 && !isFacingRight))
		{
			Flip();
			isFacingRight = !isFacingRight;
		}
	}

	void Flip()
	{
		weapon.SwithWeaponSocket();

		Vector3 newScale = transform.localScale;
		newScale.y *= -1;
		transform.localScale = newScale;
	}
}