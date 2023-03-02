using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int projectileStartingNum;
    [SerializeField] List<Projectile> projectileList;
    
    private void Awake() {
        for (int i = 0; i < projectileStartingNum; i++) {
            Projectile newProjectile = InstantiateNewProjectile();
            projectileList.Add(newProjectile);
        }
    }

    public Projectile GetInactiveProjectile() {
        if (FindInactiveProjectile(out Projectile projectile)) return projectile;
        return InstantiateNewProjectile();
    }

    public bool FindInactiveProjectile(out Projectile _projectile) {
        foreach (Projectile projectile in projectileList) {
            if (!projectile.IsActive) {
                _projectile = projectile;
                return true;
            }
        }
        _projectile = null;
        return false;
    }

    public Projectile InstantiateNewProjectile() {
        Projectile newProjectile = Instantiate(projectilePrefab, transform).GetComponent<Projectile>();
        newProjectile.Deactivate();
        return newProjectile;
    }
}
