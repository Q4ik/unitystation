﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StationGateway : NetworkBehaviour
{
	[SerializeField]
	private SpriteRenderer SpriteBaseTop;
	[SerializeField]
	private SpriteRenderer SpriteBaseBottom;
	[SerializeField]
	private SpriteRenderer SpriteBaseCentre;// TODO animation of centre piece
	[SerializeField]
	private SpriteRenderer SpriteBaseRightTop;
	[SerializeField]
	private SpriteRenderer SpriteBaseRightMiddle;
	[SerializeField]
	private SpriteRenderer SpriteBaseRightBottom;
	[SerializeField]
	private SpriteRenderer SpriteBaseLeftTop;
	[SerializeField]
	private SpriteRenderer SpriteBaseLeftMiddle;
	[SerializeField]
	private SpriteRenderer SpriteBaseLeftBottom;

	[SerializeField]
	private Sprite[] Online;
	[SerializeField]
	private Sprite[] Offline;
	[SerializeField]
	private Sprite[] PowerOff;

	[SerializeField]
	private List<GameObject> Worlds = new List<GameObject>();

	private GameObject SelectedWorld;

	private bool HasPower = true;

	private bool IsConnected;

	[SerializeField]
	private int RandomCountBegining = 300;
	[SerializeField]
	private int RandomCountEnd = 1200;

	private RegisterTile registerTile;

	private Matrix Matrix => registerTile.Matrix;

	private void Start()
	{
		registerTile = GetComponent<RegisterTile>();
		SetOffline();
		var count = Random.Range(RandomCountBegining, RandomCountEnd);
		Invoke("WorldSetup", count);
	}

	private void WorldSetup()//do message here as well
	{
		//Selects Random world
		SelectedWorld = Worlds[Random.Range(0, Worlds.Count)];

		if (HasPower == true)
		{
			SetOnline();

			var text = "Alert! New Gateway connection formed.";
			CentComm.MakeAnnouncement(CentComm.CentCommAnnounceTemplate, text, CentComm.UpdateSound.announce);

			var scene = SelectedWorld.transform.parent.parent.parent.gameObject;

			if (scene.activeSelf == false)
			{
				scene.SetActive(true);
			}

			loop();
		}
	}

	private void loop()
	{
		DetectPlayer();
		Invoke("loop", 2f);
	}

	[Server]
	private void DetectPlayer()
	{	
		var playersFound = Matrix.Get<ObjectBehaviour>(registerTile.LocalPositionServer + Vector3Int.up, ObjectType.Player, true);

		Logger.Log("Players on tile:" + playersFound);

		foreach (ObjectBehaviour player in playersFound)
		{
			Logger.Log("transport player:" + player);
			TransportPlayers(player);
		}
	}

	[Server]
	private void TransportPlayers(ObjectBehaviour player)
	{
		Logger.Log("Server transport");
		player.GetComponent<PlayerSync>().SetPosition(SelectedWorld.GetComponent<RegisterTile>().WorldPosition, false);
	}

	private void SetOnline()
	{
		SpriteBaseTop.sprite = Online[1];
		SpriteBaseBottom.sprite = Online[0];
		SpriteBaseRightMiddle.sprite = Online[2];
		SpriteBaseLeftMiddle.sprite = Online[3];
		SpriteBaseRightBottom.sprite = Online[4];
		SpriteBaseLeftBottom.sprite = Online[5];
		SpriteBaseRightTop.sprite = Online[6];
		SpriteBaseLeftTop.sprite = Online[7];
		SpriteBaseCentre.sprite = Online[8];
	}

	private void SetOffline()
	{
		SpriteBaseTop.sprite = Offline[1];
		SpriteBaseBottom.sprite = Offline[0];
		SpriteBaseRightMiddle.sprite = Offline[2];
		SpriteBaseLeftMiddle.sprite = Offline[3];
		SpriteBaseRightBottom.sprite = Offline[4];
		SpriteBaseLeftBottom.sprite = Offline[5];
		SpriteBaseRightTop.sprite = Offline[6];
		SpriteBaseLeftTop.sprite = Offline[7];
		SpriteBaseCentre.sprite = Offline[8];
	}

	private void SetPowerOff()
	{
		SpriteBaseTop.sprite = PowerOff[1];
		SpriteBaseBottom.sprite = PowerOff[0];
		SpriteBaseRightMiddle.sprite = PowerOff[2];
		SpriteBaseLeftMiddle.sprite = PowerOff[3];
		SpriteBaseRightBottom.sprite = PowerOff[4];
		SpriteBaseLeftBottom.sprite = PowerOff[5];
		SpriteBaseRightTop.sprite = PowerOff[6];
		SpriteBaseLeftTop.sprite = PowerOff[7];
		SpriteBaseCentre.sprite = PowerOff[8];
	}

}
