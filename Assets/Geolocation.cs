using UnityEngine;
using System.Collections;

public class Geolocation : MonoBehaviour
{

	public Font fontstyle;

	public OmniController SoundController;
	public double latitude;
	public double longitude;
	public int altitude = 0;
	public int timestamp = 0;
	//public double lat_1 = 40.693158;
	//public double long_1 = -73.985269;
	public string failed = "failed";

	public float scale = 1.0f;

	public double dis1 = 1000;
	public double dis2 = 1000;
	public double dis3 = 1000;

	public double old_lon = 0;
	public double old_lat = 0;

	public double dest_lat;
	public double dest_lon;
	public string dest_name;

	public string lat_changed = "no";
	public string lon_changed = "no";

	public int check = 0;

	public GUIStyle myGuiStyle_CORDS;

	public float gpsUpdate = 0.1f;
	public float gpsOther = 1.0f;

	public GUISkin menuSkin;

	public double distance = 1000; //distance to the nearest poi in meters

	public float earth_radius = 6371000; //radius in meters


	private AudioSource bing;
	public AudioClip bing0;

	/*
	private AudioSource library;
	public AudioClip library0;
	private AudioSource courtyard;
	public AudioClip courtyard0;
	private AudioSource fiveguys;
	public AudioClip fiveguys0;
	*/

	string[] bus_stops;
	double[] bus_stops_lat;
	double[] bus_stops_long;
	int current_index = 3;

	int stop_index = Manager.current_index; //tells us which index of the array to check
	int stop_before_index; //

	bool to_pratt = Manager.Pratt; //true = going to pratt, false = going to NYU
	string going_to; //string value to help display which direction we're going

	bool stop_before = false; //have you passed the stop before YOUR stop?
	bool arrived = false; //have you arrived at your stop?
	bool played = false; //did the sound play yet?
	bool clip_changed = false; //did the clip change yet?


	//--------------------------------------SOUND----------------------------------------------
	//-----------------------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------------

	//for the purpose of having two audioclip arrays (one for each route)
	//0deg = to pratt
	//90deg = to NYU

	private AudioSource _0deg;
	private AudioSource _90deg;
	private AudioSource _180deg;
	private AudioSource _270deg;
	
	public AudioClip[] clipIn0;
	public AudioClip[] clipIn90;
	public AudioClip[] clipIn180;
	public AudioClip[] clipIn270;
	//public GameObject myCamera;
	public float vol = 100.0f;

	//------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------
	//-----------------------------------------COMPASS-----------------------------------------

	static float 	xValue;
	static float	yValue;
	static float	zValue;

