using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace KinectV2Components
{
	public class KinectAvatarController : MonoBehaviour
	{
		/// <summary>
		/// A pointer to the root joint, used in the inizialization procedure for building the joint hierarchy.
		/// </summary>
		public GameObject RootJoint;

		/// <summary>
		/// The index of the body in the bodies array.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Maps each joint name to the respective GameObject
		/// </summary>
		public Dictionary<JointType, GameObject> JointObjects { get; protected set; }


		/// <summary>
		/// Maps each joint type to its parent joint type.
		/// </summary>
		public static readonly Dictionary<JointType, JointType> ParentJoint = new Dictionary<JointType, JointType> ()
		{
			{ JointType.SpineBase, JointType.SpineBase },
			{ JointType.SpineMid, JointType.SpineBase },
			{ JointType.SpineShoulder, JointType.SpineMid },
			{ JointType.Neck, JointType.SpineShoulder },
			{ JointType.Head, JointType.Neck },

			{ JointType.ShoulderRight, JointType.SpineShoulder },
			{ JointType.ElbowRight, JointType.ShoulderRight },
			{ JointType.WristRight, JointType.ElbowRight },
			{ JointType.ThumbRight, JointType.WristRight },
			{ JointType.HandRight, JointType.WristRight },
			{ JointType.HandTipRight, JointType.HandRight },

			{ JointType.ShoulderLeft, JointType.SpineShoulder },
			{ JointType.ElbowLeft, JointType.ShoulderLeft },
			{ JointType.WristLeft, JointType.ElbowLeft },
			{ JointType.HandLeft, JointType.WristLeft },
			{ JointType.ThumbLeft, JointType.WristLeft },
			{ JointType.HandTipLeft, JointType.HandLeft },

			{ JointType.HipRight, JointType.SpineBase },
			{ JointType.KneeRight, JointType.HipRight },
			{ JointType.AnkleRight, JointType.KneeRight },
			{ JointType.FootRight, JointType.AnkleRight },

			{ JointType.HipLeft, JointType.SpineBase },
			{ JointType.KneeLeft, JointType.HipLeft },
			{ JointType.AnkleLeft, JointType.KneeLeft },
			{ JointType.FootLeft, JointType.AnkleLeft },
		};


		/// <summary>
		/// Maps each joint type to the respective parent joint, i.e., the joint from which the parent bone starts.
		/// </summary>
		public Dictionary<JointType, GameObject> ParentJointGameObject { get; private set; }

		private void Awake ()
		{
			// Initializes dictionaries.
			ParentJointGameObject = new Dictionary<JointType, GameObject> ();
			JointObjects = new Dictionary<JointType, GameObject>();

			// Initializes gameobjects.
			SkeletonExploration(RootJoint);
		}


		/// <summary>
		/// Explores the GameObject hierarchy and initializes the joint gameobjects.
		/// <param name="root">The object from which the exploration starts</param>
		/// </summary>
		private void SkeletonExploration (GameObject root)
		{
			if (root.CompareTag("AvatarJoint"))
			{
				// Adds the joint to the hierarchy.
                try
                {
                    JointType joint = (JointType) System.Enum.Parse(typeof(JointType), root.name);
                    JointObjects[joint] = root;
                    if (IsLeaf(joint))
                        // Stops exploration if the joint is a leaf.
                        return;
                }
                catch (System.ArgumentException)
                {
                    // We found a game object which does not correspond to a joint.
                    Debug.LogWarning("[KINECT AVATAR CONTROLLER]: The object " + root.name + " is tagged as" +
                        " a Kinect joint but its name does not correspond to a joint.");
                }
			}
			// Explores the child objects.
			foreach (Transform child in root.transform)
				SkeletonExploration (child.gameObject);
		}

        /// <summary>
		/// Checks if a joint is a leaf joint.
		/// </summary>
		/// <param name="jointName">The joint to check.</param>
		/// <returns>True if the joint is a leaf, false otherwise.</returns>
        public static bool IsLeaf(JointType joint)
        {
            switch (joint)
            {
                case JointType.Head:
                case JointType.HandTipLeft:
                case JointType.ThumbLeft:
                case JointType.HandTipRight:
                case JointType.ThumbRight:
                case JointType.FootLeft:
                case JointType.FootRight:
                    return true;
                default:
                    return false;
            }
        }
	}
}


