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
                    ""id"": ""997f60b3-df8a-4b90-8cfa-d501ef37baa4"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button3"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bb4fe24-880a-4a4c-a18a-a47bbacfce1c"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29a7c631-783c-472f-bfbe-2e836cb77930"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""powerupaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c28246ec-3ddf-40a0-a412-7adfa9187c23"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button3"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""driftbar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb1c3886-7076-4fdf-986d-c7c97c3033b1"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""driftbar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""334ff061-416d-4509-ae08-c24c1d5735e0"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""driftbar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ed723dc-08e4-4d78-90ca-2f79b977f0a4"",
                    ""path"": ""<Joystick>/stick"",
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
