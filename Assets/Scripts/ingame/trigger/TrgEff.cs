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
                case "���� ȸ��":
                    Debug.Log("���� ȸ��");
                    break;

                default:
                    Debug.Log("ȿ�� ����");
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (effect == "���� ȸ��")
            {
                if (collision.tag == "unActBottle")
                {
                    Destroy(collision.gameObject);
                }
            }
        }

        // switch ������ ontriggerEnter2D�� �� �� �ֵ���
        // Scene ���� bottle�� count�ϴ� ��ũ��Ʈ �ۼ��ϱ�
        // �̹� ������ �ִ� bottle�� ȸ���Ѵٸ� mission count������ ����.

    }
}

