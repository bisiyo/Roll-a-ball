using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public Text countText;
	public Text winText;
    public Text acc;
    public Text trialText;
    public Transform pickups;
    public string level = "0";
    private Vector3 originalPos;
    private bool canMove = false;
    public bool mobileBuild = true;
    private AudioSource audioSource;
    private Rigidbody rb;
	private int count;
    Vector3 movement = new Vector3();
    public int trial = 1;

    void Start ()
	{
		rb = GetComponent<Rigidbody>();

        if (mobileBuild) InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);

        audioSource = GetComponent<AudioSource>();

        winText.text = "";

        SetCountText();
        trialText.text = "Life used:" + trial.ToString();

    }

    void FixedUpdate ()
	{


        if (mobileBuild)
        {
            Vector3 a = UnityEngine.InputSystem.Accelerometer.current.acceleration.ReadValue();
            acc.text = a.ToString("F6");
            movement = new Vector3(a.x, 0.0f, a.y);
        }



        if (canMove) rb.AddForce (movement * speed);
	}


	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
            if (canMove)
            {
                audioSource.Play();

                other.gameObject.transform.parent.transform.Find("Pick Ups"); ;
                other.gameObject.transform.parent = other.gameObject.transform.parent.transform.parent.transform.Find("Back Up");

                other.gameObject.SetActive(false);

                count = count + 1;

                SetCountText();
            }
		}

        if (other.gameObject.tag == "Ground")
        {
            if (level != other.gameObject.transform.parent.name)
            {
                winText.text = "";
                pickups = other.gameObject.transform.parent.transform.Find("Pick Ups"); 
                level = other.gameObject.transform.parent.name;
                //other.gameObject.transform.parent.GetComponent<TiltingGround>().enabled = true;
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
            trial++;

            trialText.text = "Life used:" + trial.ToString();

            winText.text = "";

            other.GetComponent<StartOriginalPos>().BackToOriginalPos();


        }





    }

    void SetCountText()
	{
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
        }
	}
}