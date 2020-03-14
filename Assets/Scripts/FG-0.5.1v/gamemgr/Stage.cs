
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gamemgr{
    public class Stage {

        public Stage() {
        }

        public int id;

        public int inMap { get; set; }
        public int stageNum;
        public bool isFinished;
        public char grade { get; set; }
        public int finDate;
        public string diary { get; set; }
        public void isLocked { get; set; }
        public string unLockCond { get; set; }

        private int MAX_TRG = 10;
        private int triggerList = new int [MAX_TRG];
        private int MAX_STR = 10;
        private int strctList = new int[MAX_STR];





    }
}