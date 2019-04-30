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
		private KinectAvatarController player;

		/// <summary>
		/// Contains the previous values of the joints positions.
		/// </summary>
		private Dictionary<JointType, Vector3> bufferOld;

		/// <summary>
		/// Contains the current values of the joints positions.
		/// </summary>
		private Dictionary<JointType, Vector3> bufferNew;

        [SerializeField]
		/// <summary>
		/// Enumerates the buffered joints.
		/// </summary>
		private JointType[] bufferedJoints;


		public void Awake()
		{
            // Registers the task to the Kinect Manager.
            RegisterToKinectManager();

			bufferOld = new Dictionary<JointType, Vector3>();
			bufferNew = new Dictionary<JointType, Vector3>();

            // Retrieves the player controller.
			player = KinectManager.Instance.PlayerOne.GetComponent<KinectAvatarController>();
		}


		private void Start()
		{
			foreach (JointType bufferedJoint in bufferedJoints)
			{
                // Initializes dictionaries. These default values will never be used for computing a direction
                // and will be overwritten after 2 frames.
                try
                {
                    bufferOld[bufferedJoint] = player.JointObjects[bufferedJoint].transform.position;
                    bufferNew[bufferedJoint] = player.JointObjects[bufferedJoint].transform.position;
                }
				catch (KeyNotFoundException)
                {
                    Debug.LogError("[KINECT POSITION BUFFER]: Joint " + bufferedJoint.ToString() + " not found.");
                }
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

		/// <summary>
		/// Computes the current direction of the given joint, based on the values stored into
		/// the buffers.
		/// </summary>
		/// <param name="jointName">The name of the buffered joint.</param>
		/// <returns>A vector which describes the direction.</returns>
		public Vector3 GetDirection(string jointName)
		{
            JointType joint = (JointType)Enum.Parse(typeof(JointType), jointName);

            try
            {
                Vector3 direction = bufferNew[joint] - bufferOld[joint];
                return direction;
            }
			catch (KeyNotFoundException)
            {
                Debug.LogError("[KINECT POSITION BUFFER]: Joint " + joint.ToString() + " not found.");
                return Vector3.zero;
            }
		}


        public Vector3 GetDirection(JointType joint)
        {
            return bufferNew[joint] - bufferOld[joint];
        }
	}
}


