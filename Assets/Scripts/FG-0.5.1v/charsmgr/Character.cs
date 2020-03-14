
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
        private int MAX_KEY = 10;
        private string keywordList = new int[MAX_KEY];
        private int MAX_PER = 10;
        private string personalityList = new int [MAX_PER];
        public string appearance { get; set; }
        public string story { get; set; }
        public SuperPower superpower { get; set; }


    }
}