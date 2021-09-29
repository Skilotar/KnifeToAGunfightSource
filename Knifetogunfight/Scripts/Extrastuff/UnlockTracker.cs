using System;
using ItemAPI;
using UnityEngine;
using SaveAPI;

namespace Knives
{
	public class UnlockablesTracker : MonoBehaviour
	{

		public UnlockablesTracker()
		{
			

		}


		private void Start()
		{

			player = base.GetComponent<PlayerController>();


		}


		private void Update()
		{
			// ammo starved unlock trigger
			if(AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.AMMO_STARVED) == false)
			{
				foreach (Gun gun1 in player.inventory.AllGuns)
				{
					if (gun1.CurrentAmmo == 0)
					{
						foreach (Gun gun2 in player.inventory.AllGuns)
						{
							if (gun2.CurrentAmmo == 0 && gun2 != gun1)
							{
								AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.AMMO_STARVED, true);
							}
						}
					}
				}
			}


			
		}

		public PlayerController player;
	}
}
