using System;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace KinectV2Components
{
	[RequireComponent(typeof(KinectAvatarController))]
	public class Avateering : KinectService
	{
		/// <summary>
		/// Check this if you want to use a KinectV1-rigged avatar.
		/// </summary>
		public bool useKinectV1Skeleton = false;


		/// <summary>
		/// The offset rotation for the avatar.
		/// </summary>
		public Vector3 offsetRotation = Vector3.zero;


		/// <summary>
		/// The avatar's controller (mandatory on the gameObject).
		/// </summary>
		protected KinectAvatarController controller;


		/// <summary>
		/// A dictionary which returns the correction offset for each joint.
		/// Note: new Quaternion(0, 1, 0, -1) = Quaternion.Euler(0, 90, 0)
		/// Note: new Quaternion(0, -1, 0, -1) = Quaternion.Euler(0, -90, 0)
		/// </summary>
		private Dictionary<JointType, Quaternion> offsetRotations = new Dictionary<JointType, Quaternion>()
		{
			{ JointType.ElbowLeft, new Quaternion(0, 1, 0, -1) },
			{ JointType.ElbowRight, new Quaternion(0, -1, 0, -1) },
			{ JointType.WristLeft, new Quaternion(0, 1, 0, -1) },
			{ JointType.WristRight, new Quaternion(0, -1, 0, -1) },
			{ JointType.HandLeft, new Quaternion(0, 1, 0, -1) },
			{ JointType.HandRight, new Quaternion(0, -1, 0, -1) },
			{ JointType.KneeLeft, new Quaternion(0, 1, 0, -1) },
			{ JointType.KneeRight, new Quaternion(0, -1, 0, -1) },
			{ JointType.AnkleLeft, new Quaternion(0, 1, 0, -1) },
			{ JointType.AnkleRight, new Quaternion(0, -1, 0, -1) },
		};


		/// <summary>
		/// Contains the joints that were added in the Kinect V2 skeleton.
		/// </summary>
		protected List<JointType> KinectV2Joints = new List<JointType>()
		{
			JointType.SpineShoulder,
			JointType.ThumbLeft,
			JointType.ThumbRight,
			JointType.HandTipLeft,
			JointType.HandTipRight,
		};


		/// <summary>
		/// See <see cref="MonoBehaviour.Awake"/>
		/// </summary>
		public void Awake()
		{
			controller = GetComponent<KinectAvatarController>();

            // Registers the task to the Kinect Manager.
            RegisterToKinectManager();
		}


        /// <summary>
        /// Updates the avatar's position and joint orientations.
        /// </summary>
        /// <param name="bodies">The bodies tracked by Kinect.</param>
        public override void Elaborate(Body[] bodies)
        {
			// Retrieves the body informations.
			Body avatarBody = bodies[controller.Id];
			Dictionary<JointType, JointOrientation> orientations = avatarBody.JointOrientations;

			// Defines utility quaternions.
			Quaternion adjust;

			// Creates a breadth-first joint traversal enumerator.
			IEnumerator<JointType> jointEnumerator = 
				((IEnumerable<JointType>)Enum.GetValues(typeof(JointType))).GetEnumerator();

			while (jointEnumerator.MoveNext())
			{
				// Retrieves current joint.
				JointType currentJoint = jointEnumerator.Current;

				// Skip this phase if we are considering a KinectV2 joint in a KinectV1-rigged avatar.
				if (useKinectV1Skeleton && KinectV2Joints.Contains(currentJoint))
					continue;

				// If current joint is leaf then estimate the orientation from the positions.
				if (KinectAvatarController.IsLeaf(currentJoint))
				{
					// Uncomment the following statement if you want to estimate the orientations for the leaf joints.
					// EstimateJointOrientation(currentJoint, avatarBody.Joints);
					continue;
				}

				// Gets rotating  joint's gameobject.
				GameObject rotatingJoint = controller.JointObjects[currentJoint];
				

				// Gets the quaternion acquired from Kinect.
				// Also reverses the Z axis and the angle in order to convert the reference frame.
				Quaternion newOrientation = new Quaternion(orientations[currentJoint].Orientation.X,
						orientations[currentJoint].Orientation.Y, -orientations[currentJoint].Orientation.Z,
						-orientations[currentJoint].Orientation.W);

				// Adds offset if needed.
				try
				{
					adjust = offsetRotations[currentJoint];
				}
				catch (KeyNotFoundException)
				{
					adjust = Quaternion.identity;
				}
				newOrientation *= adjust;

				// Applies rotation by considering also the avatar orientation in world coordinates.
				// There is no need of spherical interpolation since Kinect already provides a frame-by-frame set 
				// of orientations.
				rotatingJoint.transform.rotation = Quaternion.Euler(offsetRotation) * newOrientation;
			}
		}


		/// <summary>
		/// Estimates the new joint orientations from the current positions.
		/// </summary>
		/// <param name="currentJoint">The joint whose orientation has to be estimated.</param>
		/// <param name="positions">The Joints dictionary provided by Kinect Body struct.</param>
		public void EstimateJointOrientation(JointType currentJoint, Dictionary<JointType, Windows.Kinect.Joint> positions)
		{
			// Retrieves current joint's new position with the Z coordinate reversed because of the
			// frame references disparity.
			Windows.Kinect.Joint kinectJoint = positions[currentJoint];
			Vector3 newPosition = new Vector3(kinectJoint.Position.X, kinectJoint.Position.Y, -kinectJoint.Position.Z);

			// Retrieves parent joint's new position.
			JointType parentJoint = KinectAvatarController.ParentJoint[currentJoint];
			Windows.Kinect.Joint parent = positions[parentJoint];
			Vector3 parentPosition = new Vector3(parent.Position.X, parent.Position.Y, -parent.Position.Z);

			// Computes the direction from parent to current joint.
			Vector3 newDirection = newPosition - parentPosition;

			// Computes the new orientation.
			Quaternion newOrientation = Quaternion.FromToRotation(Vector3.up, newDirection);

			// Applies the new orientation.
			GameObject jointObject = controller.JointObjects[currentJoint];
			jointObject.transform.rotation = Quaternion.Euler(offsetRotation) * newOrientation;
		}
	}
}
