using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpawner : MonoBehaviour
{
    [SerializeField] private Ball[] possibleBalls;

    private Ball currentHeldBall;

    // Start is called before the first frame update
    void OnEnable()
    {
        InstantiateRandomBall();
    }

    // Update is called once per frame
    void Update()
    {
        TryDroppingBall();   
    }

    private void TryDroppingBall()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        ToggleActiveBall(true);
        currentHeldBall.transform.parent = null;

        InstantiateRandomBall();
    }

    private Ball PickRandomFromArray(Ball[] balls) 
    {
        int randomIndex = Random.Range(0, possibleBalls.Length);
        return possibleBalls[randomIndex];
    }

    private void InstantiateRandomBall()
    {
        currentHeldBall = Instantiate(PickRandomFromArray(possibleBalls), transform);
        ToggleActiveBall(false);
    }

    private void ToggleActiveBall(bool toggle)
    {
        currentHeldBall.GetComponent<Rigidbody>().useGravity = toggle;
        currentHeldBall.GetComponent<Collider>().enabled = toggle;
    }
}
