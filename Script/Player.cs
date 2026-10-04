using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Ryan Joshua Smith Parking Pots Game Player Movement Script

public class Player : MonoBehaviour
{
    Gyroscope movementGyro;

    [Header("Player Variables")]
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float playerSpeed;
    [SerializeField] float controlSensitivty = 2f;

    [Header("Player Lives")]
    [SerializeField] TMP_Text playerLivesText;
    private int playerLivesCount;

    [Header("Player Respawn")]
    [SerializeField] GameObject playerParent;
    [SerializeField] GameObject playerRespawnPositions;

    [Header("End of Level State")]
    [SerializeField] GameObject loseStateWindow;
    [SerializeField] GameObject winStateWindow;

    [Header("Goal Check")]
    static int playerGoalCount;
    [SerializeField] int playerSceneAmount;

    [Header("Collectable")]
    [SerializeField] static int collectableCount;
    [SerializeField] TMP_Text collectableText;
    private int storedCollectables;

    [Header("VFX")]
    [SerializeField] ParticleSystem movementDust;

    [Header("Traffic Cone")]
    [SerializeField] float bounceAmount;
    private bool isBouncing = false;

    [Header("Sound Design")]
    [SerializeField] AudioSource collisionSFX;
    [SerializeField] AudioSource respawnSFX;
    [SerializeField] AudioSource failSFX;
    [SerializeField] AudioSource collectableSFX;
    [SerializeField] AudioSource winSFX;

    [Header("Player Model")]
    [SerializeField] GameObject characterPosition;
    [SerializeField] GameObject[] characterPrefabs;
    [SerializeField] int requiredCharacter;

    [SerializeField] Animator cameraShake;

    [Header("Required Character Icon")]
    [SerializeField] Sprite[] characterIcons; // Must Correspond With Character Type Index
    [SerializeField] Image requiredCharacterIcon; // Image To Swap With Icon


    //-----------------------------------Start is called once upon creation-------------------------
    private void Start()
    {
        Application.targetFrameRate = 60;

        playerLivesCount = 4;
        DetermineCharacterAmount();
        PlayerPrefs.GetInt("Collectables");

        requiredCharacter = CharacterSelect.activeCharacter; // Gathers Selected Characters's Index Number
        Instantiate(characterPrefabs[requiredCharacter], characterPosition.transform);

        requiredCharacterIcon.sprite = characterIcons[requiredCharacter]; // Shows The Corresponding Character Icon On HUD

        movementGyro.enabled = true;
    }

    //-----------------------------------Player Control, Model Rotation & VFX-------------------------
    private void FixedUpdate()
    {
        Vector3 tilt;

        if (SystemInfo.supportsGyroscope) // Determining whether the Phone Can Use Gyroscopic Control and if so use it for Moving, if Not The use Acceleration As Typically cheaper
        {
            Vector3 gyro = movementGyro.gravity;

            tilt = new Vector3(-gyro.y, gyro.x, gyro.z);
        }
        else
        {
            Vector3 accel = Input.acceleration;

            tilt = new Vector3(-accel.y, accel.x, accel.z);
        }

        // Solution to movement made with webgl in mind. Acts as if vertical, not horizontal (DO NOT USE IF WEBGL BUILD)
        tilt = new Vector3(tilt.y, -tilt.x, 0f);

        // Sensitive Movement, by Creating New Direction Using Previously determined Tilt Mulitplied By Alterable Variable For Best Feel
        playerRigidbody.linearVelocity = new Vector3((tilt.x * controlSensitivty) * playerSpeed, 0f, (tilt.y * controlSensitivty) * playerSpeed);

        Quaternion newTurnVal = Quaternion.LookRotation(new Vector3(-tilt.x, 0f, -tilt.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, newTurnVal, 0.2f);   // Rotation of Player Model, Visually Demonstrates The Way They Are Facing

        if (playerLivesCount == 0)
        {
            loseStateWindow.SetActive(true); // Visual Indication of Failure
            failSFX.Play();
            Time.timeScale = 0; // Stops Player From Unexpectedly Breaking Game By Moving
        }

        if (playerRigidbody.linearVelocity.magnitude > 1f) // It's Never Zero Realistically
        {
            if (!movementDust.isPlaying)
                movementDust.Play();
        }
        else // Stops Particle System If Player Not Moving
        {
            if (movementDust.isPlaying)
                movementDust.Stop();
        }

        if (isBouncing == true) 
        {
            playerRigidbody.AddForce(transform.forward * bounceAmount, ForceMode.Impulse); // Worst Way To Go About This But Doesn't feel bad (Pushes Player Back Until isBouncing is False)
        }

    }

    //-----------------------------------How The Player Reacts Upon Impact-------------------------
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Player[] objects = FindObjectsByType<Player>(FindObjectsSortMode.None);

            foreach (Player obj in objects)
            {
                obj.RespawnFunction();
            }

            RespawnFunction();
            playerLivesCount--; // Removes a life
            playerLivesText.text = playerLivesCount.ToString(); // Updates Life Count
        }

