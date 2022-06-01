using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class UserAgent : Agent
{
    public Transform target;
    Rigidbody rBody;
    public JoyStickScript joyStick;

    public override void Initialize()
    {
        this.rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0.0f, 1f, 0.0f);
        }

        target.localPosition = new Vector3(
            Random.value * 8, 1f, -Random.value * 8);
    }

    class HitData
    {
        public int type;
        public float distance;
        public Vector3 direction;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        /// <summary>
        /// RaySensor‚ÌŽ©‘OŽÀ‘•
        /// </summary>
        /*
        List<RaycastHit> hits = new List<RaycastHit>();
        Color[] colors = new Color[3] { Color.red, Color.yellow, Color.green};
        List <HitData> hitDatas = new List<HitData>();
        for (int i = -1; i < 2; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(Mathf.Abs(i)*15), 0, Mathf.Sin(Mathf.Abs(i)*15));
            Vector3 direction = i < 0 ? new Vector3((transform.forward.x * pos.x) + (transform.forward.z * pos.z), 0, (transform.forward.x * pos.z) - (transform.forward.z * pos.x)) : 
                i > 0 ? new Vector3((transform.forward.x * pos.x) - (transform.forward.z * pos.z), 0, -((transform.forward.x * pos.z) + (transform.forward.z * pos.x))):
                transform.forward;
            Ray ray = new Ray(this.transform.position, direction);
            Debug.Log($"pos{i}: {pos}");
            Debug.DrawRay(ray.origin, ray.direction * 15, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int type = hit.collider.tag == "Goal" ? 1 : 0;
                float distance = hit.distance;
                hitDatas.Add(new HitData { type = type, distance = distance, direction = direction });
            }
            hits.Add(hit);
        }
        */
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-0.01f);
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * 50);
        if (controlSignal.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(controlSignal);

        float distanceToTarget = Vector3.Distance(
            this.transform.localPosition, target.localPosition);
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = joyStick.input.x;
        continuousActionsOut[1] = joyStick.input.y;
    }
}