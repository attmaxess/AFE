﻿using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class WindWall : ObjectElementSkillBehaviour
    {
        
//        internal override void OnSpawn(Vector3 startPos, Vector3 direction)
//        {
//            movable.MoveToDir(startPos, direction);
//        }
        internal override void OnSpawn(Vector3 startPos, Vector3 direction, IDamageMessage damageMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}