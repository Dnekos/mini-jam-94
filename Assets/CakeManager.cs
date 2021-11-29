using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CakeManager : MonoBehaviour
{
	Vector3 startPos;
	Quaternion startRot;
	Rigidbody rb;
	AudioSource hitsound;
	public bool Released; // stops controller from placing blocks (actually why not let them?)

	[SerializeField] Text Startbtn;
	[SerializeField] float DropForce = 2;
	[SerializeField] ParticleSystem confetti;
	[SerializeField] GameObject WinText, ArrowCanvas;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		hitsound = GetComponent<AudioSource>();
		startPos = transform.position;
		startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Release()
	{
		if (!Released)
		{
			rb.isKinematic = false;
			Released = true;
			Startbtn.text = "Reset";
			rb.AddForce(Vector3.down * DropForce, ForceMode.Impulse);
			ArrowCanvas.SetActive(false);
		}
		else
		{
			rb.isKinematic = true;

			transform.position = startPos;
			transform.rotation = startRot;
			Released = false;
			Startbtn.text = "Go!";

			WinText.SetActive(false);
			confetti.Stop();
			ArrowCanvas.SetActive(true);
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		hitsound.Play();
		if (collision.gameObject.tag == "Oven")
		{
			confetti.Play();
			WinText.SetActive(true);
		}
	}
}
