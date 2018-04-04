using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;
    public Text FoodText;

    private Animator _animator;
    private int _food;

    // Use this for initialization
    protected override void Start ()
    {
        _animator = GetComponent<Animator>();

        _food = GameManager.Instance.PlayerFoodPoints;

        FoodText.text = "Food: " + _food;
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = _food;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            _food += PointsPerFood;
            FoodText.text = "+" + PointsPerFood + " Food: " + _food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            _food += PointsPerSoda;
            FoodText.text = "+" + PointsPerSoda+ " Food: " + _food;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        if (hitWall != null) hitWall.DamageWall(WallDamage);
        _animator.SetTrigger("playerChop");

    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)
    {
        _animator.SetTrigger("playerHit");
        _food -= loss;
        FoodText.text = "-" + loss + " Food: " + _food;
        CheckIfGameOver();
    }

    // Update is called once per frame
	void Update ()
	{
	    if (!GameManager.Instance.PlayerTurn) return;

	    int horizontal = (int) Input.GetAxisRaw("Horizontal");
	    int vertical = (int) Input.GetAxisRaw("Vertical");

	    if (horizontal != 0)
	        vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        _food--;
        FoodText.text = "Food: " + _food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
            
        }

        CheckIfGameOver();

        GameManager.Instance.PlayerTurn = false;
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}
