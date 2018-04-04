using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject GameMngr;

	void Awake () {
	    if (GameManager.Instance == null)
	    {
	        Instantiate(GameMngr);
	    }
	}
}
