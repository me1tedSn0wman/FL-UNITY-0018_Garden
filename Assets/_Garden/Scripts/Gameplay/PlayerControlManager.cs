using UnityEngine;
using UnityEngine.InputSystem;

public enum DragState { 
    Empty,
    Dragging,
}


public class PlayerControlManager : MonoBehaviour
{
    [SerializeField] private DragState dragState;

    public float raycastDistance = 10f;

    [SerializeField] private Vector2 crntMousePosition;
    [SerializeField] public Vector3 crntMouseWorldPositon;

    private Camera mainCamera;

    public void Awake()
    {
        mainCamera = GameplayManager.Instance.mainCamera;
    }

    public void OnMousePosition(InputAction.CallbackContext context) { 
        crntMousePosition = context.ReadValue<Vector2>();
        crntMouseWorldPositon = GetPointerWorldPosition(1);

        switch (dragState) {
            case DragState.Dragging:
//                TryCheckCell();
                break;
            default:
                break;
        }
    }

    public void OnMouseButton(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                TrySelectFlower();
                break;
            case InputActionPhase.Canceled:
                TryMoveFlower();
                break;
        }
    }

    public void TrySelectFlower() {
        int layerMask = 1 << 6;
        /*
        RaycastHit hit;

        if (Physics.Raycast(crntMousePosition, transform.TransformDirection(Vector3.forward), out hit, raycastDistance, layerMask)) {
            

            if (hit.transform.CompareTag("Flower")) { 

                dragState = DragState.Dragging;

                Flower selectedFlower = hit.transform.GetComponent<Flower>();
                if (selectedFlower != null)
                {
                    GameplayManager.Instance.SetSelectedFlower(selectedFlower);
                }
            }
        }
        */
    }

    public void TryMoveFlower() {
      //  GameplayManager.Instance.ReleaseFlower();
        dragState = DragState.Empty;
    }

    public Vector3 GetPointerWorldPosition(float z)
    {
        Vector3 pointerPos = mainCamera.ScreenToWorldPoint(
            new Vector3(
                crntMousePosition.x,
                crntMousePosition.y,
                z
                ));
        return pointerPos;
    }

    public Vector3 GetPointerWorldPositionOnSurface() {
        int layerMask = 1 << 8;

        RaycastHit hit;

        if (Physics.Raycast(crntMousePosition, transform.TransformDirection(Vector3.forward), out hit, raycastDistance, layerMask)) {
            return hit.point;
        }

        return Vector3.zero;
    }
}
