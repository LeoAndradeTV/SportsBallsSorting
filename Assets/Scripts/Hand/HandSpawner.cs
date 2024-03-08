using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HandSpawner : MonoBehaviour
{
    [SerializeField] private Ball[] possibleBalls;
    [SerializeField] private Image nextBallSprite;
    [SerializeField] private Image nextBallColor;
    [SerializeField] private Material hologramMaterial;

    private PlayerInputActions inputActions;

    private Ball currentHeldBall;
    private Ball currentBallHologram;
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
        nextBallColor.color = nextBall.GetNextBallColor();
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
        InstantiateHologram(currentHeldBall);
        SetNextBall();
    }

    private void InstantiateHologram(Ball heldBall)
    {
        currentBallHologram = Instantiate(heldBall, transform);
        currentBallHologram.ToggleBall(false);
        currentBallHologram.GetComponent<MeshRenderer>().material = hologramMaterial;
    }

    public void DropBall()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        if (currentHeldBall == null || currentBallHologram == null) return;

        currentHeldBall.DropBall();
        Destroy(currentBallHologram.gameObject);
        handController.DisableMovement();

    }

    public Ball GetCurrentBall()
    {
        return currentHeldBall;
    }

    public Ball GetCurrentHologram()
    {
        return currentBallHologram;
    }

    public Vector3 GetCurrentBallRadius()
    {
        float radius = currentBallHologram.GetComponent<SphereCollider>().radius * currentBallHologram.transform.localScale.y;
        return new Vector3(0, radius, 0);
    }
}
