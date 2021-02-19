using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceRecruiterApp.Model.Tasks.SST
{
    public class OutputVar
    {
        public int RT { get; set; }
        public bool isCorrect { get; set; }
        public bool isPressed { get; set; }
        public int SSDH { get; set; }

        public int SSDL { get; set; }

        public bool isEarly { get; set; }

        public bool isLate { get; set; }

        public string PressedKey { get; set; }

        public int Ommission { get; set; }

        public int Commission { get; set; }
    }
}
