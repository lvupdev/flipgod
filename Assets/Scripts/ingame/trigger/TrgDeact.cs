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
                case "�� ����":
                    Debug.Log("�� ����");
                    if (isDogSnackInTheScene)
                    {

                    }
                    break;
                default:
                    Debug.Log("���� ���� ����");
                    break;
            }

        }
    }
}