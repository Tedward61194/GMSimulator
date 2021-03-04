using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    RaycastHit hit;
    Vector3 movePoint;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
        //if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8))) {
            transform.position = hit.point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
        //if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8))) {
            transform.position = hit.point;
        }

        if (Input.GetMouseButton(0)) {
            Instantiate(prefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
