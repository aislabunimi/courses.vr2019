using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;

namespace KinectV2Components
{
	public class KinectManager : MonoBehaviour
	{
		/// <summary>
		/// The first player.
		/// </summary>
		public GameObject PlayerOne;


		/// <summary>
		/// The Kinect sensor.
		/// </summary>
		public KinectSensor Sensor { get; protected set; }


		/// <summary>
		/// The list of task the manager must execute for each frame.
		/// </summary>
		public List<KinectService> Tasks = new List<KinectService>();


		/// <summary>
		/// The PlayerOne's controller.
		/// </summary>
		protected KinectAvatarController playerOneController;


		/// <summary>
		/// Reader for BodyFrame structures.
		/// </summary>
		protected BodyFrameReader bodyFrameReader;


		/// <summary>
		/// An array which contains the tracked bodies.
		/// </summary>
		protected Body[] bodies;


		/// <summary>
		/// Index of PlayerOne in bodies array.
		/// </summary>
		protected Body PlayerOneBody { get; private set; }


		/// <summary>
		/// True if it is possible to log an untracked body.
		/// </summary>
		private bool canLogUntrackedBody;


		/// <summary>
		/// See <see cref="MonoBehaviour.Awake"/>.
		/// </summary>
		public void Awake()
		{
			// Sets the player's controller
			if (PlayerOne == null)
				return;
			playerOneController = PlayerOne.GetComponent<KinectAvatarController>();

			// Disables the PlayerOne gameObject.
			PlayerOne.SetActive(false);

			// Initializes Kinect V2 sensor
			InitializeKinectV2Sensor();
		}


		/// <summary>
		/// Performs Kinect sensor's initialization routine.
		/// </summary>
		protected void InitializeKinectV2Sensor()
		{
			// Gets and opens default sensor.
			Sensor = KinectSensor.GetDefault();
			if (!Sensor.IsOpen)
				Sensor.Open();

			// Sets bodyFrameReader.
			bodyFrameReader = Sensor.BodyFrameSource.OpenReader();

			// Initializes the bodies array.
			bodies = new Body[bodyFrameReader.BodyFrameSource.BodyCount];

			// Sets player 1's id to a default value.
			PlayerOneBody = null;
		}


		public void Update()
		{
			// Checks for sensor availability.
			if (!Sensor.IsAvailable)
			{
				Debug.Log("Sensor is not available.");
				return;
			}

			// Checks for sensor opennes.
			if (!Sensor.IsOpen)
			{
				Debug.Log("Sensor is closed.");
				return;
			}

			// Performs data acquisition for each frame.
			using (BodyFrame currentFrame = bodyFrameReader.AcquireLatestFrame())
			{
				if (currentFrame == null)
					return;

				// Updates bodies array.
				currentFrame.GetAndRefreshBodyData(bodies);

				// Looks for PlayerOne into frame.
				if (PlayerOneBody == null || !PlayerOneBody.IsTracked)
				{
					PlayerOne.SetActive(false);
					if (!FindPlayerOne())
					{
						if (canLogUntrackedBody)
							InvokeRepeating("LogUntrackedPlayerOne", 0.5f, 5f);
						return;
					}
					else
					{
						// Re-activates player and restores the logger status.
						PlayerOne.SetActive(true);
						CancelInvoke("LogUntrackedPlayerOne");
						Debug.Log("Found player One");
					}
				}

				// Executes each of the elaboration tasks in the given order.
				IEnumerator<KinectService> taskEnumerator = Tasks.GetEnumerator();
				while (taskEnumerator.MoveNext())
					taskEnumerator.Current.Elaborate(bodies);
			}
		}

		/// <summary>
		/// Looks for PlayerOne, if tracked, into bodies array.
		/// </summary>
		/// <returns>True if PlayerOne was found, false otherwise.</returns>
		protected bool FindPlayerOne()
		{
			// Loops into the bodies array and looks for a tracked body.
			for (int i = 0; i < bodies.Length; i++)
				if (bodies[i] != null && bodies[i].IsTracked)
				{
					PlayerOneBody = bodies[i];
					playerOneController.Id = i;
					return true;
				}
			return false;
		}


		public void OnApplicationQuit()
		{
			// Closes sensor.
			if (Sensor.IsOpen)
				Sensor.Close();

			// Disposes data structures.
			if (bodyFrameReader != null)
			{
				bodyFrameReader.Dispose();
				bodyFrameReader = null;
			}
		}


		/// <summary>
		/// Logs a warning saying the given body is not tracked.
		/// </summary>
		private void LogUntrackedPlayerOne()
		{
			Debug.LogWarning("Player 1 is not tracked.");
		}
	}

}
