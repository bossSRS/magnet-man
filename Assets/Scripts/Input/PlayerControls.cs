//// Author: Sadikur Rahman ////
// Defines the player's input bindings for touch and mouse controls using Unity's Input System.

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerControls : IInputActionCollection {
    private readonly InputActionAsset asset;

    private readonly InputActionMap player;
    private readonly InputAction touchPress;
    private readonly InputAction touchPos;

    public PlayerControls() {
        asset = InputActionAsset.FromJson(@"{
            ""name"": ""PlayerControls"",
            ""maps"": [
                {
                    ""name"": ""Player"",
                    ""actions"": [
                        {""name"": ""TouchPress"", ""type"": ""Button"", ""id"": ""press"", ""expectedControlType"": ""Button""},
                        {""name"": ""TouchPos"", ""type"": ""Value"", ""id"": ""position"", ""expectedControlType"": ""Vector2""}
                    ],
                    ""bindings"": [
                        {""path"": ""<Touchscreen>/press"", ""action"": ""TouchPress""},
                        {""path"": ""<Mouse>/leftButton"", ""action"": ""TouchPress""},
                        {""path"": ""<Touchscreen>/position"", ""action"": ""TouchPos""},
                        {""path"": ""<Mouse>/position"", ""action"": ""TouchPos""}
                    ]
                }
            ]
        }");

        player = asset.FindActionMap("Player", true);
        touchPress = player.FindAction("TouchPress", true);
        touchPos = player.FindAction("TouchPos", true);
    }

    public InputActionAsset Get() => asset;
    public bool Contains(InputAction action)
    {
        return true;
    }

    public void Enable() => asset.Enable();
    public void Disable() => asset.Disable();

    public InputBinding? bindingMask { get; set; }
    public ReadOnlyArray<InputDevice>? devices { get; set; }
    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public PlayerActions Player => new PlayerActions(this);

    public struct PlayerActions {
        private readonly PlayerControls wrapper;
        public PlayerActions(PlayerControls wrapper) => this.wrapper = wrapper;

        public InputAction TouchPress => wrapper.touchPress;
        public InputAction TouchPos => wrapper.touchPos;

        public InputActionMap Get() => wrapper.player;
        public void Enable() => Get().Enable();
        public void Disable() => Get().Disable();
    }

    public IEnumerator<InputAction> GetEnumerator() {
        yield return touchPress;
        yield return touchPos;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
