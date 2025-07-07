using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DragProjectile : MonoBehaviour
{
    public Vector3 targetPosition;
    private Vector3 m_velocity = Vector3.zero;
    private bool targetIsDestination;
    public Hero source;
    public Entity target;
    public AbilityDefinition linkedAbility;
    public Image model;
    public ParticleSystem trail;
    [FormerlySerializedAs("contact")] public ParticleSystem contactParticles;

    [Header("Settings")] public float acceleration = 20f;
    public float maxSpeed = 15f;
    public float friction = 5f;
    public float distanceTrigger = 10;

    public void Update()
    {
        targetPosition.z = 0;

        // Direction du projectile vers le doigt
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Accélération dans la direction
        m_velocity += direction * (acceleration * Time.deltaTime);

        // Limite de vitesse max
        m_velocity = Vector3.ClampMagnitude(m_velocity, maxSpeed);

        // Optionnel : friction si le joueur arrête son doigt
        m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, friction * Time.deltaTime);

        // Déplacement
        transform.position += m_velocity * Time.deltaTime;

        // Rotation vers le mouvement (optionnel)
        if (m_velocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(m_velocity.y, m_velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (targetIsDestination)
        {
            if (Vector3.Distance(transform.position, targetPosition) <= distanceTrigger)
            {
                StartCoroutine(Impact());
            }
        }
    }

    public void SetTarget(Vector3 destination, Entity target)
    {
        friction *= 3;
        maxSpeed *= 3;
        acceleration *= 3;
        targetIsDestination = true;
        targetPosition = destination;
        this.target = target;
    }

    private IEnumerator Impact()
    {
        targetIsDestination = false;
        model.gameObject.SetActive(false);
        trail.Stop();
        contactParticles.gameObject.SetActive(true);
        contactParticles.Play();
        source.TryCastAbility(linkedAbility, target);


        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}