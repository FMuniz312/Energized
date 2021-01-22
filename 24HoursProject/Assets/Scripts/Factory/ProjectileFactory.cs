using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;

    static public ProjectileFactory instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    public GameObject CreateProjectile(Vector3 direction)
    {
        return CreateProjectile(Vector3.zero, direction);
    }
    public GameObject CreateProjectile(Vector3 direction, float speed)
    {
       return CreateProjectile(Vector3.zero, direction, speed);
    }
    public GameObject CreateProjectile(Vector3 direction, float speed, bool isitenemyprojectile)
    {
        return CreateProjectile(Vector3.zero, direction, speed, isitenemyprojectile);
    }

    public GameObject CreateProjectile(Vector3 position, Vector3 direction, float speed = 3, bool isitenemyprojectile = true)
    {
        
        ProjectileBehaviour projectileBehaviour = Instantiate(projectilePrefab, position, Quaternion.identity).GetComponent<ProjectileBehaviour>();
        projectileBehaviour.SetBehaviour(direction, speed, isitenemyprojectile);
        return projectileBehaviour.gameObject;

    }
}
