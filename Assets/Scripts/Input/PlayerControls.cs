// Auto-generated version placeholder

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerControls : IInputActionCollection {
    private InputActionAsset asset;
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
                        {
                            ""path"": ""<Touchscreen>/press"",
                            ""action"": ""TouchPress"",
                            ""interactions"": """"
                        },
                        {
                            ""path"": ""<Mouse>/leftButton"",
                            ""action"": ""TouchPress"",
                            ""interactions"": """"
                        },
                        {
                            ""path"": ""<Touchscreen>/position"",
                            ""action"": ""TouchPos"",
                            ""interactions"": """"
                        },
                        {
                            ""path"": ""<Mouse>/position"",
                            ""action"": ""TouchPos"",
                            ""interactions"": """"
                        }
                    ]
                }
            ]
        }");

        // Set references
        player = asset.FindActionMap("Player", true);
        touchPress = player.FindAction("TouchPress", true);
        touchPos = player.FindAction("TouchPos", true);
    }

    public InputActionAsset Get() => asset;
    public bool Contains(InputAction action)
    {
        throw new System.NotImplementedException();
    }

    public void Enable() => asset.Enable();
    public void Disable() => asset.Disable();
    public InputBinding? bindingMask { get; set; }
    public ReadOnlyArray<InputDevice>? devices { get; set; }
    public ReadOnlyArray<InputControlScheme> controlSchemes { get; }

    private readonly InputActionMap player;
    private readonly InputAction touchPress;
    private readonly InputAction touchPos;

    public struct PlayerActions {
        private readonly PlayerControls wrapper;
        public PlayerActions(PlayerControls wrapper) { this.wrapper = wrapper; }
        public InputAction TouchPress => wrapper.touchPress;
        public InputAction TouchPos => wrapper.touchPos;
        public InputActionMap Get() => wrapper.player;
        public void Enable() => Get().Enable();
        public void Disable() => Get().Disable();
    }

    public PlayerActions Player => new PlayerActions(this);
    public IEnumerator<InputAction> GetEnumerator()
    {
        yield return null;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        yield return null;
    }
}
