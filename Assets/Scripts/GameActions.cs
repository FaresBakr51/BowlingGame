// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/GameActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameActions"",
    ""maps"": [
        {
            ""name"": ""ButtonActions"",
            ""id"": ""a7f26d43-ca3e-4aca-9248-99362d0010c6"",
            ""actions"": [
                {
                    ""name"": ""powerupaction"",
                    ""type"": ""Button"",
                    ""id"": ""421b8609-13e1-4749-8cf0-f0aa2ab731d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""driftbar"",
                    ""type"": ""Button"",
                    ""id"": ""2037763c-0abf-46fd-ba65-8225675ac4b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""moving"",
                    ""type"": ""Value"",
                    ""id"": ""24eae9dd-3d6f-461d-952d-a394132dc575"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""29a7c631-783c-472f-bfbe-2e836cb77930"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9f50701-9dad-4ee6-86f7-ac18290617de"",
                    ""path"": ""<SwitchProControllerHID>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f797cb0-7c06-49d5-a3bb-da5bc7f336a7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9ddad06-bde9-499f-b46a-6d44468a3440"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea6b3772-66ce-47fb-bad2-d37618b8ba4f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""334ff061-416d-4509-ae08-c24c1d5735e0"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""driftbar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b37dbd39-787e-4df3-9191-25138f7798cf"",
                    ""path"": ""<SwitchProControllerHID>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""driftbar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8109c364-96f0-4071-8054-06c82fd3f8c3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""driftbar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f027d046-6a79-4151-b1bb-6082b3ab3ddf"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""driftbar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ed723dc-08e4-4d78-90ca-2f79b977f0a4"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""moving"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ButtonActions
        m_ButtonActions = asset.FindActionMap("ButtonActions", throwIfNotFound: true);
        m_ButtonActions_powerupaction = m_ButtonActions.FindAction("powerupaction", throwIfNotFound: true);
        m_ButtonActions_driftbar = m_ButtonActions.FindAction("driftbar", throwIfNotFound: true);
        m_ButtonActions_moving = m_ButtonActions.FindAction("moving", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // ButtonActions
    private readonly InputActionMap m_ButtonActions;
    private IButtonActionsActions m_ButtonActionsActionsCallbackInterface;
    private readonly InputAction m_ButtonActions_powerupaction;
    private readonly InputAction m_ButtonActions_driftbar;
    private readonly InputAction m_ButtonActions_moving;
    public struct ButtonActionsActions
    {
        private @GameActions m_Wrapper;
        public ButtonActionsActions(@GameActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @powerupaction => m_Wrapper.m_ButtonActions_powerupaction;
        public InputAction @driftbar => m_Wrapper.m_ButtonActions_driftbar;
        public InputAction @moving => m_Wrapper.m_ButtonActions_moving;
        public InputActionMap Get() { return m_Wrapper.m_ButtonActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ButtonActionsActions set) { return set.Get(); }
        public void SetCallbacks(IButtonActionsActions instance)
        {
            if (m_Wrapper.m_ButtonActionsActionsCallbackInterface != null)
            {
                @powerupaction.started -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnPowerupaction;
                @powerupaction.performed -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnPowerupaction;
                @powerupaction.canceled -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnPowerupaction;
                @driftbar.started -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnDriftbar;
                @driftbar.performed -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnDriftbar;
                @driftbar.canceled -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnDriftbar;
                @moving.started -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnMoving;
                @moving.performed -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnMoving;
                @moving.canceled -= m_Wrapper.m_ButtonActionsActionsCallbackInterface.OnMoving;
            }
            m_Wrapper.m_ButtonActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @powerupaction.started += instance.OnPowerupaction;
                @powerupaction.performed += instance.OnPowerupaction;
                @powerupaction.canceled += instance.OnPowerupaction;
                @driftbar.started += instance.OnDriftbar;
                @driftbar.performed += instance.OnDriftbar;
                @driftbar.canceled += instance.OnDriftbar;
                @moving.started += instance.OnMoving;
                @moving.performed += instance.OnMoving;
                @moving.canceled += instance.OnMoving;
            }
        }
    }
    public ButtonActionsActions @ButtonActions => new ButtonActionsActions(this);
    public interface IButtonActionsActions
    {
        void OnPowerupaction(InputAction.CallbackContext context);
        void OnDriftbar(InputAction.CallbackContext context);
        void OnMoving(InputAction.CallbackContext context);
    }
}
