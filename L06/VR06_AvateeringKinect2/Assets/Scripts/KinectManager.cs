using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;

namespace KinectV2Components
{
	public class KinectManager : MonoBehaviour
	{
        // Singleton pattern implementation.
        private static KinectManager instance = null;
        /// <summary>
        /// Unique instance of the Kinect Manager.
        /// </summary>
        public static KinectManager Instance { get { return instance; } }

		/// <summary>
		/// Handle to the Kinect sensor.
		/// </summary>
		public KinectSensor Sensor { get; protected set; }

        [SerializeField]
		/// <summary>
		/// The PlayerOne's controller.
		/// </summary>
		protected KinectAvatarController playerOneController;

        /// <summary>
        /// Reference to the avatar's GameObject.
        /// </summary>
        public GameObject PlayerOne { get { return playerOneController.gameObject; } }

        /// <summary>
		/// The list of task the manager must execute for each frame.
		/// </summary>
		protected List<KinectService> Tasks = new List<KinectService>();

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
		private void Awake()
		{
            if (instance == null)
            {
                // Singleton initialization.
                instance = this;
                DontDestroyOnLoad(gameObject);

                // Checks player one.
                if (playerOneController == null)
                {
                    Debug.LogError("[KINECT MANAGER]: Player One Controller was not set.");
                    return;
                }

                // Initializes Kinect V2 sensor
                InitializeKinectV2Sensor();
            }
            else
                Destroy(gameObject);
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
			canLogUntrackedBody = true;
		}


        private void ShutdownKinectSensor()
        {
            Sensor.Close();
        }


		private void Update()
		{
			// Checks for sensor availability.
			if (!Sensor.IsAvailable)
			{
				Debug.LogError("Sensor is not available.");
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
					playerOneController.gameObject.SetActive(false);
					if (!FindPlayerOne())
					{
						if (canLogUntrackedBody)
							StartCoroutine(UntrackedBodyLogger(1, 5));
						return;
					}
					else
					{
						// Re-actives player and restores the logger status.
						playerOneController.gameObject.SetActive(true);
						StopCoroutine("UntrackedBodyLogger");
						canLogUntrackedBody = true;
					}
				}

                // Executes each of the elaboration tasks in the given order.
                int nTasks = Tasks.Count;
				for (int i = 0; i < nTasks; i++)
                    try
                    {
                        Tasks[i].Elaborate(bodies);
                    }
					catch (System.Exception e)
                    {
                        // We don't want an error in a single task to interrupt the entire cycle,
                        // so we log the exception and go on with the loop.
                        Debug.LogError(e.Message);
                    }
			}
		}

        /// <summary>
        /// Looks for player one in the world, if tracked, into the bodies array.
        /// </summary>
        /// <returns>True if player one was found, false otherwise.</returns>
        protected bool FindPlayerOne()
        {
            // We search the player who is closest to the center of the Kinect sensor.
            // We iterate along all the tracked bodies in order to find the one with
            // the minimum distance from the Kinect' center.
            bool found = false;
            float minDistanceFromZero = 100f;
            for (int i = 0; i < bodies.Length; i++)
                if (bodies[i] != null && bodies[i].IsTracked)
                {
                    float distanceFromZero = Mathf.Abs(bodies[i].Joints[JointType.SpineBase].Position.X);
                    if (distanceFromZero < minDistanceFromZero)
                    {
                        minDistanceFromZero = distanceFromZero;
                        playerOneController.Id = i;
                        found = true;
                    }
                }
            return found;
        }


		public void OnApplicationQuit()
		{
			// Closes sensor.
			if (Sensor.IsOpen)
				Sensor.Close();

			// Disposes data structures.
			if (bodyFrameReader != null)
				bodyFrameReader.Dispose();
		}


		/// <summary>
		/// Logs a warning saying the given body is not tracked and waits for 10 seconds before allowing 
		/// the next log.
		/// </summary>
		/// <param name="id">The untracked body id.</param>
        /// <param name="interval">The amount of time which elapses until the next log.</param>
		/// <returns></returns>
		protected IEnumerator UntrackedBodyLogger(int id, float interval = 10f)
		{
			Debug.LogWarning("Body " + id + " is not tracked");
			canLogUntrackedBody = false;
			yield return new WaitForSeconds(interval);
			canLogUntrackedBody = true;
		}

        /// <summary>
        /// Adds a new task to the <see cref="Tasks"/> list.
        /// </summary>
        /// <param name="task">The task to add.</param>
        public void AddTask(KinectService task)
        {
            Tasks.Add(task);
        }
	}

}
