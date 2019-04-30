using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

namespace KinectV2Components
{
	[RequireComponent(typeof(KinectAvatarController))]
	public class KinectBodyMonitor : KinectService
	{
		public Text UIMonitorLogger;

		protected KinectAvatarController controller;

		public void Awake()
		{
			controller = GetComponent<KinectAvatarController>();
            RegisterToKinectManager();
		}

		public override void Elaborate(Body[] bodies)
		{
			Body body = bodies[controller.Id];
			Windows.Kinect.Vector4 jointOrientation = body.JointOrientations[JointType.SpineBase].Orientation;
			Quaternion measuredQuaternion = new Quaternion(jointOrientation.X, jointOrientation.Y, -jointOrientation.Z,
				-jointOrientation.W);

			CameraSpacePoint spineBaseSpacePoint = body.Joints[JointType.SpineBase].Position;
			CameraSpacePoint headSpacePoint = body.Joints[JointType.Head].Position;
			Vector3 spineBasePosition = new Vector3(spineBaseSpacePoint.X, spineBaseSpacePoint.Y, spineBaseSpacePoint.Z);
			Vector3 headPosition = new Vector3(headSpacePoint.X, headSpacePoint.Y, -headSpacePoint.Z);
			Vector3 spineBaseToHead = headPosition - spineBasePosition;

			float angleX = Vector3.Dot(Vector3.up, new Vector3(spineBaseToHead.x, 1, 0)) * Mathf.Rad2Deg;
			float angleY = Vector3.Dot(Vector3.up, new Vector3(0, spineBaseToHead.y, 0)) * Mathf.Rad2Deg;
			float angleZ = Vector3.Dot(Vector3.up, new Vector3(0, 1, spineBaseToHead.z)) * Mathf.Rad2Deg;

			UIMonitorLogger.text = "Monitor: back\nObserved rotations:\n" + measuredQuaternion.eulerAngles.ToString() +
				"\nComputed angle:\n" + new Vector3(angleX, angleY, angleZ);
		}
	}
}

