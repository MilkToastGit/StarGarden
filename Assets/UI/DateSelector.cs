using System;
using UnityEngine;
using TMPro;
using FantomLib;

namespace StarGarden.UI
{
    public class DateSelector : MonoBehaviour
    {
        public DateTime SelectedDate => selectedDate;

        [SerializeField] private TextMeshProUGUI Text;
        private DateTime selectedDate;

        private string defaultDate = "";                 //When it is empty, it is the current time.
        private string resultDateFormat = "yyyy/M/d";    //Java Datetime format.

        private string style = "android:Theme.DeviceDefault.Light.Dialog.Alert"; //Dialog theme

        private void Awake()
        {
            UpdateText(DateTime.Today);
        }

        //Show dialog
        public void Show()
        {
#if UNITY_EDITOR
            Debug.Log("DatePickerController.Show called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowDatePickerDialog(
                defaultDate,
                resultDateFormat,
                gameObject.name,
                "ReceiveResult",
                style);
#endif
        }

        //Set date string dynamically and show dialog (current date string will be overwritten)
        public void Show(string defaultDate)
        {
            this.defaultDate = defaultDate;
            Show();
        }


        //Returns value when 'OK' pressed.
        private void ReceiveResult(string result)
        {
            selectedDate = DateTime.Parse(result);
            defaultDate = result;
            UpdateText(selectedDate);
            print(Zodiac.GetStarsignFromDate(selectedDate));
        }

        private void UpdateText(DateTime d)
        {
            string text = $"{d.Year}/{d.Month.ToString("D2")}/{d.Day.ToString("D2")}";
            Text.text = text;
        }
    }
}