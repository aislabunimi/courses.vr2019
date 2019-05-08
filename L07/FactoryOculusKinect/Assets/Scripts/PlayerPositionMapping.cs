using System;
using UnityEngine;
using Windows.Kinect;

namespace KinectV2Components
{
	[RequireComponent(typeof(KinectAvatarController))]
	public class PlayerPositionMapping : KinectService
	{
		/// <summary>
		/// A gameObject (should be empty) which defines the origin of the reference frame.
		/// Player's SpineBase coordinates will be updated with respect to this origin.
		/// </summary>
		public GameObject origin;


		/// <summary>
		/// A coefficient that helps in solving the problem of the measure proportions disparity from 
		/// Kinect's world to Unity's world.
		/// </summary>
		public float positionMultiplyer = 1f;


		/// <summary>
		/// The avatar's controller (mandatory on the gameObject).
		/// </summary>
		protected KinectAvatarController controller;


		/// <summary>
		/// See <see cref="MonoBehaviour.Awake"/>.
		/// </summary>
		public void Awake()
		{
			controller = gameObject.GetComponent<KinectAvatarController>();

			SetKinectManager();
		}


		protected override void SetKinectManager()
		{
			// Acquires the Kinect Manager from the avatar controller.
			KinectManager kinectManager = controller.kinectManager;

			// Adds the service to the Kinect Manager.
			kinectManager.Tasks.Add(this);
		}


		/// <summary>
		/// Updates the avatar position with a computation based on the SpineBase's position.
		/// </summary>
		/// <param name="bodies">The bodies tracked by Kinect.</param>
		public override void Elaborate(Body[] bodies)
		{
			// Retrieves the SpineBase position.
			CameraSpacePoint kinectSpineBase = bodies[controller.Id].Joints[JointType.SpineBase].Position;
			Vector3 spineBaseAcquiredPosition = new Vector3(kinectSpineBase.X, kinectSpineBase.Y, -kinectSpineBase.Z);

			// Applies the coefficient.
			spineBaseAcquiredPosition *= positionMultiplyer;

			// Computes the absolute coordinates.
			spineBaseAcquiredPosition += origin.transform.position;

			// Applies the translation.
			gameObject.transform.position = spineBaseAcquiredPosition;
		}
	}
}
