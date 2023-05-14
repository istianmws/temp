using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour, IPointerDownHandler
{
    
    [SerializeField] Collider col;
    [SerializeField] Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] LineRenderer aimLine;
    [SerializeField] Transform aimworld;
    float forceFactor;
    bool shoot;
    bool shootingMode;
    Vector3 forceDirection;
    Ray ray;
    Plane plane;

    public bool ShootingMode { get => shootingMode; }

    private void Update()
    {
        if (shootingMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                aimLine.gameObject.SetActive(true);
                aimworld.gameObject.SetActive(true);
                plane = new Plane(Vector3.up, this.transform.position);

            }
            else if(Input.GetMouseButton(0))
            {
                var mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                var ballViewportPos = Camera.main.WorldToViewportPoint(this.transform.position);
                var ballScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                var pointerDirection = ballViewportPos - mouseViewportPos;
                pointerDirection.z = 0;
                
                //aim
                // aimLine.transform.position = ballScreenPos;
                // var positions = new Vector3[]{ballScreenPos, Input.mousePosition};
                // aimLine.SetPositions(positions);
                
                //force direction
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                plane.Raycast(ray, out var distance);
                forceDirection = (this.transform.position - ray.GetPoint(distance)); 
                forceDirection.Normalize();
                
                
                //power
                forceFactor =pointerDirection.magnitude*2;

                //visual
                aimworld.transform.position = this.transform.position;
                aimworld.forward = forceDirection;
                aimworld.localScale = new Vector3(1,1,0.5f+forceFactor);

            }
            else if (Input.GetMouseButtonUp(0))
            {
                shoot = true;
                shootingMode = false;
                
                aimLine.gameObject.SetActive(false );
                aimworld.gameObject.SetActive(false);
            }
        }
    }
    private void FixedUpdate() 
    {
        if (shoot)
        {
            shoot = false;
            Vector3 direction = Camera.main.transform.forward;
            direction.y = 0;
            rb.AddForce(direction * force * forceFactor, ForceMode.Impulse);
        }

        if (rb.velocity.sqrMagnitude > 0 && rb.velocity.sqrMagnitude < 0.01f)
        {
            rb.velocity = Vector3.zero;
        }
    }
    public bool IsMove()
    {
        return rb.velocity != Vector3.zero;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        shootingMode = true;
    }
}
