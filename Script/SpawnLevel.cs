using UnityEngine;

// Ryan Joshua Smith Determine Stage Script

public class SpawnLevel : MonoBehaviour
{
    [Header("3D Models")]
    [SerializeField] GameObject[] levelPrefabs; // Must Be Arranged In Same Order As Previous Scene

    [Header("Chosen Level & Character")]
    [SerializeField] int requiredLevel;  // Index Number For Previously Choosen Level

    [Header("Positions")]
    [SerializeField] GameObject levelPosition; // For Correct Transform 


    //-----------------------------------Start is called once upon creation-------------------------
    private void Awake()
    {
        requiredLevel = LevelSelect.activeLevel; 

        Instantiate(levelPrefabs[requiredLevel], levelPosition.transform); // Places Correct Level Model With Correct Position, Size & Rotation
    }
}