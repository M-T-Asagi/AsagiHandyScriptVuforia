using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsagiVuforiaScripts
{
    public class MarkeredObjectArrangeToStage : MonoBehaviour
    {
        [System.Serializable]
        public class VectorsUseAs
        {
            public ValueFrom x;
            public ValueFrom y;
            public ValueFrom z;

            public VectorsUseAs()
            {
                x = 0;
                y = 0;
                z = 0;
            }

            public VectorsUseAs(ValueFrom _x, ValueFrom _y, ValueFrom _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }
        }

        [System.Serializable]
        public enum ValueFrom
        {
            X = 0,
            Y,
            Z,
            Zero,
            MinusX,
            MinusY,
            MinusZ
        }

        [System.Serializable]
        public enum DirectionTypes
        {
            MarkerForward = 0,
            MarkerRight,
            MarkerLeft,
            MarkerBackward,
            MarkerUp,
            MarkerBottom,
            WorldForward,
            WorldRight,
            WorldLeft,
            WorldBackward,
            WorldUp,
            WorldBottom
        }

        [SerializeField, Tooltip("マーカーの読み込み管理")]
        TrackStateManager trackStateManager = null;
        [SerializeField, Tooltip("マーカー読み込み後オブジェクトを配置する先")]
        Transform stage = null;
        [SerializeField, Tooltip("マーカー読み込み時に配置されるオブジェクト")]
        GameObject item = null;
        [SerializeField, Tooltip("オブジェクト配置時に参照される前向きのベクトル")]
        DirectionTypes itemLookTo = 0;
        [SerializeField, Tooltip("オブジェクト配置時に前向きのベクトルがどの値を参照するか")]
        VectorsUseAs lookToVectors = null;
        [SerializeField, Tooltip("オブジェクト配置時に参照される上向きのベクトル")]
        DirectionTypes itemUpTo = 0;
        [SerializeField, Tooltip("オブジェクト配置時に上向きのベクトルがどの値を参照するか")]
        VectorsUseAs upToVectors = null;
        [SerializeField, Tooltip("Trueだとマーカーを読んでいる時しかオブジェクトがアクティブにならない")]
        bool activeOnlyMarkerdetected = false;

        GameObject instatiatedItem = null;
        Transform instatiatedItemTransform = null;

        Transform markerTransform = null;
        
        // Start is called before the first frame update
        void Start()
        {
            instatiatedItem = Instantiate(item, stage);
            instatiatedItemTransform = instatiatedItem.transform;
            instatiatedItem.SetActive(false);

            markerTransform = trackStateManager.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if(trackStateManager.Tracked)
            {
                if (!instatiatedItem.activeSelf)
                    instatiatedItem.SetActive(true);

                instatiatedItemTransform.position = markerTransform.position;
                instatiatedItemTransform.rotation = Quaternion.LookRotation(
                    ConvertDirectionVector(GetDirectionFromType(markerTransform, itemLookTo), lookToVectors),
                    ConvertDirectionVector(GetDirectionFromType(markerTransform, itemUpTo), upToVectors));
            }
        }

        Vector3 ConvertDirectionVector(Vector3 direction, VectorsUseAs vectorsUseAs)
        {
            return new Vector3(
                GetVectorValueWithValueFrom(direction, vectorsUseAs.x),
                GetVectorValueWithValueFrom(direction, vectorsUseAs.y),
                GetVectorValueWithValueFrom(direction, vectorsUseAs.z));
        }

        float GetVectorValueWithValueFrom(Vector3 vector, ValueFrom from)
        {
            switch(from)
            {
                case ValueFrom.X:
                    return vector.x;
                case ValueFrom.Y:
                    return vector.y;
                case ValueFrom.Z:
                    return vector.z;
                case ValueFrom.MinusX:
                    return -vector.x;
                case ValueFrom.MinusY:
                    return -vector.y;
                case ValueFrom.MinusZ:
                    return -vector.z;
                default:
                    return 0;
            }
        }

        Vector3 GetDirectionFromType(Transform _transform, DirectionTypes type)
        {
            switch(type)
            {
                case DirectionTypes.MarkerForward:
                    return _transform.forward;
                case DirectionTypes.MarkerRight:
                    return _transform.right;
                case DirectionTypes.MarkerLeft:
                    return -_transform.right;
                case DirectionTypes.MarkerBackward:
                    return -_transform.forward;
                case DirectionTypes.MarkerUp:
                    return _transform.up;
                case DirectionTypes.MarkerBottom:
                    return -_transform.up;
                case DirectionTypes.WorldForward:
                    return Vector3.forward;
                case DirectionTypes.WorldBackward:
                    return Vector3.back;
                case DirectionTypes.WorldRight:
                    return Vector3.right;
                case DirectionTypes.WorldLeft:
                    return Vector3.left;
                case DirectionTypes.WorldUp:
                    return Vector3.up;
                case DirectionTypes.WorldBottom:
                    return Vector3.down;
                default:
                    return Vector3.zero;
            }
        }
    }
}