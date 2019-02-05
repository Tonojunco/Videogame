using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurunAlgo : MonoBehaviour {

	// Use this for initialization
	public float speed;
	private GameObject playerobject;
	private float unitx;
	private float unity;
	private float unitxnegative;
	private float unitynegative;
	private Transform playerobjecttransform;
	private GameObject parent;
	private Rigidbody2D rb2d;
	private float playerpositionx;
	private float playerpositiony;
	private float unitx2;
	private float unity2;

	void Start () {
		//parent=transform.parent.gameObject;
		playerobject = GameObject.FindGameObjectWithTag("Player");
		rb2d=GetComponent<Rigidbody2D> ();
		playerpositionx=0.01f;
		playerpositiony=0.01f;
		unitxnegative=1;
		unitynegative=1;
		playerPosition();

	}

	// Update is called once per frame
	void Update () {
		//playerPosition();
		unitx=playerobject.transform.position.x-gameObject.transform.position.x;

		unity=playerobject.transform.position.y-gameObject.transform.position.y;
		unitx2=unitx*unitx;
		unity2=unity*unity;
		float divide=unitx2+unity2;
		if(unitx<0){
			unitxnegative=-1;
		}
		else{
			unitxnegative=1;
		}
		if(unity<0){
			unitynegative=-1;
		}
		else{
			unitynegative=1;
		}
		float movementx=(unitx2/divide)*unitxnegative;
		float movementy=(unity2/divide)*unitynegative;
		Debug.Log("Going to move x: "+movementx);
		Debug.Log("Going to move y: "+movementy);
		Vector2 movement=new Vector2 (movementx,movementy);
		//playerpositionx=playerobjecttransform.position.x*playerobjecttransform.position.x;
		//playerpositiony=playerobjecttransform.position.y*playerobjecttransform.position.y;

		//transform.LookAt(playerobjecttransform);
		//Vector2 movement=new Vector2 ((playerpositionx/(playerpositionx+playerpositiony)),(playerpositionx/(playerpositionx+playerpositiony)));
    rb2d.AddForce(movement*speed);
			//parent.transform.position+=gameObject.transform.position;

	}
	void playerPosition(){
		playerpositionx=playerobject.transform.position.x;
		playerpositiony=playerobject.transform.position.y;
		Debug.Log("Position x: "+playerpositionx);
		Debug.Log("Position y: "+playerpositiony);
	}
}
