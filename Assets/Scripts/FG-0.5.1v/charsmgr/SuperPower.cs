using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace charsmgr{
    public abstract class SuperPower {

        public SuperPower() {
        }

        public int tensionLoss { get; set; }

        public int specNum { get; set; }


        public abstract void activate();

        public abstract void specialmove();

    }
}