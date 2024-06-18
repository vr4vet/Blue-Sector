using System.Collections;
using UnityEngine;

public class FactoryFishState : MonoBehaviour
{
    // ------------ Public Variables ------------

    public enum Tier
    {
        Tier1,
        Tier2,
        Tier3,
        BadQuality
    }

    public enum GuttingState
    {
        GuttingSuccess,
        GuttingIncomplete,
        GuttingFailure,
    }

    // The current public states of the fish.
    public Tier fishTier;
    public GuttingState guttingState;
    public bool ContainsMetal = false;
    public bool Stunned;
    public bool correctlyBled;

    // ------------ Editor Variables ------------

    [SerializeField]
    public Material _bleedingFish;

    // ------------------ Private Variables ------------------

    public Material metalMat;

    // ------------ Unity Functions ------------

    void Awake()
    {
        _bleedingFish = Resources.Load<Material>("Materials/Fish/salmonBleeding");
        metalMat = Resources.Load<Material>("Materials/Old project material/Conveyor/Metal");
    }

    void Start()
    {
        if (fishTier == Tier.BadQuality)
        {
            Stunned = true;
        }
        // If the fish is alive, it should start moving
        if (Stunned == false)
        {
            StartCoroutine(AliveFish());
        }
    }

    // ------------ Public Functions ------------

    /// <summary>
    /// When the player cuts the gills of the fish.
    /// </summary>
    public void CutFishGills()
    {
        Renderer fishMaterial = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        
        if (fishTier == Tier.BadQuality)
        {
            GameManager.Instance.PlaySound("incorrect");
            return;
        }
        if (!Stunned && !correctlyBled)
        {
            correctlyBled = true;
            fishMaterial.material = _bleedingFish;
            GameManager.Instance.PlaySound("incorrect");
            return;
        }
        if (Stunned && !correctlyBled)
        {
            correctlyBled = true;
            fishMaterial.material = _bleedingFish;
            GameManager.Instance.PlaySound("correct");
            return;
        }
    }

    /// <summary>
    /// When the player cuts the body of the fish.
    /// </summary>
    public void CutFishBody()
    {
        // To make the cutting process more forgiving, we will not penalize the player for a bad cut if the fish is already cut correctly
        // The player will only be penalized for a bad cut if the fish is alive or has not been cut yet
        if (!correctlyBled)
        {
            correctlyBled = false;
            GameManager.Instance.PlaySound("incorrect");
        }
    }

    /// <summary>
    /// When the knife is chipped for the first time add metal to fish and update state.
    /// </summary>
    public void PlaceMetalInFish()
    {
        ContainsMetal = true;
        GameObject neck = transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject Metal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Metal.name = "MetalPiece";
        Metal.transform.localScale = new Vector3(0.02f,0.025f,0.003f);
        Metal.GetComponent<Renderer>().material = metalMat;
        Metal.transform.SetParent(neck.transform);
        Metal.transform.position = neck.transform.position + new Vector3(-0.007f,0.04f,0f);
    }

    // ------------ Private Functions ------------

    /// <summary>
    /// Coroutine that makes the fish move while alive.
    /// </summary>
    /// <returns> The fish will move in a random direction </returns>
    private IEnumerator AliveFish()
    {
        // Every time the fish "gathers strength" it will shake 4 times
        for (int i = 0; i < 4; i++)
        {
            // Each repetition will have a random delay between 0.5 and 1 seconds
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));

            // The fish head and tail rigidbodies
            Rigidbody head = transform.GetChild(2).transform.GetChild(0).GetComponent<Rigidbody>();
            Rigidbody tail = transform.Find("Salmon_Armature/Head/Neck/Back_1/Back_2/Back_3/Back_4/Back_5/Tail/Tail_end").GetComponent<Rigidbody>();

            // Makes the fish head move upwards
            head.AddForce(Vector3.up * 800, ForceMode.Force);

            // Makes the fish tail move upwards
            tail.AddForce(Vector3.up * 500, ForceMode.Force);

            // If the fish is no longer alive, stop the coroutine
            if (Stunned == true)
            {
                yield break;
            }
        }

        // Wait for 2-8 seconds before repeating
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(AliveFish());
    }
}
