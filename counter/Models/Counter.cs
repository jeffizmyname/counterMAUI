using Google.Cloud.Firestore;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace counter.Models
{
    public class Counter : INotifyPropertyChanged
    {
    
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _value;
        public string id { get; set; }
        public string name { get; set; }
        public int defaultVal { get; set; } = 0;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        public Color coutnerColor { get; set; }
        public Color secondaryColor { get; set; }


        private float colorFactorL = 0.85f;
        private float colorFactorD = 2.3f;

        public Counter(string id, string name, Color color, int defaultVal)
        {
            this.id = id;
            this.name = name;
            this.coutnerColor = color;
            this.secondaryColor = CreateSecondaryColor(color, colorFactorL, colorFactorD);
            this.defaultVal = defaultVal;
            this.Value = defaultVal;
        }

        public Counter(Dictionary<string, object> counterDic)
        {
            name = (string)counterDic["Name"];
            coutnerColor = Color.FromArgb((string)counterDic["Color"]);
            secondaryColor = CreateSecondaryColor(coutnerColor, colorFactorL, colorFactorD);
            defaultVal = Convert.ToInt32(counterDic["BaseValue"]);
            Value = Convert.ToInt32(counterDic["Value"]);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Color CreateSecondaryColor(Color color, float lightFactorMultiplier, float darkFactorMultiplier)
        {
            double luminance = (0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue);

            float factor;

            if (luminance > 0.3)
            {
                factor = Math.Clamp(1 - ((float)luminance * lightFactorMultiplier), 0f, 1f);
                Trace.WriteLine("Light color " + luminance);
            }
            else if (luminance == 0)
            {
                Trace.WriteLine("Color is black: " + luminance);
                return new Color(255, 255, 255, color.Alpha); 
            }
            else if (luminance < 0.02 && luminance > 0.01)
            {
                factor = Math.Clamp(1 + ((1 - (float)luminance) * darkFactorMultiplier * 60), 0f, 60f);
                Trace.WriteLine("Medium dark " + luminance);
            }
            else if (luminance < 0.01 && luminance >= 0.002)
            {
                factor = Math.Clamp(1 + ((1 - (float)luminance) * darkFactorMultiplier * 10), 0f, 10f);
                Trace.WriteLine("Dark color " + luminance);
            }
            else if (luminance < 0.002)
            {
                factor = Math.Clamp(1 + ((1 - (float)luminance) * darkFactorMultiplier * 50), 0f, 50f);
                Trace.WriteLine("Very dark color " + luminance);
            }
            else
            {
                factor = Math.Clamp(1 + ((1 - (float)luminance) * darkFactorMultiplier * 5), 0f, 5f);
                Trace.WriteLine("Dark color " + luminance);
            }

            float red = Math.Clamp(color.Red * factor, 0f, 1f);
            float green = Math.Clamp(color.Green * factor, 0f, 1f);
            float blue = Math.Clamp(color.Blue * factor, 0f, 1f);

            return new Color(red, green, blue, color.Alpha);

        }


    }
}
