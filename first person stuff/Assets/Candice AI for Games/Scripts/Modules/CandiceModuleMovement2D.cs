using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandiceAIforGames.AI;

public class CandiceModuleMovement2D : CandiceBaseModule
{

    public CandiceModuleMovement2D(string moduleName = "CandiceModuleMovement2D") : base(moduleName) { }
    public void MoveForward2D(Transform transform, CandiceAIController2D aiController)
    {
        transform.position += transform.forward * aiController.MoveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.identity;
    }
    public void MoveForwardWithSlopeAlignment2D(Transform transform, CandiceAIController2D aiController)
    {
        var ray = new Ray(transform.position, Vector3.down);
        Vector3 velocity = transform.forward;
        if (Physics.Raycast(ray, out RaycastHit hitInfo, aiController.HalfHeight + 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            velocity = slopeRotation * velocity;
        }
        transform.position += velocity * aiController.MoveSpeed * Time.deltaTime;
    }

    public void LookAt2D(Transform transform, CandiceAIController2D aiController)
    {
        transform.LookAt(aiController.LookPoint);        
    }
    public void LookAway2D(Transform transform, CandiceAIController2D aiController)
    {
        transform.LookAt(-aiController.MainTarget.transform.forward);
    }

    public void RotateTo2D(Transform transform, CandiceAIController2D aiController)
    {
        float desiredAngle = 180;
        int direction = 1;
        float angle = Vector3.Angle((aiController.MainTarget.transform.position - aiController.transform.position), aiController.transform.right);
        if (angle > 90)
            angle = 360 - angle;
        if (angle > desiredAngle)
            direction = -1;
        float rotation = (direction * aiController.RotationSpeed) * Time.deltaTime;
        aiController.transform.Rotate(0, rotation, 0);
    }
}

