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
		/// The avatar's controller (mandatory on the gameObject).
		/// </summary>
		private KinectAvatarController controller;

        /// <summary>
        /// Position multipliers.
        /// </summary>
        private float multX, multY, multZ;

        /// <summary>
        /// Reference to the attached Transform.
        /// </summary>
        private Transform tr;

		/// <summary>
		/// See <see cref="MonoBehaviour.Awake"/>.
		/// </summary>
		public void Awake()
		{
			controller = GetComponent<KinectAvatarController>();
            tr = GetComponent<Transform>();

            // Retrieves the position multipliers.
            multX = controller.transform.lossyScale.x;
            multY = controller.transform.lossyScale.y;
            multZ = controller.transform.lossyScale.z;

            RegisterToKinectManager();
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
            spineBaseAcquiredPosition.x *= multX;
            spineBaseAcquiredPosition.y *= multY;
            spineBaseAcquiredPosition.z *= multZ;

			// Computes the absolute coordinates.
			spineBaseAcquiredPosition += origin.transform.position;

			// Applies the translation.
			tr.position = spineBaseAcquiredPosition;
		}
	}
}
