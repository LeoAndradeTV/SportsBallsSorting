using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpawner : MonoBehaviour
{
    [SerializeField] private Ball[] possibleBalls;

    private Ball currentHeldBall;
    private HandController handController;

    // Start is called before the first frame update
    void OnEnable()
    {
        handController = GetComponent<HandController>();
        InstantiateRandomBall();
        ActionsManager.BallHasCollided += InstantiateRandomBall;
    }

    private void OnDisable()
    {
        ActionsManager.BallHasCollided -= InstantiateRandomBall;
    }

    // Update is called once per frame
    void Update()
    {
        TryDroppingBall();   
    }

    private void TryDroppingBall()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        handController.DisableMovement();
        ToggleActiveBall(true);
        currentHeldBall.transform.parent = null;
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
