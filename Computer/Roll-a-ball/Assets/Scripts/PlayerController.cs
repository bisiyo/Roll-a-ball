using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;
    public Transform pickups;
    public string level = "0";
    private Vector3 originalPos;
    private bool canMove = true;

    private AudioSource audioSource;
    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;
	private int count;

	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 

        audioSource = GetComponent<AudioSource>();

        winText.text = "";

        // Run the SetCountText function to update the UI (see below)
        SetCountText();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector

        if (canMove) rb.AddForce (movement * speed);
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
            if (canMove)
            {
                audioSource.Play();

                other.gameObject.transform.parent.transform.Find("Pick Ups"); ;
                other.gameObject.transform.parent = other.gameObject.transform.parent.transform.parent.transform.Find("Back Up");

                // Make the other game object (the pick up) inactive, to make it disappear
                other.gameObject.SetActive(false);

                // Add one to the score variable 'count'
                count = count + 1;

                // Run the 'SetCountText()' function (see below)
                SetCountText();
            }
		}

        if (other.gameObject.tag == "Ground")
        {
            if (level != other.gameObject.transform.parent.name)
            {
                winText.text = "";
                count = 0;
                pickups = other.gameObject.transform.parent.transform.Find("Pick Ups"); 
                level = other.gameObject.transform.parent.name;
                originalPos = transform.position;
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                canMove = true;

            }

            if (other.gameObject.GetComponent<Collider>().isTrigger)
            {
                winText.text = "";
                GameObject.Find(level).gameObject.SetActive(false);
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX ;
                canMove = false;

            }
        }

        if (other.gameObject.tag == "Enemy")
        {
            transform.position = originalPos;
            count = 0;
            countText.text = "Stars: " + count.ToString();

            Transform backup = GameObject.Find(level).transform.Find("Back Up");
           while (backup.childCount != 0)
            {
                backup.GetChild(0).gameObject.SetActive(true);
                backup.GetChild(0).transform.parent = backup.parent.Find("Pick Ups");
            }

            winText.text = "";

            Transform mobiles = backup.parent.Find("Mobiles");

            if (mobiles)
            {
                for (int i = 0; i < mobiles.childCount; i++)
                {
                    mobiles.GetChild(i).GetComponent<StartOriginalPos>().BackToOriginalPos();

                }
            }

            other.GetComponent<StartOriginalPos>().BackToOriginalPos();


        }





    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Stars: " + count.ToString ();

		if (pickups.childCount < 1) 
		{
            if (level != "5")
            {
			winText.text = "Go to Next Level !";

                GameObject.Find(level).transform.Find("Enemys").gameObject.SetActive(false);
                GameObject.Find(level).transform.Find("Hole").GetComponent<Renderer>().enabled = false ;
                GameObject.Find(level).transform.Find("Hole").GetComponent<BoxCollider>().isTrigger = true;

            }
            else
            {
                winText.text = "You win !!";
            }
            // Set the text value of our 'winText'
        }
	}
}