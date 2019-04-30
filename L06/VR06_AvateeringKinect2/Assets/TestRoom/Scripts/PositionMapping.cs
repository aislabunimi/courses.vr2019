using System.Collections.Generic;
using UnityEngine;
using KinectV2Components;
using Windows.Kinect;
using System;

/// <summary>
/// Demo class for the position-based avateering.
/// </summary>
[RequireComponent(typeof(KinectAvatarController))]
public class PositionMapping : KinectService
{

	/// <summary>
	/// The Kinect Manager which provides data.
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

        RegisterToKinectManager();

		foreach (JointType jointType in Enum.GetValues(typeof(JointType)))
		{
			// Sets the joints dictionary according to the names of the scene's objects.
			GameObject jointGameObject = GameObject.Find(jointType.ToString());
			if (jointGameObject != null)
				joints[jointType] = jointGameObject;
		}
	}


	/// <summary>
	/// Maps each joint position to the respective game object's transform.
	/// </summary>
	/// <param name="bodies">The body data provided by Kinect.</param>
	public override void Elaborate(Body[] bodies)
	{
		Body playerBody = bodies[controller.Id];
		Dictionary<JointType, GameObject> joints = controller.JointObjects;

		foreach (JointType jointType in System.Enum.GetValues(typeof(JointType)))
		{
			// Acquisition of joint's 3D coordinates and conversion to Vector3 object.
			// We must remember that the Z axis is reverted!
			CameraSpacePoint jointMeasuredPosition = playerBody.Joints[jointType].Position;
			Vector3 jointPositionInUnity = new Vector3(jointMeasuredPosition.X, jointMeasuredPosition.Y,
				-jointMeasuredPosition.Z);

			// Position update.
			joints[jointType].transform.position = jointPositionInUnity;
		}
	}
}

