using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ingame{
    public class Structure {

        public Structure() {
        }

        public int id;

        public bool isDynamic { get; set; }
        public bool isInGame { get; set; }


        public void dynamicMove(Vector2 vec) {
        }

    }
}