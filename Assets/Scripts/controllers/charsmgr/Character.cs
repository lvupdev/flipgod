using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace charsmgr{
    public class Character {

        public Character() {
        }

        public string charName { get; set; }
        public int birthday { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        
        private string keywordList;
        private string personalityList;
        public string appearance { get; set; }
        public string story { get; set; }
        public SuperPowerController superpowerController { get; set; }


    }
}