	IEnumerator Start()
	{
		//compass only
		//AndroidJNI.AttachCurrentThread();

		if (Screen.width > 1000) {
			scale = 1.0f;
		} else if (Screen.width > 700) {
			scale = 0.66f;
		} else if (Screen.width > 400) {
			scale = 0.44f;
		}

		/*
		//------------------------only for 4/17 demo, change names later------------------------------
		bus_stops = new string[10]; //<---- change this number if you want to add more than 10 bus stops
		//bus_stops [0] = "Jay St/Myrtle Plz";
		//bus_stops [1] = "Tillary St/Jay St";
		//bus_stops [2] = "Myrtle Av/Fleet Pl";
		bus_stops [0] = "Dibner Building";
		bus_stops [1] = "Five Guys";
		bus_stops [2] = "Middle of the Park";
		bus_stops [3] = "Nothing";

		//-------------------------------the arrays do hold 10 values---------------------------------
		//-------------------------------the undefined ones are default = 0---------------------------
		
		bus_stops_lat = new double[10];
		//bus_stops_lat [0] = 40.694652f;
		bus_stops_lat [0] = 40.694465;
		bus_stops_lat [1] = 40.693653;
		bus_stops_lat [2] = 40.694044;

		print ("reset!");
		
		
		
		bus_stops_long = new double[10];
		//bus_stops_long [0] = -73.987068f;
		bus_stops_long [0] = -73.985959;
		bus_stops_long [1] = -73.985949;
		bus_stops_long [2] = -73.985654;

		//-------------------------------------------------------------------------------------
		*/

		bus_stops = new string[11];

		if (to_pratt) {
			bus_stops [0] = "Jay St/Myrtle Plz";
			bus_stops [1] = "Tillary St/Jay St";
			bus_stops [2] = "Myrtle Av/Fleet Pl";
			bus_stops [3] = "Myrtle Av/Ashland";
			bus_stops [4] = "Myrtle Av/St Edwards";
			bus_stops [5] = "Myrtle Av/N Portland";
			bus_stops [6] = "Myrtle Av/Carlton";
			bus_stops [7] = "Myrtle Av/Vanderbilt";
			bus_stops [8] = "Myrtle Av/Clinton";
			bus_stops [9] = "Myrtle Av/Washington";
			bus_stops [10] = "Myrtle Av/Steuben";
		}
		else {
			bus_stops [0] = "Myrtle Av/Grand Av";
			bus_stops [1] = "Myrtle Av/Washington";
			bus_stops [2] = "Myrtle Av/Clinton";
			bus_stops [3] = "Myrtle Av/Vanderbilt";
			bus_stops [4] = "Myrtle Av/Carlton";
			bus_stops [5] = "Myrtle Av/N Portland";
			bus_stops [6] = "Myrtle Av/St Edwards";
			bus_stops [7] = "Myrtle Av/Navy St";
			bus_stops [8] = "Myrtle Av/Prince St";
			bus_stops [9] = "Metrotech Underpass";
			bus_stops [10] = "Jay St/Myrtle Plz";
		}
		/*
		bus_stops [12] = "Myrtle Av/Classon";
		bus_stops [13] = "Myrtle Av/Franklin";
		bus_stops [14] = "Myrtle Av/Spencer";
		bus_stops [15] = "Myrtle Av/Walworth";
		bus_stops [16] = "Myrtle Av/Nostrand";
		bus_stops [17] = "Myrtle Av/Marcy";
		bus_stops [18] = "Myrtle Av/Tompkins";
		bus_stops [19] = "Myrtle Av/Throop";
		bus_stops [20] = "Myrtle Av/Marcus Garvey";
		bus_stops [21] = "Myrtle Av/Lewis";
		bus_stops [22] = "Myrtle Av/Broadway";
		bus_stops [23] = "Myrtle Av/Bushwick";
		bus_stops [24] = "Myrtle Av/Evergreen";
		bus_stops [25] = "Myrtle Av/Hart";
		bus_stops [26] = "Myrtle Av/De Kalb";
		bus_stops [27] = "Myrtle Av/Stockholm";
		bus_stops [28] = "Myrtle Av/Wilson";
		bus_stops [29] = "Myrtle Av/Knickerbocker";
		bus_stops [30] = "Myrtle Av/Menahan";
		bus_stops [31] = "Gates Av/Wyckoff";
		bus_stops [32] = "Palmetto St/St Nicholas";
		*/

		
		bus_stops_lat = new double[11];
		//bus_stops_lat [0] = 40.694652f;

		if (to_pratt) {
			bus_stops_lat [0] = 40.694046;
			bus_stops_lat [1] = 40.695960;
			bus_stops_lat [2] = 40.693471;
			bus_stops_lat [3] = 40.693423;
			bus_stops_lat [4] = 40.693359;
			bus_stops_lat [5] = 40.693293;
			bus_stops_lat [6] = 40.693209;
			bus_stops_lat [7] = 40.693044;
			bus_stops_lat [8] = 40.693099;
			bus_stops_lat [9] = 40.693325;
			bus_stops_lat [10] = 40.693760;
		}
		else {
			bus_stops_lat [0] = 40.693672;//
			bus_stops_lat [1] = 40.693355;//
			bus_stops_lat [2] = 40.693180;//
			bus_stops_lat [3] = 40.693233;//
			bus_stops_lat [4] = 40.693306;//
			bus_stops_lat [5] = 40.693446;//
			bus_stops_lat [6] = 40.693519;//
			bus_stops_lat [7] = 40.693629;//
			bus_stops_lat [8] = 40.693741;//
			bus_stops_lat [9] = 40.693186;//
			bus_stops_lat [10] = 40.694046;
		}
		
		
		bus_stops_long = new double[11];
		//bus_stops_long [0] = -73.987068f;

		if (to_pratt) {
			bus_stops_long [0] = -73.987052;
			bus_stops_long [1] = -73.986783;
			bus_stops_long [2] = -73.981501;
			bus_stops_long [3] = -73.979332;
			bus_stops_long [4] = -73.977370;
			bus_stops_long [5] = -73.975394;
			bus_stops_long [6] = -73.972460;
			bus_stops_long [7] = -73.969142;
			bus_stops_long [8] = -73.968364;
			bus_stops_long [9] = -73.966464;
			bus_stops_long [10] = -73.962754;
		}
		else {
			bus_stops_long [0] = -73.964504;//
			bus_stops_long [1] = -73.967315;//
			bus_stops_long [2] = -73.969241;//
			bus_stops_long [3] = -73.970757;//
			bus_stops_long [4] = -73.972750;//
			bus_stops_long [5] = -73.976317;//
			bus_stops_long [6] = -73.978264;//
			bus_stops_long [7] = -73.980946;//
			bus_stops_long [8] = -73.982709;//
			bus_stops_long [9] = -73.986400;//
			bus_stops_long [10] = -73.987052;
		}

		if (to_pratt) {
				going_to = "Pratt";
		} else {
				going_to = "NYU";
		}

		//---------------------------------sound-----------------------------------------
		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------

		vol = vol/100.0f;
		
		_0deg = new AudioSource ();
		_90deg = new AudioSource ();
		_180deg = new AudioSource ();
		_270deg = new AudioSource ();
		
		_0deg = gameObject.AddComponent("AudioSource") as AudioSource;
		_90deg = gameObject.AddComponent("AudioSource") as AudioSource;
		_180deg = gameObject.AddComponent("AudioSource") as AudioSource;
		_270deg = gameObject.AddComponent("AudioSource") as AudioSource;


		

		
		_0deg.volume = 100.0f;
		_90deg.volume = 100.0f;
		_180deg.volume = 0.0f;
		_270deg.volume = 0.0f;
		/*
		_0deg.panLevel = 0.0f;
		_90deg.panLevel = 0.0f;
		_180deg.panLevel = 0.0f;
		_270deg.panLevel = 0.0f;
		*/

		//-----------------------------------------------------------------------------------------

		//below is the code figuring out which direction you are going, and which stop comes before
		//your true destination. where the number is 2, we mean the actual last index of the arrays.

		//this is to make sure that an error does not crash the app, and that the app doesn't get
		//stuck waiting for you to get close to [lat 0, long 0].

		if (stop_index != 0) {

			stop_before_index = stop_index - 1;

		} else {
			stop_before_index = stop_index;
		}

		dest_lat = bus_stops_lat [stop_before_index];
		dest_lon = bus_stops_long [stop_before_index];
		dest_name = bus_stops [stop_before_index];

		//----------------------------------sound------------------------------------------
		//---------------------------------------------------------------------------------
		//---------------------------------------------------------------------------------
		//---------------------------------------------------------------------------------

		_0deg.clip = clipIn0[stop_before_index];
		_90deg.clip = clipIn90[stop_before_index];
		//_180deg.clip = clipIn180[stop_before_index];
		//_270deg.clip = clipIn270[stop_before_index];

		//----------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------


		bing = new AudioSource ();
		bing = gameObject.AddComponent("AudioSource") as AudioSource;
		bing.clip = bing0;
		//bing.loop = true;

		/*
		library = new AudioSource ();
		library = gameObject.AddComponent("AudioSource") as AudioSource;
		library.clip = library0;
		//library.loop = true;

		courtyard = new AudioSource ();
		courtyard = gameObject.AddComponent("AudioSource") as AudioSource;
		courtyard.clip = courtyard0;
		//courtyard.loop = true;

		fiveguys = new AudioSource ();
		fiveguys = gameObject.AddComponent("AudioSource") as AudioSource;
		fiveguys.clip = fiveguys0;
		//fiveguys.loop = true;
		*/


		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;

		
		// Start service before querying location
		Input.location.Start(gpsOther, gpsUpdate);
		
		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		
		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}
		
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			failed = "failed";
			yield break;
			check++;
		}
		else
		{
			if (latitude != Input.location.lastData.latitude) {
				lat_changed = "yes";
				old_lat = latitude;
				latitude = Input.location.lastData.latitude;
			}
			else {
				lat_changed = "no";
			}

			if (longitude != Input.location.lastData.longitude) {
				lon_changed = "yes";
				old_lon = longitude;
				longitude = Input.location.lastData.longitude;
			}
			else {
				lon_changed = "no";
			}

			failed = "succeeded";

			//check each POI
			//dis1 = DistanceCalculation (latitude, longitude, lat_1, long_1);

			/* -----------------------------UNCOMMENT LATER-----------------------
			if (dis1 < 11) {
				distance = dis1;
				//omni.sound (true);
				bing.Play ();
			}
			else {
				distance = 1000;
				//omni.sound (false);
				bing.Stop ();
			}
			------------------------------------------------------------------------*/

			check++;
			// Access granted and location value could be retrieved
			print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		}
		
		// Stop service if there is no need to query location updates continuously
		//Input.location.Stop();
	}

	void OnGUI() {
		//GUI.skin.label.fontSize = (int)(70 * scale);

		GUI.Label (new Rect (5, 50, Screen.width, 50), "check #: " + check, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 110, 100, 50), "Latitude: " + Input.location.lastData.latitude, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 170, 100, 50), "Longitude: " + Input.location.lastData.longitude, myGuiStyle_CORDS);
		//if (distance < 1000) {
		GUI.Label (new Rect (5, 230, 100, 50), "Destination: " + dest_name, myGuiStyle_CORDS);
		//}
		GUI.Label (new Rect (5, 290, 100, 50), "Status: " + failed, myGuiStyle_CORDS);

		GUI.Label (new Rect (5, 350, 100, 50), "lat change: " + lat_changed, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 410, 100, 50), "lon change: " + lon_changed, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 470, 100, 50), "old lat: " + old_lat, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 530, 100, 50), "old lon: " + old_lon, myGuiStyle_CORDS);

		GUI.Label (new Rect (5, 590, 100, 50), "dest lat: " + dest_lat, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 650, 100, 50), "dest lon: " + dest_lon, myGuiStyle_CORDS);
		//GUI.Label (new Rect (5, 590, 100, 50), "closest to: " + bus_stops[current_index], myGuiStyle_CORDS);

		GUI.Label (new Rect (5, 710, 100, 50), "Distance: " + distance, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 770, 100, 50), "Going towards: " + going_to, myGuiStyle_CORDS);
		//GUI.Label (new Rect (5, 770, 100, 50), "Distance3: " + dis3, myGuiStyle_CORDS);
		GUI.Label (new Rect (5, 830, 100, 50), "yvalue: " + yValue, myGuiStyle_CORDS);

		if (arrived) {
			GUI.Label (new Rect (5, 900, 100, 50), "You've arrived!", myGuiStyle_CORDS);
		}

		GUI.skin = menuSkin;
		if(GUI.Button(new Rect(Screen.width - (scale * 400), Screen.height - (scale * 200), (scale * 300), (scale * 100)), "Back")){
			Application.LoadLevel("menu");
		}
		if(GUI.Button(new Rect(Screen.width - (scale * 700),Screen.height - (scale * 200), (scale * 300), (scale * 100)), "play")){
			sound (true, to_pratt);
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			//omni.GetComponent<OmniController>().sound (true);
			//bing.Play ();
			//dis1 = DistanceCalculation (old_lat, old_lon, lat_1, long_1);
			//distance = dis1;

			/*
			if (!library.isPlaying) {
				library.Play ();
			}
			*/
			sound (true, to_pratt);


		}




		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			failed = "failed";
			check++;
		}
		else
		{
			if (latitude != Input.location.lastData.latitude) {
				lat_changed = "yes";
				old_lat = latitude;
				latitude = Input.location.lastData.latitude;
			}
			else {
				lat_changed = "no";
			}
			
			if (longitude != Input.location.lastData.longitude) {
				lon_changed = "yes";
				old_lon = longitude;
				longitude = Input.location.lastData.longitude;
			}
			else {
				lon_changed = "no";
			}
			
			failed = "succeeded";
			
			//check each POI
			if ((latitude != null) && (longitude != null)) {
				distance = DistanceCalculation (latitude, longitude, dest_lat, dest_lon);
			}
			//dis2 = DistanceCalculation (latitude, longitude, bus_stops_lat [1], bus_stops_long [1]);
			//dis3 = DistanceCalculation (latitude, longitude, bus_stops_lat [2], bus_stops_long [2]);

			
			if (distance < 36) {
				print ("passed: " + dest_name);
				//distance = dis1;
				//omni.sound (true);
				/*
				if (dest_name == bus_stops[0]) {
					if (!library.isPlaying) {
						library.Play ();
					}
				}
				else if (dest_name == bus_stops[1]) {
					if (!fiveguys.isPlaying) {
						fiveguys.Play();
					}
				}
				else {
					if (!courtyard.isPlaying) {
						courtyard.Play();
					}
				}*/



				//current_index = 0;
				if (stop_before) {
					arrived = true;
					sound (true, to_pratt);
					stop_before = false;
				}
				else if (!stop_before && !arrived) {

					dest_lat = bus_stops_lat [stop_index];
					dest_lon = bus_stops_long [stop_index];
					dest_name = bus_stops [stop_index];


					sound (true, to_pratt);
					if (_0deg.isPlaying || _90deg.isPlaying) {
						played = true;
					}

					//_180deg.clip = clipIn180[stop_index];
					//_270deg.clip = clipIn270[stop_index];


					if ((latitude != null) && (longitude != null)) {
						distance = DistanceCalculation (latitude, longitude, dest_lat, dest_lon);
					}
					stop_before = true;

					print ("updated destination to: " + dest_name);

				}

			}

			if (played && !clip_changed) {
				if (!_0deg.isPlaying && !_90deg.isPlaying) {
					_0deg.clip = clipIn0[stop_index];
					_90deg.clip = clipIn90[stop_index];
					clip_changed = true;
				}

			}

			/*
			if (dis2 < 11) {
				//distance = dis2;
				if (!fiveguys.isPlaying) {
					fiveguys.Play();
				}
				current_index = 1;
			}
			if (dis3 < 11) {
				//distance = dis3;
				if (!courtyard.isPlaying) {
					courtyard.Play();
				}
				current_index = 2;
			}
			else {
				//omni.sound (false);
				//bing.Stop ();

				current_index = 3;
			}
			*/

			
			check++;
			// Access granted and location value could be retrieved
			print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		}

	}


	double DistanceCalculation(double lat, double lon, double lat_poi, double lon_poi) {


				double lat_radians1 = latitude * Mathf.Deg2Rad;
				double lat_radians2 = lat_poi * Mathf.Deg2Rad;
				//float lon_radians1 = longitude * Mathf.Deg2Rad;
				//float lon_radians2 = lon * Mathf.Deg2Rad;
				double change_lat = (lat - lat_poi) * Mathf.Deg2Rad;
				double change_lon = (lon - lon_poi) * Mathf.Deg2Rad;

				double a = (System.Math.Sin (change_lat / 2) * System.Math.Sin (change_lon / 2)) +
						(System.Math.Cos (lat_radians1) * System.Math.Cos (lat_radians2) *
						System.Math.Sin (change_lon / 2) * System.Math.Sin (change_lon / 2));
				double c = 2 * System.Math.Atan2 (System.Math.Sqrt (a), System.Math.Sqrt (1 - a));

				double d = earth_radius * c;

				print (d);
				return d;

		}
	/*
	public void play_recording() {
		bing.Play ();
		StartCoroutine(sound(true, to_pratt));
	}*/

	public void sound(bool play, bool pratt) {
		//yield return new WaitForSeconds(3f);

		if ((play) && (pratt)) {
			_0deg.Play ();
			//_90deg.Play ();
			//_180deg.Play ();
			//_270deg.Play ();
		} 
		else if ((play) && (!pratt)) {
			_90deg.Play();
		}
		/*
		else {
			_0deg.Stop ();
			_90deg.Stop ();
			//_180deg.Stop ();
			//_270deg.Stop ();
		}
		*/
	}

	/*
	void FixedUpdate () {
		float azimuth = yValue;
		float azimuthRad = azimuth * Mathf.Deg2Rad;
		
		if (azimuth <= 90.0f) 
		{
			_0deg.volume = Mathf.Cos (azimuthRad) * vol;
			_90deg.volume = Mathf.Sin (azimuthRad) * vol;
			_180deg.volume = 0.0f;
			_270deg.volume = 0.0f;
		}
		else if (azimuth > 90.0f && azimuth <= 180.0f) 
		{
			_0deg.volume = 0.0f;
			_90deg.volume = Mathf.Cos (azimuthRad - Mathf.PI/2.0f) * vol;
			_180deg.volume = Mathf.Sin (azimuthRad - Mathf.PI/2.0f) * vol;
			_270deg.volume = 0.0f;
		} 
		else if (azimuth > 180.0f && azimuth <= 270.0f) 
		{
			_0deg.volume = 0.0f;
			_90deg.volume = 0.0f;
			_180deg.volume = Mathf.Cos (azimuthRad - Mathf.PI) * vol;
			_270deg.volume = Mathf.Sin (azimuthRad - Mathf.PI) * vol;
		}
		else if (azimuth > 270.0f && azimuth <= 360.0f) 
		{
			_0deg.volume = Mathf.Sin (azimuthRad - Mathf.PI - Mathf.PI/2.0f) * vol;
			_90deg.volume = 0.0f;
			_180deg.volume = 0.0f;
			_270deg.volume = Mathf.Cos (azimuthRad - Mathf.PI - Mathf.PI/2.0f) * vol;
		}
	}
	*/



}