using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public enum GameStatus{
    Playing,
    Winning,
    GameOver
}
public class playerControl : MonoBehaviour
{
    private int count;

    public Text countText;
    float angle;
    float radius;

    public Text GameText;
    public Text stopWatchText;

    bool isFalling;
    float fallingAnimationParam;

    GameStatus gameStatus;

    GameObject otherObjectHole;

    Stopwatch stopWatch = new Stopwatch();

    public AudioClip coinAudioClip;
    public AudioClip deathAudioClip;

    public AudioSource musicSource;

    void Start()
    {
        count = 0;

        GameText.text = "";

        setCountText();

        isFalling = false;
        fallingAnimationParam = 1;

        gameStatus = GameStatus.Playing;

        otherObjectHole = null;

        angle = 0;
        radius = 0;

        stopWatch.Start();

        musicSource.clip = coinAudioClip;
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (gameStatus == GameStatus.GameOver)
        {
            fallingAnimationParam = Mathf.Max(fallingAnimationParam-.05f, 0);
            transform.localScale = new Vector2(fallingAnimationParam, fallingAnimationParam);
            transform.position = Vector2.MoveTowards(transform.position, otherObjectHole.transform.position, 1); 

            if( Input.GetKey("r"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (gameStatus == GameStatus.Winning)
        {
            if (Input.GetKey("r"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            radius = Mathf.Max(0, Mathf.Min(61, radius + (moveHorizontal / 10)));

            angle += 2 * Mathf.PI * Time.deltaTime * 10f;
            float x = Mathf.Cos(angle * Time.deltaTime) * radius;
            float y = Mathf.Sin(angle * Time.deltaTime) * radius;

            transform.position = new Vector2(x, y);
            transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime * radius / 2);
        }

        setCountText();

        //Vector2 velocity = new Vector2(Mathf.Cos(angle +90), Mathf.Sin(angle + 90));
        //angle += Time.deltaTime;
        //Vector2 movement = new Vector2(Mathf.Cos(angle+180), Mathf.Sin(angle+180)) * radius;
        //rb2d.AddForce(movement);

        //if( radius > 62)
        //{
        //    rb2d.AddForce(new Vector2(Mathf.Cos(angle + 180), Mathf.Sign(angle + 180)) * 5 );
        //}

        ////Store the current vertical input in the float moveVertical.
        //float moveVertical = Input.GetAxis("Vertical");

        ////Use the two store floats to create a new Vector2 variable movement.
        //Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        ////Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        //rb2d.AddForce(movement * speed);


    }

    //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            musicSource.Play();
        }
        else if (other.gameObject.CompareTag("Hole"))
        {
            gameStatus = GameStatus.GameOver;
            otherObjectHole = other.gameObject;
            stopWatch.Stop();
            musicSource.clip = deathAudioClip;
            musicSource.Play();
        }

        if (count >= 7)
        {
            gameStatus = GameStatus.Winning;
            stopWatch.Stop();
        }
    }

    void setCountText()
    {
        countText.text = "Count: " + count.ToString()+"/7";

        stopWatchText.text = "Elapsed Time: "+ stopWatch.Elapsed.Minutes.ToString()+":"+ stopWatch.Elapsed.Seconds.ToString();

        if (gameStatus == GameStatus.Winning)
        {
            GameText.text = "You Win! press 'R' to play again";
        }
        if (gameStatus == GameStatus.GameOver)
            GameText.text = "Game Over..Press 'R' to restart";
    }
}
