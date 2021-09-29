using System;
using ItemAPI;
using UnityEngine;

namespace Knives
{
	public class PlayerSpecialStates : MonoBehaviour
	{

		public PlayerSpecialStates()
		{
			theatrefreebegotten = false;
			
			


		}


		private void Start()
		{
			this.aIActor = base.GetComponent<AIActor>();

		}


		private void Update()
		{

		}

		public bool theatrefreebegotten;



		private AIActor aIActor;
	}
}
