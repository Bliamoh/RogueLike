using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BoardManager BoardScript;

    private int _level = 3;

	// Use this for initialization
	void Start ()
	{
	    BoardScript = GetComponent<BoardManager>();
	    InitGame();
	}

    void InitGame()
    {
        BoardScript.SetupScene(_level);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
