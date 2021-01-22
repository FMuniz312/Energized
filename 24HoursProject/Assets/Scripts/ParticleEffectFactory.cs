using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ParticleEffectFactory : MonoBehaviour
{
    static public ParticleEffectFactory instance;
    [SerializeField] ParticlesData[] particlesData;

    private void Awake()
    {
        if(instance == null)instance = this;
    }
    public enum Particle
    {
        RedBallsExplosion,
        SpawnWarning,
        EnemyDeath
    }


    public GameObject CreateParticleEffect(Particle particletype, Vector3 spawnposition)
    {
        GameObject particleEffect = particlesData.Where((p) => p.particleType == particletype).Select(p => p.particleEffect).First();
        return Instantiate(particleEffect, spawnposition, Quaternion.identity);
    }

    [System.Serializable]
    public class ParticlesData
    {
        public GameObject particleEffect;
       public  Particle particleType;

    }
}
