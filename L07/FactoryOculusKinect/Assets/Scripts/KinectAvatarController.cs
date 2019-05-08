using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace KinectV2Components
{
	public class KinectAvatarController : MonoBehaviour
	{
		/// <summary>
		/// The game's Kinect Manager from which the avatar retrieves the input data.
		/// </summary>
		public KinectManager kinectManager;

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
		public Dictionary<string, GameObject> JointObjects { get; protected set; }


		/// <summary>
		/// Names of the single joints.
		/// </summary>
		public static readonly List<string> JointNames = new List<string> ()
		{
			"SpineBase", "SpineMid", "SpineShoulder", "Neck", "Head",
			"ShoulderRight", "ElbowRight", "WristRight", "HandRight", "HandTipRight", "ThumbRight",
			"ShoulderLeft", "ElbowLeft", "WristLeft", "HandLeft", "HandTipLeft", "ThumbLeft",
			"HipRight", "KneeRight", "AnkleRight", "FootRight",
			"HipLeft", "KneeLeft", "AnkleLeft", "FootLeft"
		};


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

		public void Awake ()
		{
			// Initializes dictionaries.
			ParentJointGameObject = new Dictionary<JointType, GameObject> ();
			JointObjects = new Dictionary<string, GameObject>();

			// Initializes gameobjects.
			SkeletonExploration (RootJoint);
		}


		/// <summary>
		/// Explores the GameObject hierarchy and initializes the joint gameobjects.
		/// <param name="root">The object from which the exploration starts</param>
		/// </summary>
		protected void SkeletonExploration (GameObject root)
		{
			if (root.tag.Equals("AvatarJoint") && JointNames.Contains(root.name))
			{
				// Adds the joint to the hierarchy.
				JointObjects [root.name] = root;
				if (IsLeaf (root.name))
					// Stops exploration if the joint is a leaf.
					return;
			}
			// Explores the child objects.
			foreach (Transform child in root.transform)
				SkeletonExploration (child.gameObject);
		}


		/// <summary>
		/// Checks if a joint is a leaf joint.
		/// </summary>
		/// <param name="jointName">The joint name.</param>
		/// <returns>True if the joint is a leaf, false otherwise.</returns>
		public static bool IsLeaf (string jointName)
		{
			switch (jointName) {
				case "Head":
				case "HandTipLeft":
				case "ThumbLeft":
				case "HandTipRight":
				case "ThumbRight":
				case "FootLeft":
				case "FootRight":
					return true;
				default:
					return false;
			}
		}
	}
}


