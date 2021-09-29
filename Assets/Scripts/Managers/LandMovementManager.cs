using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LandMovementManager : MonoBehaviour
{
    public GameObject HeroParty;
    public GameObject Hero;

    NavMeshAgent heroAgent;
    Animator horseAnim;
    Animator heroAnim;
    bool agentMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        heroAgent = HeroParty.GetComponent<NavMeshAgent>();
        horseAnim = HeroParty.GetComponent<Animator>();
        heroAnim = Hero.GetComponent<Animator>();

        if(PlayerPrefs.HasKey("LastPlaceOnLandX"))
        {
            float x = PlayerPrefs.GetFloat("LastPlaceOnLandX");
            float y = PlayerPrefs.GetFloat("LastPlaceOnLandY");
            float z = PlayerPrefs.GetFloat("LastPlaceOnLandZ");

            // 412, 10, 412
            //HeroParty.transform.position = new Vector3(x, y, z);
            HeroParty.GetComponent<NavMeshAgent>().Warp(new Vector3(x, y, z));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Touch to navigate to
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 300, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                if(hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    if(!agentMoving)
                    {
                        horseAnim.SetBool("isIdle", false);
                        horseAnim.CrossFade("run", 0.01f);
                        heroAnim.CrossFade("Ride", 0.01f);
                    }

                    heroAgent.SetDestination(hitInfo.point);

                    agentMoving = true;
                    return;
                }
            }
        }

        // Test if hero has reached to its destination
        if (agentMoving)
        {
            if(heroAgent.pathStatus == NavMeshPathStatus.PathComplete && heroAgent.remainingDistance <= 0.03)
            {
                Stop();
            }
        }
    }

    public void Stop()
    {
        horseAnim.CrossFade("idle", 0.01f);
        heroAnim.CrossFade("NW_Idle", 0.01f);
        horseAnim.SetBool("isIdle", true);
        agentMoving = false;
    }

    public void AttackTo(int index)
    {
        switch(index)
        {
            case 0:
                SceneManager.LoadScene("TemporaryArenaFarm");
                break;
            case 1:
                SceneManager.LoadScene("TemporaryArenaCemetary");
                break;
        }
    }
}
