using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ingame{
    public class Trigger {

        public Trigger() {
        }

        public int id;

        public string name { get; set; }

        public string expl { get; set; }

        public TrgAct actCond { get; set; }

        public TrgEff effect { get; set; }

        public TrgDeact deactCond { get; set; }





        public void activate() {
            // TODO implement here
        }

        public void effectOn() {
            // TODO implement here
        }

        public void deactivate() {
            // TODO implement here
        }

    }
}