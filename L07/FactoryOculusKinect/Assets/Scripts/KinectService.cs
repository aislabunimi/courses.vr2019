using UnityEngine;
using Windows.Kinect;

namespace KinectV2Components
{
	public abstract class KinectService : MonoBehaviour
	{
		/// <summary>
		/// Adds this service to the respective Kinect Manager.
		/// </summary>
		protected abstract void SetKinectManager();


		/// <summary>
		/// Performs an elaboration on the given Kinect frame.
		/// </summary>
		/// <param name="bodies">The bodies array acquired by the Kinect frame.</param>
		public abstract void Elaborate(Body[] bodies);
	}
}