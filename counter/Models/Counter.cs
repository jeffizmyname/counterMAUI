using Google.Cloud.Firestore;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace counter.Models
{
    public class Counter : INotifyPropertyChanged
    {
        public string id { get; set; }

        private int value;

        public event PropertyChangedEventHandler PropertyChanged;

        public string name { get; set; }
        public int defaultVal { get; set; } = 0;

        public int Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }
        public Color coutnerColor { get; set; }
        public Color secondaryColor { get; set; }

        public Counter(string id, string name, Color color, int defaultVal)
        {
            this.id = id;
            this.name = name;
            this.coutnerColor = color;
            this.secondaryColor = CreateSecondaryColor(color, 0.85f);
            this.defaultVal = defaultVal;
            this.Value = defaultVal;
        }

        public Counter(Dictionary<string, object> counterDic)
        {
            name = (string)counterDic["Name"];
            coutnerColor = Color.FromArgb((string)counterDic["Color"]);
            defaultVal = Convert.ToInt32(counterDic["BaseValue"]);
            Value = Convert.ToInt32(counterDic["Value"]);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Color CreateSecondaryColor(Color color, float factor)
        {
            factor = Math.Clamp(factor, 0f, 2f);

            float red = color.Red * factor;
            float green = color.Green * factor;
            float blue = color.Blue * factor;

            red = Math.Clamp(red, 0f, 1f);
            green = Math.Clamp(green, 0f, 1f);
            blue = Math.Clamp(blue, 0f, 1f);

            return new Color(red, green, blue, color.Alpha);
        }

    }
}
