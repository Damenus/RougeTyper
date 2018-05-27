﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
		
	public class EnemyInBattle : MonoBehaviour {


		public int health;
		public float animWaitTime;

		private Animator Animator;

		// Use this for initialization
		void Start () {
			health = 100;
			Animator = GetComponent<Animator>();
			InvokeRepeating ("PlayAttackAnim", 6f, animWaitTime);
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void PlayAttackAnim() {
			Animator.SetTrigger("enemyAttack");
		}

		private void OnDisable ()
		{
			 CancelInvoke();
		}
		

		public void hit(int damage) {
			health -= damage;
			// Animator.SetTrigger("enemyDamage");
		}
	}

}