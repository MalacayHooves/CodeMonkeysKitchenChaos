using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static Player LocalInstance { get; private set; }

    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;

    public static void ResetStaticData() { OnAnyPlayerSpawned = null; }

    [SerializeField] private Transform kitchenObjectHoldPoint;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private LayerMask collisionsLayerMask;

    [SerializeField] private List<Vector3> spawnPositionList;

    private bool isWalking = false;
    private float moveDistance;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;

    //private float playerHeight = 2f;
    private float playerRadius = .7f;
    private float interactDistance = 2f;

    private KitchenObject kitchenObject;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounterArg;
    }
    public event EventHandler OnPickedSomething;

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) LocalInstance = this;

        transform.position = spawnPositionList[(int)OwnerClientId];

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (!IsOwner) return;
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() { kitchenObject = null; }

    public bool HasKitchenObject() { return kitchenObject != null; }

    private void HandleMovement()
    {
        //get input
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        //move player
        moveDistance = moveSpeed * Time.deltaTime;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        bool canMove = CanMoveInDirection(moveDir);
        //!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) //cannot move towards moveDir
        {
            //attempt only X move
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && CanMoveInDirection(moveDirX);

            if (canMove)
            {
                //can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                //cannot move only on the X
                //attempt only Z movement
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && CanMoveInDirection(moveDirZ);

                if (canMove)
                {
                    //can move only on the Z
                    moveDir = moveDirZ;
                }
                //else
                //{
                //cannot move in any direction
                //}
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        //rotate player
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);

        //set bool for animation
        isWalking = moveDir != Vector3.zero;
    }

    private void HandleInteractions()
    {
        //get input
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //Has clearCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private bool CanMoveInDirection(Vector3 direction)
    {
        return !Physics.BoxCast
            (transform.position, Vector3.one * playerRadius, direction, Quaternion.identity, moveDistance, collisionsLayerMask);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void SetSelectedCounter(BaseCounter newSelectedCounter)
    {
        selectedCounter = newSelectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounterArg = selectedCounter });
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
