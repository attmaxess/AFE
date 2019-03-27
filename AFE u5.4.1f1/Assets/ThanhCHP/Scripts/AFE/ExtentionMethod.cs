﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public static class ExtentionMethod
    {
        public static List<IReceiveDamageable> ListReceiverDamageables(this GameObject go,
            IChampionConfig championConfig)
        {
            var _objects = Physics.OverlapSphere(go.transform.position, championConfig.Range.Value);
            var _l = _objects.OfType<IReceiveDamageable>().ToList();
            return _l;
        }

        public static IReceiveDamageable ReceiverDamageNearest(this GameObject go, IChampionConfig championConfig)
        {
            IReceiveDamageable receiveDamageable = null;
            var _objects = Physics.OverlapSphere(go.transform.position, championConfig.Range.Value);
            var _l = _objects.Where(_ =>
                _.GetComponent<IReceiveDamageable>() != null &&
                _.transform.GetInstanceID() != go.transform.GetInstanceID()).ToList();
            float tempDis = -1;
            foreach (var item in _l)
            {
                if (tempDis < Vector3.Distance(item.transform.position, go.transform.position))
                {
                    tempDis = Vector3.Distance(item.transform.position, go.transform.position);
                    receiveDamageable = item.GetComponent<IReceiveDamageable>();
                }
            }

            return receiveDamageable;
        }

        public static IReceiveDamageable ReceiverDamageNearest(this GameObject go, IChampionConfig championConfig,
            out Transform nearestGameObject)
        {
            IReceiveDamageable receiveDamageable = null;
            Collider collider = null;
            var _objects = Physics.OverlapSphere(go.transform.position, championConfig.Range.Value);
            var _l = _objects.Where(_ =>
                _.GetComponent<IReceiveDamageable>() != null &&
                _.transform.GetInstanceID() != go.transform.GetInstanceID()).ToList();
            float tempDis = -1;
            foreach (var item in _l)
            {
                if (tempDis < Vector3.Distance(item.transform.position, go.transform.position))
                {
                    tempDis = Vector3.Distance(item.transform.position, go.transform.position);
                    receiveDamageable = item.GetComponent<IReceiveDamageable>();
                    collider = item;
                }
            }

            nearestGameObject = collider.transform;
            return receiveDamageable;
        }

        public static IReceiveDamageable ReceiverDamageNearestByRayCast(this GameObject go,
            Vector3 direction, float distance, LayerMask layerMask = default(LayerMask),
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            IReceiveDamageable receiveDamageable = null;
            RaycastHit hit;
            Debug.DrawRay(go.transform.position, direction * distance, Color.red, 1);
            if (layerMask == default(LayerMask)) layerMask = ~0;

            if (Physics.Raycast(go.transform.position, direction, out hit, distance, layerMask,
                queryTriggerInteraction))
            {
                receiveDamageable = hit.transform.GetComponent<IReceiveDamageable>();
            }

            return receiveDamageable;
        }

        public static IReceiveDamageable ReceiverDamageNearestByRayCastAll(this GameObject go,
            Vector3 direction, float distance, LayerMask layerMask = default(LayerMask),
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
        {
            if (layerMask == default(LayerMask)) layerMask = ~0;
            IReceiveDamageable receiveDamageable = null;
            Debug.DrawRay(go.transform.position, direction * distance, Color.blue, 0.5f);
            var _all = Physics.RaycastAll(go.transform.position, direction, distance, layerMask,
                queryTriggerInteraction);
            float _dis = -1;
            for (int i = 0; i < _all.Length; i++)
            {
                var _receiver = _all[i].transform.GetComponent<IReceiveDamageable>();
                if (_receiver != null &&
                    Vector3.Distance(_receiver.GetTransform.position, go.transform.position) > _dis)
                {
                    receiveDamageable = _receiver;
                    _dis = Vector3.Distance(_receiver.GetTransform.position, go.transform.position);
                }
            }

            return receiveDamageable;
        }

        public static List<IReceiveDamageable> GetAllReceiverDamageNearestByRayCastAll(this GameObject go,
           Vector3 direction, float distance, LayerMask layerMask = default(LayerMask),
           QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
        {
            if (layerMask == default(LayerMask))
            {
                layerMask = ~0;
            }
            List<IReceiveDamageable> receiveDamageable = new List<IReceiveDamageable>();
            Debug.DrawRay(go.transform.position, direction * distance, Color.blue, 0.5f);
            var _all = Physics.RaycastAll(go.transform.position, direction.normalized, distance, layerMask,
                queryTriggerInteraction);
            var _allIreveiceDamaable = _all.Where(_ => _.transform.GetComponent<IReceiveDamageable>() != null).ToList();
            foreach (var item in _allIreveiceDamaable)
            {
                receiveDamageable.Add(item.transform.GetComponent<IReceiveDamageable>());
            }
            return receiveDamageable;
        }
    }
}