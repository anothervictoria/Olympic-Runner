using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    public float gravityModifier = 1.5f;
    private float highestPosition = 16f;
    private Rigidbody playerRb;
    private Vector3 playerPosition;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip hitSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();        
        playerPosition = transform.position;

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);

    }

    private Vector2 velocity;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            velocity = Vector3.up * floatForce;
            playerRb.velocity = Vector2.zero;
        }
        else
        {
            velocity = Vector2.zero;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        playerRb.AddForce(velocity, ForceMode.Impulse);

        // While space is pressed and player is low enough, float up


        playerPosition = playerRb.position;
        if (playerPosition.y > highestPosition)
        {
            Debug.Log("Test");
            playerPosition.y = highestPosition;
            playerRb.position = new Vector2(playerRb.position.x, playerPosition.y);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            playerAudio.PlayOneShot(hitSound, 1.0f);
        }

    }

}
