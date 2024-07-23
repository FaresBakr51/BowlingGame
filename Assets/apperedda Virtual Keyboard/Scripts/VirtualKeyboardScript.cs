using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

namespace VirtualKeyboardapperedda
{
    public class VirtualKeyboardScript : MonoBehaviour
    {
        //drag the InputField you want to update here in the Inspector
        public TMP_InputField textBoxToUpdate;

        //we will toggle upperKeys on and off with shift
        public GameObject upperKeys, lowerKeys;

        //this is a temporary string for the backspace, it will update the current string in the textbox and then be replaced by it
        private string tempString;

        public GameObject firstButtonToSelect;

        //do not change any code below

        public void SetTextInput(TMP_InputField currentInputField)
        {
            textBoxToUpdate = currentInputField;
            
        }
        public void WaitSelection(GameObject buttonToSelect)
        {
            StartCoroutine(WaitSelectionTime(buttonToSelect));
        }
        IEnumerator WaitSelectionTime(GameObject buttonToSelect)
        {
            yield return new WaitForSeconds(0.5f);
            EventSystem.current.SetSelectedGameObject(buttonToSelect);
        }
        public void GetButton(string buttonName)
        {

            buttonName = EventSystem.current.currentSelectedGameObject.name;
          //  textBoxToUpdate.Select();
           // textBoxToUpdate.ActivateInputField();

            //this will be where we pass the levels
            switch (buttonName)
            {
                case "backspaceBtn":
                    tempString = textBoxToUpdate.text;
                    if (tempString.Length > 0)
                    {
                        tempString = tempString.Substring(0, tempString.Length - 1);
                        textBoxToUpdate.text = tempString;
                    }
                    break;

                case "spaceBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + " ";
                    break;

                case "shiftBtn":
                    //this will toggle the visibility of the upperKeys and lowerKeys gameobject based on its current state
                    upperKeys.SetActive(!upperKeys.activeSelf);
                    lowerKeys.SetActive(!lowerKeys.activeSelf);
                    break;

                case "delBtn":
                    //this will clear the input field
                    textBoxToUpdate.text = "";
                    break;

                case "backspaceBtnShifted":
                    tempString = textBoxToUpdate.text;
                    if (tempString.Length > 0)
                    {
                        tempString = tempString.Substring(0, tempString.Length - 1);
                        textBoxToUpdate.text = tempString;
                    }
                    break;

                case "spaceBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + " ";
                    break;

                case "shiftBtnShifted":
                    //this will toggle the visibility of the upperKeys and lowerKeys gameobject based on its current state
                    upperKeys.SetActive(!upperKeys.activeSelf);
                    lowerKeys.SetActive(!lowerKeys.activeSelf);
                    break;

                case "delBtnShifted":
                    //this will clear the input field
                    textBoxToUpdate.text = "";
                    break;

                //keyboard lowercase letter buttons below

                case "1Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "1";
                    break;

                case "2Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "2";
                    break;

                case "3Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "3";
                    break;

                case "4Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "4";
                    break;

                case "5Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "5";
                    break;

                case "6Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "6";
                    break;

                case "7Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "7";
                    break;

                case "8Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "8";
                    break;

                case "9Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "9";
                    break;

                case "0Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "0";
                    break;

                case "qBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "q";
                    break;

                case "wBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "w";
                    break;

                case "eBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "e";
                    break;

                case "rBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "r";
                    break;

                case "tBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "t";
                    break;

                case "yBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "y";
                    break;

                case "uBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "u";
                    break;

                case "iBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "i";
                    break;

                case "oBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "o";
                    break;

                case "pBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "p";
                    break;

                case "aBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "a";
                    break;

                case "sBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "s";
                    break;

                case "dBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "d";
                    break;

                case "fBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "f";
                    break;

                case "gBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "g";
                    break;

                case "hBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "h";
                    break;

                case "jBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "j";
                    break;

                case "kBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "k";
                    break;

                case "lBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "l";
                    break;

                case "zBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "z";
                    break;

                case "xBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "x";
                    break;

                case "cBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "c";
                    break;

                case "vBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "v";
                    break;

                case "bBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "b";
                    break;

                case "nBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "n";
                    break;

                case "mBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "m";
                    break;

                //keyboard uppercase letter buttons below

                case "shift1Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "!";
                    break;

                case "shift2Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "@";
                    break;

                case "shift3Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "#";
                    break;

                case "shift4Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "$";
                    break;

                case "shift5Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "%";
                    break;

                case "shift6Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "^";
                    break;

                case "shift7Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "&";
                    break;

                case "shift8Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "*";
                    break;

                case "shift9Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "(";
                    break;

                case "shift0Btn":
                    textBoxToUpdate.text = textBoxToUpdate.text + ")";
                    break;

                case "QBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "Q";
                    break;

                case "WBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "W";
                    break;

                case "EBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "E";
                    break;

                case "RBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "R";
                    break;

                case "TBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "T";
                    break;

                case "YBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "Y";
                    break;

                case "UBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "U";
                    break;

                case "IBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "I";
                    break;

                case "OBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "O";
                    break;

                case "PBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "P";
                    break;

                case "ABtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "A";
                    break;

                case "SBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "S";
                    break;

                case "DBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "D";
                    break;

                case "FBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "F";
                    break;

                case "GBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "G";
                    break;

                case "HBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "H";
                    break;

                case "JBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "J";
                    break;

                case "KBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "K";
                    break;

                case "LBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "L";
                    break;

                case "ZBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "Z";
                    break;

                case "XBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "X";
                    break;

                case "CBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "C";
                    break;

                case "VBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "V";
                    break;

                case "BBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "B";
                    break;

                case "NBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "N";
                    break;

                case "MBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "M";
                    break;


                //special character buttons
                case "dashBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "-";
                    break;

                case "dashBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "_";
                    break;

                case "eqlBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "=";
                    break;

                case "eqlBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "+";
                    break;

                case "lBrcktBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "[";
                    break;

                case "lBrcktBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "{";
                    break;

                case "rBrcktBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "]";
                    break;

                case "rBrcktBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "}";
                    break;

                case "lSlashBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "\\";
                    break;

                case "lSlashBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "|";
                    break;

                case "semiBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + ";";
                    break;

                case "semiBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + ":";
                    break;

                case "sQuoteBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "'";
                    break;

                case "sQuoteBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "\"";
                    break;

                case "commaBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + ",";
                    break;

                case "commaBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "<";
                    break;

                case "periodBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + ".";
                    break;

                case "periodBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + ">";
                    break;

                case "rSlashBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "/";
                    break;

                case "rSlashBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "?";
                    break;

                case "noTildeBtn":
                    textBoxToUpdate.text = textBoxToUpdate.text + "`";
                    break;

                case "noTildeBtnShifted":
                    textBoxToUpdate.text = textBoxToUpdate.text + "~";
                    break;

                case "Confirm":

                    //  textBoxToUpdate.text = textBoxToUpdate.text + "~";
                    textBoxToUpdate.Select();
                    break;

                default:
                    break;
            }
        }
    }
}