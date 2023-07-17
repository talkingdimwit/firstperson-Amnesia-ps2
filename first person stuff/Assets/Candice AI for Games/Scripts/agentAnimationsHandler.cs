using System;
using UnityEngine;
using UnityEngine.UI;
using CandiceAIforGames.AI;

public class agentAnimationsHandler : MonoBehaviour
{	

	//CandiceAI References
	public CandiceAIController candiceAIAgent;
	private float agentHealth;
	public float incomingAttackDamage = 10f;

	//objects
	[HideInInspector]
	private bool isThisAgentDead = false;
	public GameObject deathVSFX;
	public GameObject hitVSFX;
	public GameObject drop;
	public AudioSource hitSound;

	//Time buffers
	public float fxDestroyBuffer = 1f;
	public float dropDestroyBuffer = 5f;

	//UI & Animations
	[HideInInspector]
	public static bool isInteracting = false;
	public bool isInteractable = false;
	public bool hasOpenAnimation = false;
	public GameObject interactivePrompt; //can be canvas with label attached to this object
	public Animator animations;


	void Start()
	{
		//if no candice ai controller present, populate candiceAIAgent with one attached to this gameObject
		if (candiceAIAgent == null)
		{
			candiceAIAgent = gameObject.GetComponent<CandiceAIController>();
		}
	}

	void Update()
	{
		//check to see if player is interacting with this object via input
		isInteracting = EvaluateInput("e", true, true, false);
	}

	//avaluate if agent is dead or alive (this could be any object, not just enemies, so your chest)
	public bool EvaluateAgentStatus(float health)
	{
		bool returnValue = false;
		if (health <= 0)
		{
			returnValue = true;
		}
		return returnValue;
	}

	//anything that has a collider and enters another collider set as trigger
	void OnTriggerEnter(Collider col)
	{
		//if there is a candice ai reference
		if (col.gameObject.GetComponent<CandiceAIController>() != null)
		{
			//store incoming attack damage
			incomingAttackDamage = col.gameObject.GetComponent<CandiceAIController>().attackDamage;
		}
		//if colliders belong to player or a weapon
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Weapon")
		{

			Debug.Log("Before: Hit - agentHealth: " + agentHealth + " incomingAttackDamage: " + incomingAttackDamage + " isThisAgentDead: " + isThisAgentDead);

			//if object is damageable and therefore has a candiceAIController attached
			if (candiceAIAgent != null)
			{
				//calculate hit points for object
				candiceAIAgent.hitPoints -= incomingAttackDamage;
				//set health with new hit points
				agentHealth = candiceAIAgent.hitPoints;
				//evaluate if object is dead or alive
				isThisAgentDead = EvaluateAgentStatus(agentHealth);
			}
			//for interactables, do not attach CandiceAIController
			else {

				if (isInteractable) {

					//show label first
					if (interactivePrompt != null) {
						interactivePrompt.SetActive(true);
					}				

					//Assess input
					if (isInteracting)
					{

						//if this has an open animation
						if (hasOpenAnimation) {
							if (animations != null) {
								//you should have your Open animations triggering with Open
								animations.SetTrigger("Open");
							}
						}

						//drop something
						//if you have a drop attached
						if (drop != null)
						{
							GameObject dropped = Instantiate(drop, transform.position, Quaternion.identity);
							Destroy(dropped, dropDestroyBuffer);
							//also remove prefab from drop, so that you only get 1 drop after opening the interactable
							//you can comment this, for something other than chests or objects that are openable
							drop = null;
							interactivePrompt.SetActive(false);
							interactivePrompt = null;
						}
					}

				}

			}

			//play some hit sounds on triggers (also doubles as an action sound for interactables)
			if (!isInteractable) {
				if (hitSound != null) {
					hitSound.Play();
				}
				
			}
			
			Debug.Log("After: Hit - agentHealth: " + agentHealth + " incomingAttackDamage: " + incomingAttackDamage + " isThisAgentDead: " + isThisAgentDead);
		}

		//if object is not dead
		if (!isThisAgentDead)
		{
			//if you have a vsfx attached
			if (hitVSFX != null)
			{
				GameObject hit = Instantiate(hitVSFX, transform.position, Quaternion.identity);
				Destroy(hit, fxDestroyBuffer);
			}
		}
		//if you die
		else
		{
			//if you have a drop attached
			if (drop != null)
			{
				GameObject dropped = Instantiate(drop, transform.position, Quaternion.identity);
                Destroy(dropped, dropDestroyBuffer);
            }
			//if you have a death vsfx
			if (deathVSFX != null)
			{
				GameObject dead = Instantiate(deathVSFX, transform.position, Quaternion.identity);
				Destroy(dead, fxDestroyBuffer);
			}
			//finally, since we're dead, destroy this gameObject
			Destroy(gameObject, 0.35f);
		}

	}

	//while object is inside trigger
	void OnTriggerStay(Collider col) {

		if (isInteractable && col.gameObject.tag == "Player")
		{

			//show label first
			if (interactivePrompt != null) {
				interactivePrompt.SetActive(true);
			}			

			//Assess input
			if (isInteracting)
			{

				//if this has an open animation
				if (hasOpenAnimation)
				{
					if (animations != null)
					{
						//you should have your Open animations triggering with Open
						animations.SetTrigger("Open");						
						if (hitSound != null)
						{
							hitSound.Play();
						}						
					}
				}

				//drop something
				//if you have a drop attached
				if (drop != null)
				{
					GameObject dropped = Instantiate(drop, transform.position, Quaternion.identity);
					Destroy(dropped, dropDestroyBuffer);
					//also remove prefab from drop, so that you only get 1 drop after opening the interactable
					//you can comment this, for something other than chests or objects that are openable
					drop = null;
					interactivePrompt.SetActive(false);
					interactivePrompt = null;
				}
			}

		}
	}

	//anything that has a collider and exits another collider set as trigger
	void OnTriggerExit(Collider col) {
		//if this is set as interactable, when exiting interactable triggers, hide UI label
		if (isInteractable)
		{
			//hide label
			if (interactivePrompt != null) {
				interactivePrompt.SetActive(false);
			}			
		}

	}

	//show a UI label if this object is set as interactable
	void ShowHideUILabel() {

		//if this is interactable
		if (isInteractable && !interactivePrompt.active)
		{
			interactivePrompt.SetActive(true);
		}
		else {
			interactivePrompt.SetActive(false);
		}
	
	}

	//evaluate user input
	//supports using the Unity Input System defined in Input in your Project Settings, and simple key presses, (if you want to just give it key presses, just pass the key like lshift, lctrl, enter, space etc, and not have to use the unity Input system)
	//also supports while pressed, on key press down, and on keypress up
	private bool EvaluateInput(string input, bool isKey, bool down, bool up) {

		bool returnValue = false;

		//if key then just give it a key
		if (isKey)
		{
			if (down)
			{
				returnValue = Input.GetKeyDown(input);
			}
			else if (up)
			{
				returnValue = Input.GetKeyUp(input);
			}
			else {
				returnValue = Input.GetKey(input);
			}

		}
		//otherwise uses unity input system
		else {
			if (down)
			{
				returnValue = Input.GetButtonDown(input);
			}
			else if (up)
			{
				returnValue = Input.GetButtonUp(input);
			}
			else
			{
				returnValue = Input.GetButton(input);
			}
		}

		return returnValue;


	}

}