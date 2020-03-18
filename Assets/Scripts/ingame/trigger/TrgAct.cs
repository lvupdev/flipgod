using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ingame {
    public class TrgAct : MonoBehaviour {

        string actCond;

        public TrgAct(string actCondName) 
        {
            this.actCond = actCondName;
        }

        public void Activate() 
        {
            switch (actCond)
            {
                case "상시 발동":
                    Debug.Log("상시 발동");
                    break;

                default:
                    Debug.Log("발동 안 함");
                    break;
            }
            
        }


    }
}