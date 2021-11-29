using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

	[SerializeField] Rigidbody cakerb;
	[SerializeField] float spinforce;
    // Start is called before the first frame update
    void Start()
    {
		cakerb.AddTorque(new Vector3(spinforce, spinforce, 0));
    }
	public void GoToScene(int index)
	{
		SceneManager.LoadScene(index);
	}
}
