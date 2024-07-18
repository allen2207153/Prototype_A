using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing
{
    private float _climbHeight = 3f;
    private float _lowClimbHight = 0.5f;
    private float _bodyHight = 1f;
    private float _checkDistance = 1f;
    private Vector3 _climbHitNormal;

    public bool ClimbDetect(Transform playerTransform, Vector3 playerInput, out Vector3 climbPos)
    {
        climbPos = default(Vector3);
        Vector3 rayStart = playerTransform.position + Vector3.up * _lowClimbHight;
        Vector3 rayDirection = playerTransform.forward;

        Debug.DrawRay(rayStart, rayDirection * _checkDistance, Color.red);

        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit obsHit, _checkDistance))
        {
            _climbHitNormal = obsHit.normal;
            if (Vector3.Angle(-_climbHitNormal, playerTransform.forward) > 45
                || Vector3.Angle(-_climbHitNormal, playerInput) > 45)
            {
                return false;
            }

            if (Physics.Raycast(playerTransform.position + Vector3.up * _lowClimbHight
                , -_climbHitNormal
                , out RaycastHit firstWallHit
                , _checkDistance))
            {
                Debug.DrawRay(playerTransform.position + Vector3.up * _lowClimbHight
                    , -_climbHitNormal * _checkDistance
                    , Color.red);

                if (Physics.Raycast(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight)
                    , -_climbHitNormal
                    , out RaycastHit secondWallHit
                    , _checkDistance))
                {
                    Debug.DrawRay(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight)
                        , -_climbHitNormal * _checkDistance
                        , Color.red);

                    if (Physics.Raycast(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 2)
                        , -_climbHitNormal
                        , out RaycastHit thirdWallHit
                        , _checkDistance))
                    {
                        Debug.DrawRay(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 2)
                            , -_climbHitNormal * _checkDistance
                            , Color.red);

                        if (Physics.Raycast(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 3)
                            , -_climbHitNormal
                            , _checkDistance))
                        {
                            Debug.DrawRay(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 3)
                                , -_climbHitNormal * _checkDistance
                                , Color.red);

                            return false;
                        }
                        else if (Physics.Raycast(thirdWallHit.point + Vector3.up * _bodyHight
                            , Vector3.down
                            , out RaycastHit ledgeHit
                            , _bodyHight))
                        {

                            Debug.DrawRay(thirdWallHit.point + Vector3.up * _bodyHight
                                , Vector3.down * _bodyHight
                                , Color.red);

                            climbPos = ledgeHit.point;
                            return true;
                        }

                    }
                    else if (Physics.Raycast(secondWallHit.point + Vector3.up * _bodyHight
                            , Vector3.down
                            , out RaycastHit ledgeHit
                            , _bodyHight))
                    {

                        Debug.DrawRay(secondWallHit.point + Vector3.up * _bodyHight
                                , Vector3.down * _bodyHight
                                , Color.red);

                        climbPos = ledgeHit.point;
                        return true;
                    }
                }
                else if (Physics.Raycast(firstWallHit.point + Vector3.up * _bodyHight
                            , Vector3.down
                            , out RaycastHit ledgeHit
                            , _bodyHight))
                {

                    Debug.DrawRay(firstWallHit.point + Vector3.up * _bodyHight
                                , Vector3.down * _bodyHight
                                , Color.red);

                    climbPos = ledgeHit.point;
                    return true;
                }
            }
        }
        return false;
    }
}
