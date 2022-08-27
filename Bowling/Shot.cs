using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class Shot
    {
        // PROPERTIES
        public int Pins { get; set; }
        public Frame Frame { get; set; }

        // CONSTRUCTORS
        public Shot(int pins, Frame frame)
        {
            this.Pins = pins;
            this.Frame = frame;
        }
    }
}
