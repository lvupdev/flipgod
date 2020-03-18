using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace charsmgr{
    public class Membrane : SuperPower {

        public Membrane() {
        }

        public float cooltime { get; set; }

        public float duration { get; set; }

        public bool isSpecial { get; set; }

        override
        public void activate()
        {
            // TODO implement here
        }

        override
        public void specialmove()
        {
            // TODO implement here
        }

    }
}