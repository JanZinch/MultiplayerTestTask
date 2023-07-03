using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class UniStructsExtensions
    {
        #region Vector2

        public static Vector2 WithX(this Vector2 vector2, float x)
        {
            return new Vector2(x, vector2.y);
        }
        
        public static Vector2 WithY(this Vector2 vector2, float y)
        {
            return new Vector2(vector2.x, y);
        }
        
        #endregion
        
        #region Vector3

        public static Vector3 WithX(this Vector3 vector3, float x)
        {
            return new Vector3(x, vector3.y, vector3.z);
        }
        
        public static Vector3 WithY(this Vector3 vector3, float y)
        {
            return new Vector3(vector3.x, y, vector3.z);
        }
        
        public static Vector3 WithZ(this Vector3 vector3, float z)
        {
            return new Vector3(vector3.x, vector3.y, z);
        }

        #endregion
        
        #region Color

        public static Color WithRed(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }
        
        public static Color WithGreen(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }
        
        public static Color WithBlue(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }
        
        public static Color WithAlpha(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }

        #endregion
        
        #region Color32

        public static Color32 WithRed(this Color32 color, byte r)
        {
            return new Color32(r, color.g, color.b, color.a);
        }
        
        public static Color32 WithGreen(this Color32 color, byte g)
        {
            return new Color32(color.r, g, color.b, color.a);
        }
        
        public static Color32 WithBlue(this Color32 color, byte b)
        {
            return new Color32(color.r, color.g, b, color.a);
        }
        
        public static Color32 WithAlpha(this Color32 color, byte a)
        {
            return new Color32(color.r, color.g, color.b, a);
        }

        #endregion

        #region LayerMask

        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & (1 << layer)) != 0;
        }
        
        #endregion
        

        #region Component

        public static bool HasComponent<TComponent>(this Component component)
        {
            return component.TryGetComponent<TComponent>(out TComponent reference);
        }
        
        public static bool HasComponent(this Component component, Type type)
        {
            return component.TryGetComponent(type, out Component reference);
        }
        
        public static bool HasComponent<TComponent>(this GameObject gameObject)
        {
            return gameObject.TryGetComponent<TComponent>(out TComponent reference);
        }
        
        public static bool HasComponent(this GameObject gameObject, Type type)
        {
            return gameObject.TryGetComponent(type, out Component reference);
        }
        
        
        
        
        #endregion

        #region Transform

        public static TComponent GetClosest<TComponent>(this Transform transform, IEnumerable<TComponent> other, Vector3 offset = new Vector3()) where TComponent : Component
        {
            TComponent closest = null;
            float minDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position + offset;
            
            foreach (TComponent otherOne in other)
            {
                float distance = Vector3.Distance(otherOne.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    closest = otherOne;
                    minDistance = distance;
                }
            }
            return closest;
        }

        public static void LookAt2D(this Transform transform, Vector3 worldPosition)
        {
            Vector3 direction = (worldPosition - transform.position).normalized;
            
            float targetRotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotationZ);
        }
        
        public static void LookAt2D(this Transform transform, Vector3 worldPosition, float maxZRotation)
        {
            Vector3 direction = (worldPosition - transform.position).normalized;
            
            float targetRotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(targetRotationZ, targetRotationZ - maxZRotation, targetRotationZ + maxZRotation));
        }
        
        public static Matrix4x4 TransformTo(this Transform from, Transform to) => to.worldToLocalMatrix * from.localToWorldMatrix;
        
        #endregion

        #region RectTransform

        public static void SetWidth(this RectTransform tr, float width)
        {
            tr.sizeDelta = tr.sizeDelta.WithX(width);
        }

        public static void SetHeight(this RectTransform tr, float height)
        {
            tr.sizeDelta = tr.sizeDelta.WithY(height);
        }

        public static void SetLocalX(this RectTransform tr, float x)
        {
            var p = tr.localPosition;
            tr.localPosition = new Vector3(x, p.y, p.z);
        }
        
        public static void SetLocalY(this RectTransform tr, float y)
        {
            var p = tr.localPosition;
            tr.localPosition = new Vector3(p.x, y, p.z);
        }
        
        public static void SetLocalZ(this RectTransform tr, float z)
        {
            var p = tr.localPosition;
            tr.localPosition = new Vector3(p.x, p.y, z);
        }

        public static void Shift(this RectTransform tr, Vector2 shift)
        {
            tr.localPosition += (Vector3)shift;
        }

        public static Rect GetPixelRect(this RectTransform tr)
            => RectTransformUtility.PixelAdjustRect(tr, tr.GetComponent<Canvas>());

        public static Vector2 GetPixelSize(this RectTransform tr) => tr.GetPixelRect().size;

        public static float GetPixelWidth(this RectTransform tr) => tr.GetPixelSize().x;
        
        public static float GetPixelHeight(this RectTransform tr) => tr.GetPixelSize().y;
        
        

        #endregion
        
    }
    
}