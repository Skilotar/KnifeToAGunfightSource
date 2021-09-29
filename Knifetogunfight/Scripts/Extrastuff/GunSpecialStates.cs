using System;
using ItemAPI;
using UnityEngine;

namespace Knives
{
	public class GunSpecialStates : MonoBehaviour
	{

		public GunSpecialStates()
		{
			DoesMacRage = false;
			DoesCountBlocks = false;
			RageCount = 0;

		}


		private void Start()
		{
			this.gun = base.GetComponent<Gun>();

		}


		private void Update()
		{

		}

		public bool DoesMacRage;
		public int RageCount;
		public bool DoesCountBlocks;
		public int successfullBlocks;


		private Gun gun;
	}
}

