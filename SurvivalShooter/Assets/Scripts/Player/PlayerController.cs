using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Player player;
    
    float horizontalMovement;
    float verticalMovement;

    void Update()
    {
		GetHorizontalMovementInput();
		GetVerticalMovementInput();

        DetectMouseClickToFire();
        DetectPickItemInput();
		DetectReloadInput();
		DetectMouseScrollInput();
	}

	void FixedUpdate()
	{
		MovePlayer();
	}

	void OnMouseDown()
	{
		if (player.GetEquippedWeapon())
		{
			player.GetEquippedWeapon().Shoot(player.GetFaction());
		}
	}

	void GetHorizontalMovementInput()
    {
		horizontalMovement = Input.GetAxisRaw("Horizontal");
	}

    void GetVerticalMovementInput()
    {
		verticalMovement = Input.GetAxisRaw("Vertical");
	}

	void MovePlayer()
    {
        player.Move(new Vector2(horizontalMovement, verticalMovement).normalized);
	}

	void DetectPickItemInput()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			player.PickNearestItem();
		}
	}

	void DetectReloadInput()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			player.Reload();
		}
	}

	void DetectMouseClickToFire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
			player.Attack();
        }
    }

	void DetectMouseScrollInput()
	{
		if (Input.mouseScrollDelta.y > 0.1f)
		{
			player.ChangeWeapon(1);
		}
		else if (Input.mouseScrollDelta.y < -0.1f)
		{
			player.ChangeWeapon(-1);
		}
	}
}
