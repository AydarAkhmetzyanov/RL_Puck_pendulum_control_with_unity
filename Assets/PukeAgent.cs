using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PukeAgent : Agent
{
    public GameObject rode;
    public GameObject sphere;
    public GameObject target;

    Rigidbody rBody;
    Rigidbody rBodyRode;
    Rigidbody rBodySphere;

    int step = 0;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBodyRode = rode.GetComponent<Rigidbody>();
        rBodySphere = sphere.GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    {
        step = 0;
        var x = -1.82f * Mathf.Sin((Mathf.Deg2Rad * step / 10) + Mathf.PI / 2);
        var z = -1.82f * Mathf.Cos((Mathf.Deg2Rad * step / 10) + Mathf.PI / 2);
        this.target.transform.localPosition = new Vector3(x, 0.3f, z);

        this.transform.localPosition = new Vector3(-1.82f, 0.167f, 0f);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        

        rode.transform.localPosition = new Vector3(-1.82f, 3.244f, 0f);
        rode.transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.rBodyRode.angularVelocity = Vector3.zero;
        this.rBodyRode.velocity = Vector3.zero;

        sphere.transform.localPosition = new Vector3(-1.82f, 6.29f, 0f);
        sphere.transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.rBodySphere.angularVelocity = Vector3.zero;
        this.rBodySphere.velocity = Vector3.zero;

    }

    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(sphere.transform.localPosition.y);
        AddVectorObs(sphere.transform.localPosition.x - this.transform.localPosition.x);
        AddVectorObs(sphere.transform.localPosition.z - this.transform.localPosition.z);

        AddVectorObs(this.transform.localPosition.x);
        AddVectorObs(this.transform.localPosition.z);

        AddVectorObs(this.target.transform.localPosition.x);
        AddVectorObs(this.target.transform.localPosition.z);


        // Agent and target velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
        AddVectorObs(rBodySphere.velocity.x);
        AddVectorObs(rBodySphere.velocity.z);

        AddVectorObs(Vector3.Distance(this.transform.localPosition,
                                              new Vector3(0, 0, 0)));

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {


        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];

        //if (controlSignal.x > 0 && controlSignal.x < 1){
        //    controlSignal.x = 1;
        //}
        //if (controlSignal.x > 40)
        //{
        //    controlSignal.x = 40;
        //}

        //if (controlSignal.z > 0 && controlSignal.z < 1)
        //{
        //    controlSignal.z = 1;
        //}
        //if (controlSignal.z > 40)
        //{
        //    controlSignal.z = 40;
        //}

        //if (controlSignal.x < 0 && controlSignal.x > -1)
        //{
        //    controlSignal.x = -1;
        //}
        //if (controlSignal.x < -40)
        //{
        //    controlSignal.x = -40;
        //}

        //if (controlSignal.z < 0 && controlSignal.z > -1)
        //{
        //    controlSignal.z = -1;
        //}
        //if (controlSignal.z < -40)
        //{
        //    controlSignal.z = -40;
        //}




        rBody.AddForce(controlSignal);


        // Rewards
        float distanceToMiddle = Vector3.Distance(this.transform.localPosition,
                                              new Vector3(0,0,0));

        // too close to middle
        if (distanceToMiddle < 1f)
        {
            SetReward(-1.0f);
            Done();
            return;
        }

        // Fell off platform
        if (distanceToMiddle > 2.4f)
        {
            SetReward(-1.0f);
            Done();
            return;
        }

        //ball felt
        if ( sphere.transform.localPosition.y < 2f)
        {
            SetReward(-1.0f);
            Done();
            return;
        }

        step = step + 1;

        var x = -1.82f * Mathf.Sin((Mathf.Deg2Rad * (step ) / 20) + Mathf.PI / 2);
        var z = -1.82f * Mathf.Cos((Mathf.Deg2Rad * (step ) / 20) + Mathf.PI / 2);
        this.target.transform.localPosition = new Vector3(x, 0.3f, z);


        if (step == 7200)
        {
            SetReward(0f);
            Done();
            return;
        }

        SetReward((1 / (0.1f + Vector3.Distance(this.target.transform.localPosition,
                                              this.transform.localPosition)))/10 +

                                              (this.sphere.transform.localPosition.y/6));

    }

}
