
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace charsmgr{
    public class Freezer : SuperPower {

        public Freezer() {
        }

        public float range { get; set; }
        public float duration { get; set; }
        public bool isThrown { get; set; }

    }
}