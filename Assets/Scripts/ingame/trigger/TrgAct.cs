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
                case "��� �ߵ�":
                    Debug.Log("��� �ߵ�");
                    break;

                default:
                    Debug.Log("�ߵ� �� ��");
                    break;
            }
            
        }


    }
}