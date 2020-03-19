using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ingame {
    // Component/MonoBehaviour 상속?
    // AddComponent 하려면 상속받아야 함.
    public class Trigger : MonoBehaviour {

        public Trigger() {
            
        }

        public int id;

        // 해결해야 함.
        public string name { get; set; }

        public string expl { get; set; }

        public TrgAct actCond { get; set; }

        public TrgEff effect { get; set; }

        public TrgDeact deactCond { get; set; }


        public void Activate() {
            // TODO implement here
            actCond.Activate();
        }

        public void EffectOn() {
            // TODO implement here
            effect.EffectOn();
        }

        public void Deactivate() {
            // TODO implement here
            deactCond.Deactivate();
        }

    }
}