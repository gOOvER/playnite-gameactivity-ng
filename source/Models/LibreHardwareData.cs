using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActivity.Models
{
    public class Child
    {
        public int id { get; set; }
        public string Text { get; set; }
        public string Min { get; set; }
        public string Value { get; set; }
        public string Max { get; set; }
        public string ImageURL { get; set; }
        public List<Child> Children { get; set; }
        public string SensorId { get; set; }
        public string HardwareId { get; set; }
        public string Type { get; set; }
        public float? RawValue { get; set; }
        public float? RawMin { get; set; }
        public float? RawMax { get; set; }
    }

    public class LibreHardwareData
    {
        public int id { get; set; }
        public string Text { get; set; }
        public string Min { get; set; }
        public string Value { get; set; }
        public string Max { get; set; }
        public string ImageURL { get; set; }
        public List<Child> Children { get; set; }
    }
}
