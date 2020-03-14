
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gamemgr{

    public class Map {

        public Map() {
        }

        public string name { get; set; }

        public int id;

        private int MAX_STG = 100;
        private int stageList = new int[MAX_STG];


    }
}