﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter2D(Collider2D collider){

		if (collider.gameObject.CompareTag("Attack")){
					Debug.Log("Killed");
          Destroy(gameObject);
        }
	}
}
