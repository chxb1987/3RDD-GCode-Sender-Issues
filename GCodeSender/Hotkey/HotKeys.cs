﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using System.Windows.Input;

namespace GCodeSender.Hotkey
{
    #region HotKey Load and Update
    public class HotKeys
    {
        // Ignored single keys, need to be paired with another
        public static HashSet<Key> _ignoredKey = new HashSet<Key>() { Key.LeftAlt, Key.RightAlt, Key.LeftCtrl,
            Key.RightCtrl, Key.LeftShift, Key.RightShift, Key.RWin, Key.LWin, Key.Apps, Key.Escape};

        public static Dictionary<string, string> hotkeyCode = new Dictionary<string, string>(); // Holds Key Fucntions and Keycodes
        public static Dictionary<string, string> hotkeyDescription = new Dictionary<string, string>(); // Holds Key Functions and Description

        /// <summary>
        /// Loads Hotkey XML file and loads keycodes into hotkeyCode and Description into hotkeyDescription
        /// </summary>
        public static void LoadHotKeys()
        {
            string HotKeyFile = "Resources\\hotkeys.xml";
            hotkeyCode.Clear();
            hotkeyDescription.Clear();

            string fileName = HotKeyFile;
            if (!File.Exists(fileName))
            {
                MessageBox.Show("Hotkey file not found, no hotkeys set!", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }

            XmlReader r = XmlReader.Create(HotKeyFile);   // "hotkeys.xml");

            while (r.Read())
            {
                if (!r.IsStartElement())
                    continue;

                switch (r.Name)
                {
                    case "hotkeys":
                        break;
                    case "bind":
                        if ((r["keyfunction"].Length > 0) && (r["keycode"] != null))
                        {
                            if (!hotkeyCode.ContainsKey(r["keyfunction"]))
                                hotkeyCode.Add(r["keyfunction"], r["keycode"]);
                            hotkeyDescription.Add(r["keyfunction"], r["key_description"]);
                        }
                        break;
                }
            }
        }

        // Testing Updating selected Node
        /// <summary>
        /// Updates the selected Hotkey to the desired key.
        /// Recieved from newHotKeySave->SettingsWindow
        /// </summary>
        /// <param name="keyfunction">The hotkey name to be updated</param>
        /// <param name="newSetting">New value for the hotkey</param>
        public static void UpdateHotkey(string keyfunction, string newSetting)
        {
            var root = new XmlDocument();
            root.Load(@"Resources\\hotkeys.xml");

            foreach (XmlNode e in root.GetElementsByTagName("bind"))
            {
                if (e.Attributes["keyfunction"].Value.Equals(keyfunction))
                {
                    e.Attributes["keycode"].Value = newSetting; // FirstChild because the inner node is actually the inner text, yeah XmlNode is weird.
                    break;
                }
            }
            root.Save(@"Resources\\hotkeys.xml");
        }

        }
        #endregion

        #region HotKey Internal Class
        internal class HotKey
        {
            public bool Ctrl { get; set; }

            public bool Shift { get; set; }

            public bool Alt { get; set; }

            public Key Key { get; set; }

            public override string ToString()
            {
                return string.Format("{0}{1}{2}{3}",
                    Ctrl ? "Ctrl+" : string.Empty,
                    Shift ? "Shift+" : string.Empty,
                    Alt ? "Alt+" :
                    String.Empty, KeyToString(Key));
            }

            public string KeyToString(Key key)
            {
                var ret = key.ToString();

                switch (key)
                {
                    //case Key.NumPad0:
                    //    ret = "0";
                    //    break;
                    //case Key.NumPad1:
                    //    ret = "1";
                    //    break;
                    //case Key.NumPad2:
                    //    ret = "2";
                    //    break;
                    //case Key.NumPad3:
                    //    ret = "3";
                    //    break;
                    //case Key.NumPad4:
                    //    ret = "4";
                    //    break;
                    //case Key.NumPad5:
                    //    ret = "5";
                    //    break;
                    //case Key.NumPad6:
                    //    ret = "6";
                    //    break;
                    //case Key.NumPad7:
                    //    ret = "7";
                    //    break;
                    //case Key.NumPad8:
                    //    ret = "8";
                    //    break;
                    //case Key.NumPad9:
                    //    ret = "9";
                    //    break;
                    //case Key.Down:
                    //    ret = "Down Arrow";
                    //    break;
                    //case Key.Up:
                    //    ret = "Up Arrow";
                    //    break;
                    //case Key.Right:
                    //    ret = "Right Arrow";
                    //    break;
                    //case Key.Left:
                    //    ret = "Left Arrow";
                    //    break;
                    case Key.Subtract:
                        ret = "-";
                        break;
                    case Key.Add:
                        ret = "+";
                        break;
                    case Key.Multiply:
                        ret = "*";
                        break;
                    case Key.Divide:
                        ret = "/";
                        break;
                    case Key.Decimal:
                        ret = ",";
                        break;
                    case Key.OemComma:
                        ret = ",";
                        break;
                    case Key.OemPeriod:
                        ret = ".";
                        break;
                    case Key.OemPipe:
                        ret = "|";
                        break;
                    case Key.Oem3:
                        ret = "`";
                        break;
                }

                return ret;
            }
        }
        #endregion
    }
