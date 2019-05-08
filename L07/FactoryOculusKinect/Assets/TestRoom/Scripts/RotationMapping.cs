using System.Collections.Generic;
using UnityEngine;
using KinectV2Components;
using Windows.Kinect;
using System;

/// <summary>
/// Demo class for the orientation-based avateering.
/// </summary>
[RequireComponent(typeof(KinectAvatarController))]
public class RotationMapping : KinectService
{
	[Header("Kinect Joint corrections")]
	/// <summary>
	/// Contains the joints which need a correction in their rotations.
	/// </summary>
	public List<JointType> correctionJoints;

	/// <summary>
	/// Describes the additional rotation that must be applied to the correction joints.
	/// </summary>
	public Vector3 correctiveRotation;

	/// <summary>
	/// The avatar's controller (mandatory on the gameObject).
	/// </summary>
	protected KinectAvatarController controller;

	/// <summary>
	/// Maps each joint type to its in-scene game object.
	/// </summary>
	protected Dictionary<JointType, GameObject> joints;


	public void Awake()
	{
		controller = gameObject.GetComponent<KinectAvatarController>();
		joints = new Dictionary<JointType, GameObject>();

		SetKinectManager();

		foreach (JointType jointType in Enum.GetValues(typeof(JointType)))
		{
			// Sets the joints dictionary according to the names of the scene's objects.
			GameObject jointGameObject = GameObject.Find(jointType.ToString());
			if (jointGameObject != null)
				joints[jointType] = jointGameObject;
		}
	}


	/// <summary>
	/// Maps each joint orientatation to the respective game object's transform.
	/// </summary>
	/// <param name="bodies"></param>
	public override void Elaborate(Body[] bodies)
	{
		Body playerBody = bodies[controller.Id];

		foreach (JointType jointType in Enum.GetValues(typeof(JointType)))
		{
			// If the joint is not in the scene then do not update rotation.
			if (!joints.ContainsKey(jointType))
				continue;

			// Retrieves the joint correction.
			Quaternion correctiveQuaternion = correctionJoints.Contains(jointType) ? 
				Quaternion.Euler(correctiveRotation) : Quaternion.identity;

			// Retrieves and applies new rotation.
			Windows.Kinect.Vector4 measuredRotation = playerBody.JointOrientations[jointType].Orientation;
			Quaternion unityRotation = new Quaternion(-measuredRotation.X, -measuredRotation.Y, measuredRotation.Z,
				measuredRotation.W);

			joints[jointType].transform.rotation = unityRotation * correctiveQuaternion;
		}
	}


	/// <summary>
	/// Sets the kinectManager field.
	/// </summary>
	protected override void SetKinectManager()
	{
		// Acquires the Kinect manager from the avatar controller.
		KinectManager kinectManager = controller.kinectManager;

		// Adds the service to the Kinect manager's list.
		kinectManager.Tasks.Add(this);
	}
}

