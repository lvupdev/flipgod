using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ingame {
    public class TrgEff : Collider {

        string effect;
        
        public TrgEff(string effectName) 
        {
            this.effect = effectName;
        }

        public void EffectOn()
        {
            switch(effect)
            {
                case "물병 회수":
                    Debug.Log("물병 회수");
                    break;

                default:
                    Debug.Log("효과 없음");
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (effect == "물병 회수")
            {
                if (collision.tag == "unActBottle")
                {
                    Destroy(collision.gameObject);
                }
            }
        }

        // switch 내에서 ontriggerEnter2D를 할 수 있도록
        // Scene 내의 bottle을 count하는 스크립트 작성하기
        // 이미 세워져 있는 bottle을 회수한다면 mission count에서도 깎임.

    }
}

