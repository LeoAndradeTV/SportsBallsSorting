using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HandSpawner : MonoBehaviour
{
    [SerializeField] private Ball[] possibleBalls;
    [SerializeField] private Image nextBallSprite;

    private PlayerInputActions inputActions;

    private Ball currentHeldBall;
    private Ball nextBall;
    private HandController handController;

    // Start is called before the first frame update
    void Start()
    {
        handController = GetComponent<HandController>();
        InstantiateNextBall();
        ActionsManager.BallHasCollided += InstantiateNextBall;
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Drop.performed += Drop_performed;
    }

    private void Drop_performed(InputAction.CallbackContext obj)
    {
        DropBall();
    }

    private void OnDisable()
    {
        ActionsManager.BallHasCollided -= InstantiateNextBall;
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
        currentHeldBall = nextBall == null ?
            Instantiate(
                PickRandomFromArray(possibleBalls),
                transform) :
                Instantiate(nextBall,
                transform);
        currentHeldBall.ToggleBall(false);
        SetNextBall();
    }

    public void DropBall()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        currentHeldBall.DropBall();
        handController.DisableMovement();

    }

    public Ball GetCurrentBall()
    {
        return currentHeldBall;
    }
}
