// ISTA 425 / INFO 525 Algorithms for Games
//
// Sample code file

using UnityEngine;

public class NPCCar : Car
{
    public override void SetInputs()
    {
        //SetInputs(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetKey(KeyCode.Space));
    }
    public void SetInputs(float steer, float move, bool brake)
    {
        steerInput = steer;
        moveInput = move;
        brakeInput = brake;
    }
}


