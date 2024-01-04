using System.Collections.Generic;
// namespace Unity.FPS.Game
// {
    public class GameConstants
    {
        // all the constant string used across the game
        public const string k_AxisNameVertical = "Vertical";
        public const string k_AxisNameHorizontal = "Horizontal";
        public const string k_MouseAxisNameVertical = "Mouse Y";
        public const string k_MouseAxisNameHorizontal = "Mouse X";
        public const string k_AxisNameJoystickLookVertical = "Look Y";
        public const string k_AxisNameJoystickLookHorizontal = "Look X";
        
        public const string k_ButtonNameAttack = "Attack";
        public const string k_ButtonNameSkill1 = "Skill 1";
        public const string k_ButtonNameSkill2 = "Skill 2";
        public const string k_ButtonNameSkill3 = "Skill 3";
        public const string k_ButtonNameSkill4 = "Skill 4";
        public const string k_ButtonNameSprint = "Sprint";
        // public const string k_ButtonNameJump = "Jump";
        // public const string k_ButtonNameCrouch = "Crouch";

        public const string k_ButtonNameGamepadAttack = "Gamepad Attack";
        public const string k_ButtonNameGamepadSkill1 = "Gamepad Skill 1";
        public const string k_ButtonNameGamepadSkill2 = "Gamepad Skill 2";
        public const string k_ButtonNameGamepadSkill3 = "Gamepad Skill 3";
        public const string k_ButtonNameGamepadSkill4 = "Gamepad Skill 4";
        public const string k_ButtonNameSwitchWeapon = "Mouse ScrollWheel";
        public const string k_ButtonNameGamepadSwitchWeapon = "Gamepad Switch Weapon";
        public const string k_ButtonNameNextWeapon = "NextWeapon";
        public const string k_ButtonNamePauseMenu = "Pause Menu";
        public const string k_ButtonNameToggleDeviceMenu = "Toggle Device Menu";
        public const string k_ButtonNameSubmit = "Submit";
        public const string k_ButtonNameCancel = "Cancel";
        // public const string k_ButtonReload = "Reload";


        #region Colours
        public static readonly Dictionary<string, string> RarityColourDict = new Dictionary<string, string>(){
          {"Common", "#fff"},
          {"Uncommon", "#2bc26c"},
          {"Rare", "#2b77c2"},
          {"Epic", "#532bc2"},
          {"Legendary", "#c26c2b"},
          {"Mythic", "#c22b2b"},
          {"Limited", "#c22b9f"},
          {"Limited (A)", "#8d2bc2"},
        };
        public static readonly Dictionary<string, string> ElementColourDict = new Dictionary<string, string>(){
          {"Fire", "#c22b2b"},
          {"Wood", "#4ec22b"},
          {"Water", "#2b4cc2"},
        };
        #endregion // Colours
    }
// }