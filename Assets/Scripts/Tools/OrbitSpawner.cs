using UnityEngine;
using System.Collections.Generic;

public class OrbitSpawner : MonoBehaviour
{
    public float radius = 2f;
    public float rotationSpeed = 50f;
    private readonly Dictionary<Passive, GameObject> _orbitingObjects = new();

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void AddOrbitalObject(Passive passive)
    {
        if (passive.definition.orbitalObjectModel == null)
            return;

        GameObject newObj = Instantiate(passive.definition.orbitalObjectModel, transform);

        _orbitingObjects.Add(passive, newObj);
        UpdateOrbitalPositions();
    }

    public void RemoveOrbitalObject(Passive passive)
    {
        if (!_orbitingObjects.ContainsKey(passive))
            return;
        Destroy(_orbitingObjects[passive]);
        _orbitingObjects.Remove(passive);
        UpdateOrbitalPositions();
    }

    void UpdateOrbitalPositions()
    {
        int count = _orbitingObjects.Count;
        int i = 0;
        foreach (var obj in _orbitingObjects.Values)
        {
            i++;
            float angle = i * Mathf.PI * 2f / count;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            obj.transform.localPosition = newPos;
            
            float angleDegrees = angle * Mathf.Rad2Deg;
            obj.transform.localRotation =Quaternion.Euler(0f, 0f, angleDegrees + 90f);
        }
    }
}