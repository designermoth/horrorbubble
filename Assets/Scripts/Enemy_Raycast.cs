using UnityEngine;

public class Enemy_Raycast : MonoBehaviour
{
    Transform playerPos;
    LayerMask layerMask;
    AudioLowPassFilter audioPassFilter;

    private int frames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layerMask = LayerMask.GetMask("Player", "Walls");
        audioPassFilter = GetComponentInChildren<AudioLowPassFilter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frames++;
        if (frames % 5 == 0) //só p n correr every frame
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer

            if (Physics.Raycast(transform.position, (playerPos.position - transform.position), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    print("Player Hit by raycast");
                    audioPassFilter.enabled = false;
                }
                if (hit.transform.gameObject.CompareTag("Wall"))
                {
                    print("Walls Hit by raycast");
                    audioPassFilter.enabled = true;

                }

                Debug.DrawRay(transform.position, (playerPos.position - transform.position) * hit.distance, Color.yellow);
            }
            frames++;
            if (frames >= 50)
                frames = 0;
            else
            {
                Debug.DrawRay(transform.position, (playerPos.position - transform.position) * 1000, Color.white);
                Debug.Log("Did not Hit");

            }
        }


    }
}
