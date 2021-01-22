using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PrefabFactory : MonoBehaviour
{
    [SerializeField] ProductData[] productDatas;
    public static PrefabFactory instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public enum FactoryProduct
    {
        JumpTarget,
        BlackHole
    }

    public GameObject CreateItem(FactoryProduct factoryProduct)
    {
       return CreateItem(factoryProduct, Vector3.zero);
    }
    public GameObject CreateItem(FactoryProduct factoryProduct, Vector3 position)
    {
        GameObject gameObject = GetItem(factoryProduct);

        return Instantiate(gameObject,position,Quaternion.identity);
    }

    GameObject GetItem(FactoryProduct factoryProduct)
    {
        return productDatas.Where(p => p.factoryProductType == factoryProduct).Select(p => p.prefab).First();
    }


    [System.Serializable]
    public class ProductData
    {
        public GameObject prefab;
        public FactoryProduct factoryProductType;
    }

   
}
