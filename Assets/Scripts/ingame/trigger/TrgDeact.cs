using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ingame {
    public class TrgDeact : MonoBehaviour {

        string deactCond;
        bool isDogSnackInTheScene;

        public TrgDeact(string deactCondName) 
        {
            this.deactCond = deactCondName;
        }

        public void Deactivate() 
        {
            switch (deactCond)
            {
                case "개 간식":
                    Debug.Log("개 간식");
                    if (isDogSnackInTheScene)
                    {

                    }
                    break;
                default:
                    Debug.Log("해제 조건 없음");
                    break;
            }

        }
    }
}