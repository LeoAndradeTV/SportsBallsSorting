using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSpawner : MonoBehaviour
{
    [SerializeField] private Ball[] possibleBalls;
    [SerializeField] private Image nextBallSprite;

    private Ball currentHeldBall;
    private Ball nextBall;
    private HandController handController;

    // Start is called before the first frame update
    void OnEnable()
    {
        handController = GetComponent<HandController>();
        InstantiateNextBall();
        ActionsManager.BallHasCollided += InstantiateNextBall;
    }

    private void OnDisable()
    {
        ActionsManager.BallHasCollided -= InstantiateNextBall;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

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

    private void SetNextBall()
    {
        Ball nextBall = PickRandomFromArray(possibleBalls);
        nextBallSprite.sprite = nextBall.GetBallSprite();
        this.nextBall = nextBall;
    }

    private void InstantiateNextBall()
    {
        currentHeldBall = nextBall == null ? Instantiate(PickRandomFromArray(possibleBalls), transform) : Instantiate(nextBall, transform);
        SetNextBall();
        ToggleActiveBall(false);
    }

    private void ToggleActiveBall(bool toggle)
    {
        currentHeldBall.GetComponent<Rigidbody>().useGravity = toggle;
        currentHeldBall.GetComponent<Collider>().enabled = toggle;
    }
}
