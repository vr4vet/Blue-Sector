using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilletCut : MonoBehaviour
{

    public GameObject cutFishFront;
    public GameObject cutFishBack;
    public GameObject cutFishFrontBack;
    public GameObject cutFishBadFront;
    public GameObject cutFishBadBack;
    public GameObject cutFishBadFrontBack;
    public GameObject cutFishBadMidFront;
    public GameObject cutFishBadMidBack;
    public GameObject cutFishBadMidFrontBack;
    public GameObject cutFishBadMid;
    public GameObject cutFishMid;
    public GameObject cutFishMidFront;
    public GameObject cutFishMidBack;
    public GameObject cutFishMidFrontBack;
    public Vector3 pos;
    public Quaternion rot;
    public FiletManager manager;

    // Uses the colliding fillet's id, and which trigger on the fish that collides to determine which cut-object to instansiate

    void OnTriggerEnter(Collider other)
    {
        var id = other.transform.root.gameObject.GetComponent<FilletID>();
        pos = other.transform.position;
        rot = other.transform.rotation;

        // GOOD FISH CUTTING
        if (other.gameObject.name == "FrontCollider" && id.good == true)
        {
            


            if (id.cutback == true && id.cutfront == false && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadBack").gameObject);
            }
            else if (id.cutback == true && id.cutfront == false && id.cutmiddle == true)
            {
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMidFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadBack").gameObject);
            }
            else if (id.cutback == false && id.cutfront == false && id.cutmiddle == true)
            {
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMidFront, pos, rot);
            }
            else
            {
                Destroy(other.transform.root.gameObject);
                Instantiate(cutFishFront, pos, rot);
            }
            manager.filletWrong += 1;
        }

        if (other.gameObject.name == "BackCollider" && id.good == true)
        {

            if (id.cutback == false && id.cutfront == true && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }

                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else if (id.cutback == false && id.cutfront == true && id.cutmiddle == true)
            {
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMidFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else if (id.cutback == false && id.cutfront == false && id.cutmiddle == true)
            {
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMidBack, pos, rot);
            }
            else
            {
                Destroy(other.transform.root.gameObject);
                Instantiate(cutFishBack, pos, rot);
            }
            manager.filletWrong += 1;

        }
        if (other.gameObject.name == "MiddleCollider" && id.good == true)
        {

            pos = transform.TransformPoint(-0.01f, 0.0f, 0.14f);

            if (id.cutback == false && id.cutfront == true && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMidFront, pos, rot);
                
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else if (id.cutback == true && id.cutfront == false && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMidBack, pos, rot);
                
                Destroy(clone.transform.Find("BadBack").gameObject);
            }
            else if (id.cutback == true && id.cutfront == true && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMidFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadBack").gameObject);
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else
            {
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishMid, pos, rot);
                
            }
            manager.filletWrong += 1;

        }
        // BAD FISH CUTTING
        if (other.gameObject.name == "FrontCollider" && id.good == false)
        {
            if (id.cutback == true && id.cutfront == false && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadBack").gameObject);
            }
            else if (id.cutback == true && id.cutfront == false && id.cutmiddle == true)
            {
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadMidFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadBack").gameObject);
            }
            else if (id.cutback == false && id.cutfront == false && id.cutmiddle == true)
            {
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadMidFront, pos, rot);
            }
            else
            {
                Destroy(other.transform.root.gameObject);
                Instantiate(cutFishBadFront, pos, rot);
            }
        }

        if (other.gameObject.name == "BackCollider" && id.good == false)
        {
            if (id.cutback == false && id.cutfront == true && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }

                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else if (id.cutback == false && id.cutfront == true && id.cutmiddle == true)
            {
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadMidFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else if (id.cutback == false && id.cutfront == false && id.cutmiddle == true)
            {
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadMidBack, pos, rot);
            }
            else
            {
                Destroy(other.transform.root.gameObject);
                Instantiate(cutFishBadBack, pos, rot);
            }

        }
        if (other.gameObject.name == "MiddleCollider" && id.good == false)
        {
            pos = transform.TransformPoint(-0.01f, -0.0f, 0.14f);

            if (id.cutback == false && id.cutfront == true && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadMidFront, pos, rot);
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else if (id.cutback == true && id.cutfront == false && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadMidBack, pos, rot);
                Destroy(clone.transform.Find("BadBack").gameObject);
            }
            else if (id.cutback == true && id.cutfront == true && id.cutmiddle == false)
            {
                if (other.transform.root.Find("BadBack") != null)
                {
                    other.transform.root.Find("BadBack").SetParent(null);
                }
                if (other.transform.root.Find("BadFront") != null)
                {
                    other.transform.root.Find("BadFront").SetParent(null);
                }
                Destroy(other.transform.root.gameObject);
                GameObject clone = Instantiate(cutFishBadMidFrontBack, pos, rot);
                Destroy(clone.transform.Find("BadBack").gameObject);
                Destroy(clone.transform.Find("BadFront").gameObject);
            }
            else
            {
                Destroy(other.transform.root.gameObject);
                Instantiate(cutFishBadMid, pos, rot);
            }
            manager.filletWrong += 1;
        }
    }
}
