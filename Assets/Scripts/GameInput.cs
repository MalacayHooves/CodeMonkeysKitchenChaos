using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Right,
        Move_Left,
        Interact,
        InteractAlt,
        Pause
    }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    private PlayerInputActions inputActions;

    private void Awake()
    {
        Instance = this;

        inputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact:
                return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Move_Up:
                return inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Right:
                return inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Move_Left:
                return inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.InteractAlt:
                return inputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebind)
    {
        inputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case GameInput.Binding.Move_Up:
                inputAction = inputActions.Player.Move;
                bindingIndex = 1;
                break;
            case GameInput.Binding.Move_Down:
                inputAction = inputActions.Player.Move;
                bindingIndex = 2;
                break;
            case GameInput.Binding.Move_Right:
                inputAction = inputActions.Player.Move;
                bindingIndex = 4;
                break;
            case GameInput.Binding.Move_Left:
                inputAction = inputActions.Player.Move;
                bindingIndex = 3;
                break;
            case GameInput.Binding.Interact:
                inputAction = inputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case GameInput.Binding.InteractAlt:
                inputAction = inputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case GameInput.Binding.Pause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                inputActions.Player.Enable();
                onActionRebind();

                inputActions.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, inputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
    }
}