        if (collision.gameObject.CompareTag("Collectable"))
        {
            collectableCount++; // Adds a Point
            collectableText.text = collectableCount.ToString(); // Updates Collectable Count
            collectableSFX.Play();
            Destroy(collision.gameObject); // Removes Collectable from Scene
        }

        if (collision.gameObject.CompareTag("Cone"))
        {
            StartCoroutine(TrafficConeBounce());
            cameraShake.Play("Camera Shake");
        }

        if (collision.gameObject.CompareTag("Untagged"))
        {
            collisionSFX.Play();
            collisionSFX.pitch = UnityEngine.Random.Range(0.5f, 1.2f);
        }
    }

    private IEnumerator TrafficConeBounce()
    {
        isBouncing = true;
        yield return new WaitForSeconds(0.2f);
        isBouncing = false;
    }

    public void RespawnFunction()
    {
        playerParent.transform.position = playerRespawnPositions.transform.position; // Moves Player to Position of Choice
        respawnSFX.Play();
        respawnSFX.pitch = UnityEngine.Random.Range(0.5f, 1.2f);
    }

    //-----------------------------------How The Player Wins Level-------------------------

    private void DetermineCharacterAmount()
    {
        if (LevelSelect.activeLevel == 0 || LevelSelect.activeLevel == 1) // Determines The count for How many Players should be In Scene
        {
            playerSceneAmount = 2; // Simply if the first or second Level is being played the goal is set to 2 
        }

        else // Temporary Else statement but can easily be Changed if Additional Levels are Made
        {
            playerSceneAmount = 3;
        }
    }

    private void OnTriggerEnter(Collider other) // Once Player Collides with Object with IsTrigger active
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            playerGoalCount++; // Adds One, Once Player Moves Into Goal 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            playerGoalCount--; // Once Player Leaves Goal It Removes One From Count so Player cannot cheat
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Goal") && playerRigidbody.linearVelocity.magnitude < 0.5f) // Prevents Cheating By Waiting Until Still
        {
            if (playerGoalCount == playerSceneAmount) // Compares Count of Players In Goal Is The Same as The Amount In Scene
            {
                winStateWindow.SetActive(true); // Indication & Reward
                Time.timeScale = 0; // Temporary Pause
                winSFX.Play();
                StoreCollectable(); // Updates Overall Collectable Progress
                CostumeUponComplete(); // Updates Costume States
                playerGoalCount = 0;
            }
        }
    }

    //-----------------------------------Adding Newly Acquired Collectables-------------------------

    private void StoreCollectable()
    {
        storedCollectables = PlayerPrefs.GetInt("Collectables"); // Gathers amount of collectables player had Before level, puts it as a variable to add next
        storedCollectables += collectableCount; // Adds on what Has been Collected Just now to the Previously Collected

        PlayerPrefs.SetInt("Collectables", storedCollectables);  // Updates said Amount
        PlayerPrefs.Save();  
    }

    //-----------------------------------Unlocking Costume-------------------------

    private void CostumeUponComplete()
    {
        // Repeated code for Simple check for which Level is Active So The Unlocked Costume Is correct

        if (LevelSelect.selectedLevel == 0)
        {
            PlayerPrefs.SetInt("officeCharacterState", 1); // 1 Simply means unlocked
        }
        else if (LevelSelect.selectedLevel == 1)
        {
            PlayerPrefs.SetInt("chefCharacterState", 1);
        }

        PlayerPrefs.Save(); // Saves all that's in the midst Of being Changed
    }
}