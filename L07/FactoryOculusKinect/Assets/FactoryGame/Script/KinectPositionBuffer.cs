using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinectV2Components;
using System;
using Windows.Kinect;

namespace KinectV2Components
{
	[RequireComponent(typeof(KinectManager))]
	public class KinectPositionBuffer : KinectService
	{
		/// <summary>
		/// The player's controller.
		/// </summary>
		protected KinectAvatarController player;

		/// <summary>
		/// Contains the previous values of the joints positions.
		/// </summary>
		protected Dictionary<JointType, Vector3> bufferOld;

		/// <summary>
		/// Contains the current values of the joints positions.
		/// </summary>
		protected Dictionary<JointType, Vector3> bufferNew;

		/// <summary>
		/// Enumerates the buffered joints.
		/// </summary>
		protected List<JointType> bufferedJoints;


		public void Awake()
		{
			SetKinectManager();

			bufferedJoints = new List<JointType>()
			{
				JointType.HandTipLeft, JointType.HandTipRight, JointType.FootLeft, JointType.FootRight,
			};

			bufferOld = new Dictionary<JointType, Vector3>();
			bufferNew = new Dictionary<JointType, Vector3>();

			// Retrieves the player controller.
			player = gameObject.GetComponent<KinectManager>().PlayerOne.GetComponent<KinectAvatarController>();
		}


		public void Start()
		{
			foreach (JointType bufferedJoint in bufferedJoints)
			{
				// Initializes dictionaries. These default values will never be used for computing a direction
				// and will be overwritten after 2 frames.
				bufferOld[bufferedJoint] = player.JointObjects[bufferedJoint.ToString()].transform.position;
				bufferNew[bufferedJoint] = player.JointObjects[bufferedJoint.ToString()].transform.position;
			}
		}


		/// <summary>
		/// Buffers the required joint positions on each frame.
		/// </summary>
		/// <param name="bodies"></param>
		public override void Elaborate(Body[] bodies)
		{
			Body playerBody = bodies[player.Id];

			foreach (JointType jointType in bufferedJoints)
			{
				// Retrieves the joint position.
				CameraSpacePoint acquiredPosition = playerBody.Joints[jointType].Position;
				Vector3 newPosition = new Vector3(acquiredPosition.X, acquiredPosition.Y, acquiredPosition.Z);

				// Updates buffers.
				bufferOld[jointType] = bufferNew[jointType];
				bufferNew[jointType] = newPosition;
			}
		}


		protected override void SetKinectManager()
		{
			// Registers the task to the Kinect manager attached to this game object.
			gameObject.GetComponent<KinectManager>().Tasks.Add(this);
		}

		/// <summary>
		/// Computes the current direction of the given joint, based on the values stored into
		/// the buffers.
		/// </summary>
		/// <param name="jointName">The name of the buffered joint.</param>
		/// <returns>A vector which describes the direction.</returns>
		public Vector3 GetDirection(string jointName)
		{
			JointType joint = JointType.SpineBase;
			switch (jointName)
			{
				case "HandTipLeft":
					joint = JointType.HandTipLeft;
					break;
				case "HandTipRight":
					joint = JointType.HandTipRight;
					break;
				case "FootLeft":
					joint = JointType.FootLeft;
					break;
				case "FootRight":
					joint = JointType.FootRight;
					break;
				default:
					throw new ArgumentException("Invalid joint");
			}

			Vector3 direction = bufferNew[joint] - bufferOld[joint];
			return direction;

		}
	}
}


