﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInputHandler : NetworkBehaviour 
{

	MovementHandler movementHandler;
	CombatHandler combatHandler;
	public GameObject touched;
	public PowerHandler powerHandler;

	public bool altPressed = false;
	public bool ctrlPressed = false;
	public bool shiftPressed = false;

	// Use this for initialization
	void Start () 
	{
		movementHandler = this.GetComponent<MovementHandler>();
		combatHandler = this.GetComponent<CombatHandler>();
		powerHandler = this.GetComponent<PowerHandler>();
	}
	public override void OnStartLocalPlayer()
	{
		//move this to OnServerAddPlayer
		 MyPlayer myPlayer = FindObjectOfType(typeof(MyPlayer)) as MyPlayer;
		 myPlayer.myPlayer = this.gameObject;
		 myPlayer.inputHandler = this;
	}
	// Update is called once per frame
	void Update () 
	{
		if (!isLocalPlayer)
      		return;
//these should be changed to InputManager for easier remapping.
		/*if (Input.GetButtonDown("Shift")) {
			shiftPressed = true;
		}
		else if (Input.GetButtonUp("Shift")) {
			shiftPressed = false;
		}*/
		// Power hotkeys
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Debug.Log("Q");
			combatHandler.usePower(1);
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			Debug.Log("W");
			combatHandler.usePower(2);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("E");
			combatHandler.usePower(3);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("R");
			combatHandler.usePower(4);
		}
		//
		if (Input.GetKeyDown(KeyCode.S))
		{
			//stop attack and stop moving
			Debug.Log("S");
		}
		//Split movement and selecting to 2 different buttons
		//Need to implmement move to 
		//select
		if (Input.GetButtonDown("Fire1"))
		{
			var mousePos = Input.mousePosition;
   			mousePos.z = 10;
			RaycastHit2D[] hits = Physics2D.RaycastAll (Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
			int foundPriority = -1;
			GameObject selected;
			for(int x=0;x<hits.Length;x++)
			{
				RaycastHit2D hit = hits[x];
				if(hit.collider != null)
				{
					//square = hit.collider.gameObject.GetComponent<Square>();
					selected = hit.collider.gameObject;
					if(selected.tag != "Ground")
					{
						foundPriority = 1;
						touched = selected;
					}
				}
			}
			if( touched==null )
			{
				return;
			}
			TargetableObject targetable = touched.GetComponent<TargetableObject>();
			if( targetable != null )
			{
				combatHandler.target = targetable;
			}
			//movementHandler.moveToTarget(touched.transform);
		}
		//move
        else if (Input.GetButtonDown("Fire2"))
        {
			var mousePos = Input.mousePosition;
   			mousePos.z = 10;
			RaycastHit2D[] hits = Physics2D.RaycastAll (Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
			int foundPriority = -1;
			GameObject selected;
			for(int x=0;x<hits.Length;x++)
			{
				RaycastHit2D hit = hits[x];
				if(hit.collider != null)
				{
					//square = hit.collider.gameObject.GetComponent<Square>();
					selected = hit.collider.gameObject;
					if(selected.tag == "Ground" && foundPriority<=-1)
					{
						foundPriority = 0;
						touched = selected;
					}
				}
			}
			if( touched==null )
			{
				return;
			}
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			movementHandler.moveToLocation(pos);
		}
	}
}

/*
//Combined

var mousePos = Input.mousePosition;
   			mousePos.z = 10;
			RaycastHit2D[] hits = Physics2D.RaycastAll (Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
			int foundPriority = -1;
			GameObject selected;
			for(int x=0;x<hits.Length;x++)
			{
				RaycastHit2D hit = hits[x];
				if(hit.collider != null)
				{
					//square = hit.collider.gameObject.GetComponent<Square>();
					selected = hit.collider.gameObject;
					if(selected.tag == "Ground" && foundPriority<=-1)
					{
						foundPriority = 0;
						touched = selected;
					}
					else if(selected.tag != "Ground")
					{
						foundPriority = 1;
						touched = selected;
					}
				}
			}
			if( touched==null )
			{
				return;
			}
			if(touched.tag == "Ground")
			{

				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0;
				movementHandler.moveToLocation(pos);
			}
			else
			{
				TargetableObject targetable = touched.GetComponent<TargetableObject>();
				if( targetable != null )
				{
					combatHandler.target = targetable;
				}
				//movementHandler.moveToTarget(touched.transform);
			}
		}


 */