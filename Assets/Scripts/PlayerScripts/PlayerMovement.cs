using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Errors still present, while inputing two directions the first one is respected
//****************************************************************************************
//Developed by Antonio Junco de Haas
//Help obtained from https://unity3d.com/es/learn/tutorials/projects/2d-ufo-tutorial/controlling-player
//Move the player using the WASD Keys
public class PlayerMovement : MonoBehaviour {

//Name of the rigid body that we are going to use
  private Rigidbody2D rb2d;

//***************************************************************************************************************************************************************************************
//Values of the player
  //Direction currently facing 1 up 2 left 3 down 4 right, down as a default
  int direction=3;
  //How fast will the character go
  public float speed;
  //If true then the player can't move, used after attacks and on special events
  public bool freeze;
  //If true the player can't move, but can rotate
  public bool stickyfloor;
  //How much time unitl move again
  public float stickyfloorcounterlimit;
  //Will count until
  private float stickyfloorcounter=0;
  private bool willmoveagain=true;
  //Ammount of time the player needs to move after Attacking
  public float swinglag;
  private float swinglagcounter=0.0f;
  //Ammount of time the sword is going to be active for
  public float attacktime;
  private float attacktimecounter;

//***************************************************************************************************************************************************************************************
//Attack variables and objects
  //Game object that is the hitbox of the sword
  public GameObject sword;
  //The center of the player, used to move the sword in the four directions
  public GameObject playerCenter;
  //Orange sprite used to show the time that the hitbox is active
  public Sprite activeattack;
  //Green sprite used to show time until next attack is ready
  public Sprite recharge;
  //Pink sprite is used to show that it is possible to use the next attack
  public Sprite readyattack;
  //Sprite Renderer for values recharge and readyattack, in game object sword
  private SpriteRenderer attackstatus;
  //Attack hit box
  private BoxCollider2D attackhitbox;
  //Value preventing the player from executing immediate attacks
  public bool isAttacking=false;
  //The value of the cooldown for the next attack
  public float attackcooldown;
  private float attackcooldowncounter;

//***************************************************************************************************************************************************************************************
//UI and others
public Text pickuptext;
  //Pick up objects
  private int pickupcounter;

  void Awake(){
    //This is the default downwards position, might need to fix it to acomodate the player correctly, second f means attack position from the center of the player
    sword.transform.position =new Vector3(0.0f, -0.9f, -1f);
    freeze=false;
    stickyfloor=false;
    //Same but for the rotation
    lookDown();
  }
  void Start(){
    //Access in unity the Component of the object PlayerGameObject called RigidBody2D
    rb2d=GetComponent<Rigidbody2D> ();
    attackstatus = sword.GetComponent<SpriteRenderer>();
    //Hitbox of the attack
    attackhitbox=sword.GetComponent<BoxCollider2D>();
    //Pickup count is 0
    pickupcounter=0;
    setCountText();
  }

  void FixedUpdate (){
    Debug.Log(stickyfloor);
    //Is the player trying to move again
    if (willmoveagain==true){
      //Will the game allow him to move again
      if (stickyfloorcounter>stickyfloorcounterlimit){
        //Player is allowed to move, and variables are reset
        stickyfloor=false;
        willmoveagain=false;
        stickyfloorcounter=0.0f;
        Debug.Log("WAITINGGGGGGGGGGGGG");
      }
      else{
        //The player is not allowed to move and will increase the counter
        stickyfloorcounter++;
      }
    }

  //Movement section*****************************************************************************************************************************************************************
      //Moving Vertically
    float moveVertical=0;
    //Moving Horizontally
    float moveHorizontal=0;
    if (freeze==false){
      if(Input.GetKey("w")){
        moveVertical=1;
        direction=1;
        //Rotate the player center to look forward
        lookUp();
      }
      else if(Input.GetKey("s")){
        moveVertical=-1;
        direction=3;
        //Rotate the player center to look backwards, also the default
        lookDown();
      }

      //Moving Horizontally
      if(Input.GetKey("a")){
        moveHorizontal=-1;
        direction=2;
        lookLeft();
      }
      else if(Input.GetKey("d")){
        moveHorizontal=1;
        direction=4;
        lookRight();
      }
    }
    else{
      swinglagcounter++;
      //freeze=true;
      stickyfloor=true;
      if (swinglagcounter>swinglag){
        stickyfloor=false;
        //freeze=false;
        swinglagcounter=0.0f;
      }

    }
//Attack Section**************************************************************************************************************************************************************************
    //Checks if the player is already attacking, if the player NOT has attacked, then it will enter
    if (Input.GetKey("space")&&isAttacking==false){
      isAttacking=true;
      //freeze=true;
      //Player can't move but can rotate
      stickyfloor=true;
      Debug.Log("YOU ATTACKED");
      attackstatus.sprite=activeattack; //REVISE THIS
      //Player does not want to move, since it attacked
      willmoveagain=false;
      //Debug.Log(freeze);

      //Hitbox of attack enabled
      attackhitbox.enabled=true;
    }

    //Checks if the player is already attacking, if the player has attacked, then it will enter
    if (isAttacking==true){
      attackcooldowncounter++;
      if (attacktime>attacktimecounter){
        attacktimecounter++;
        Debug.Log("The hitbox is damaging");
        attackstatus.sprite=activeattack;
      }
      else{
        Debug.Log("WAITING FOR ATTACK TO BE READY");
        attackstatus.sprite=recharge;

        //Hitbox of attack disabled
        attackhitbox.enabled=false;
      }
      if (attackcooldowncounter>attackcooldown){
        //DestroyImmediate(sword, true);
        attackcooldowncounter=0;
        isAttacking=false;
        Debug.Log("ATTACK READY");
        attackstatus.sprite=readyattack;
        //Resets the counter for the next attack hitbox;
        attacktimecounter=0.0f;
        swinglagcounter=0.0f;
        willmoveagain=true;
      }
    }

    //This vector will determine the movement of the player character onscreen
    Vector2 movement=new Vector2 (moveHorizontal, moveVertical);
    if(stickyfloor==false){
      //Use the function AddForce to make the player's rigidbody move
      rb2d.AddForce(movement*speed);
    }

  }
  //Directional functions, used to make the player look a specific way
  //lookDown is the default
  void lookDown(){
    playerCenter.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
  }
  void lookUp(){
    playerCenter.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180f);
  }
  void lookLeft(){
    playerCenter.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 270f);
  }
  void lookRight(){
    playerCenter.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
  }
//Counter for pickup objects*********************************************************************************************************************************************************
  void OnTriggerEnter2D(Collider2D collider){
    if (collider.gameObject.CompareTag("Pickup")){
      collider.gameObject.SetActive(false);
      pickupcounter=pickupcounter+1;
      setCountText();
    }


  }
  void setCountText(){
    pickuptext.text="Pickups: "+pickupcounter.ToString();
  }

}
