using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureController : MonoBehaviour
{
    Vector2 dest = Vector2.zero;

    [SerializeField]
    private Transform player;

    private Rigidbody2D rigidbody;
    [SerializeField]
    private GestureDirState currentDirState;
    [SerializeField]
    private GestureDirState nextDirState;

    //Vector2 startPosition;
    float startTime;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;

    private readonly float minTouchStart = 0.5f;
    private readonly float minTouchThreshole = 0.2f;

    private PlayerController playerController;

    private void Start()
    {
        rigidbody = player.GetComponent<Rigidbody2D>();
        nextDirState = currentDirState = GestureDirState.None;
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))    // swipe begins
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))    // swipe ends
        {
            endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (startPosition != endPosition && startPosition != Vector3.zero && endPosition != Vector3.zero)
        {
            float deltaX = endPosition.x - startPosition.x;
            float deltaY = endPosition.y - startPosition.y;
            Debug.Log("  deltaX " + deltaX + "deltaY" + deltaY);
            if ((deltaX > minTouchStart || deltaX < -minTouchStart) && (deltaY >= -minTouchThreshole || deltaY <= minTouchThreshole))
            {
                if (startPosition.x < endPosition.x) // left right
                {
                    nextDirState = GestureDirState.Right;
                }
                else // swipe RTL
                {
                    nextDirState = GestureDirState.Left;
                }
            }

            if ((deltaY > minTouchStart || deltaY < -minTouchStart) && (deltaX >= -minTouchThreshole || deltaX <= minTouchThreshole))
            {

                if (startPosition.y < endPosition.y) // swipe Up down
                {
                    nextDirState = GestureDirState.Up;
                }
                else // swipe RTL
                {
                    nextDirState = GestureDirState.Down;
                }
            }
                startPosition = endPosition = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if ((currentDirState == GestureDirState.None || currentDirState != nextDirState) && CanTurn())
        {
            currentDirState = nextDirState;
            playerController.OnDirectionChange(GetDirVetor(currentDirState));
        }

        if (currentDirState != GestureDirState.None )
        {
            rigidbody.velocity = GetDirVetor(currentDirState) * playerController.speed;
        }
    }

    private bool CanTurn()
    {
        Vector2 dir = GetDirVetor(nextDirState);
        Vector2 pos = player.transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit && hit.collider) ? hit.collider.tag == "Player" : true;
    }

    private Vector2 GetDirVetor(GestureDirState State)
    {
        Vector2 dirVec = Vector2.zero;
        switch (State)
        {
            case GestureDirState.Up:
                dirVec = Vector3.up;
                break;
            case GestureDirState.Down:
                dirVec = Vector3.down;
                break;
            case GestureDirState.Left:
                dirVec = Vector3.left;
                break;
            case GestureDirState.Right:
                dirVec = Vector3.right;
                break;
        }
        return dirVec;
    }

}

public enum GestureDirState
{
    None,
    Right,
    Left,
    Up,
    Down
}
