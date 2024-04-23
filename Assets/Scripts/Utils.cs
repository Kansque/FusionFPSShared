using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-10, 10), 4, Random.Range(-10, 10));
    }

    public static void SetRenderLayerInChildren(Transform transform, int layerNum)
    {
        //foreach (Transform trans in transform.GetComponentInChildren<Transform>(true)) { 
         //   trans.gameObject.layer = LayerMask.NameToLayer("LocalPlayerModel");
       // }

        SetGameLayerRecursive(transform.gameObject, LayerMask.NameToLayer("LocalPlayerModel"));

       // Debug.Log("Here");
    }

   
    private static void SetGameLayerRecursive(GameObject _go, int _layer)
        {
            _go.layer = _layer;
            foreach (Transform child in _go.transform)
            {
                child.gameObject.layer = _layer;
 
                Transform _HasChildren = child.GetComponentInChildren<Transform>();
                if (_HasChildren != null)
                    SetGameLayerRecursive(child.gameObject, _layer);

            }
        }
}
