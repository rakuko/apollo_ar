﻿using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	string mainMenuSceneName;
	Font pauseMenuFont;
	private bool pauseEnabled = false;
	int stop_index = 0;

	int address_list_size;
	string[] bus_stops;
	bool to_pratt = true;


	public GUIStyle MenuStyle;
	public GUISkin MenuSkin;

	public GUISkin DestinationOn;

	public GUISkin stopSkin;
	public GUISkin submitSkin;

	public Vector2 scrollPosition = Vector2.zero;

	public bool mainMenu = true;
	public bool streetMenu = false;

	public GameObject mainGrey;
	public GameObject stopGrey;
	public GameObject stopBanner;

	public int streetMenu_index = 0;

	/*
	private bool showList = false;
	private int listEntry = 0;
	private GUIContent[] list;
	private GUIStyle listStyle;
	private bool picked = false;


	// Use this for initialization
	void Start () {
		list = new GUIContent[3];
		list [0] = new GUIContent("Jay St/Myrtle Plz");
		list [1] = new GUIContent("Tillary St/Jay St");
		list [2] = new GUIContent("Myrtle Av/Fleet Pl");
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (Popup.List (Rect (500, 800, 500, 800), showList, listEntry, new GUIContent("Destination"), list, listStyle)) {
			picked = true;
		}
	}*/



	void Start(){

		bus_stops = new string[12]; //<---- change this number if you want to add more than 10 bus stops
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
		bus_stops [10] = "Myrtle Av/Ryerson";
		bus_stops [11] = "Myrtle Av/Steuben";



		pauseEnabled = false;
		Time.timeScale = 1;
		AudioListener.volume = 1;
		Screen.showCursor = true;
	
	}
	
	void Update(){
	}
	
	private bool showGraphicsDropDown = false;
	
	void OnGUI(){
		
		//GUI.skin.box.font = pauseMenuFont;


		if (mainMenu) {
			GUI.skin = MenuSkin;
			if (GUI.Button (new Rect (50, 350, Screen.width - 100, 100), "Destination")) {

				mainMenu = false;
				mainGrey.SetActive(false);
				streetMenu = true;
				stopGrey.SetActive(true);
				stopBanner.SetActive(true);

			}
			GUI.skin = MenuSkin;
			if (!to_pratt) {
				GUI.skin = DestinationOn;
			}

			if (GUI.Button (new Rect (50, Screen.height/2, 300, 100), "To NYU")) {
				to_pratt = false;
			}
			GUI.skin = MenuSkin;
			if (to_pratt) {
				GUI.skin = DestinationOn;
			}

			if (GUI.Button (new Rect (Screen.width - 350, Screen.height/2, 300, 100), "To Pratt")) {
				to_pratt = true;
			}

			GUI.skin = submitSkin;

			if (GUI.Button (new Rect (Screen.width - 146, Screen.height - 146, 96, 96), "")) {
				Manager.current_index = stop_index;
				Manager.Pratt = to_pratt;
				Application.LoadLevel ("map");
			}
		}
		//Create the Graphics settings buttons, these won't show automatically, they will be called when
		//the user clicks on the "Change Graphics Quality" Button, and then dissapear when they click
		//on it again....


		if (streetMenu) {
			//scrollPosition = GUI.BeginScrollView(new Rect(0,Screen.height/2,Screen.width,Screen.height - 400), scrollPosition, new Rect(0,0,800,1200));
			/*
			if(GUI.Button(new Rect(350,50 ,800, 100), bus_stops [0])){
				stop_index = 0;
			}
			if(GUI.Button(new Rect(350,150,800, 100), bus_stops [1])){
				stop_index = 1;
			}
			if(GUI.Button(new Rect(350,250,800, 100), bus_stops [2])){
				stop_index = 2;
			}
			*/
			GUI.skin = stopSkin;
			if(GUI.Button(new Rect(50, Screen.height/2, 100, 100), "^")){
				if (streetMenu_index > 0) {
					streetMenu_index--;
				}
			}
			if(GUI.Button(new Rect(50, Screen.height - 146, 100, 100), "v")){
				if (streetMenu_index+6 < 11) {
					streetMenu_index++;
				}
			}

			if(GUI.Button(new Rect(100, Screen.height/2, Screen.width - 100, 100), bus_stops [streetMenu_index])){
				stop_index = streetMenu_index;
			}
			if(GUI.Button(new Rect(100, Screen.height/2 + 100, Screen.width - 100, 100), bus_stops [streetMenu_index + 1])){
				stop_index = streetMenu_index + 1;
			}
			if(GUI.Button(new Rect(100, Screen.height/2 + 200, Screen.width - 100, 100), bus_stops [streetMenu_index + 2])){
				stop_index = streetMenu_index + 2;
			}
			if(GUI.Button(new Rect(100, Screen.height/2 + 300, Screen.width - 100, 100), bus_stops [streetMenu_index + 3])){
				stop_index = streetMenu_index + 3;
			}
			if(GUI.Button(new Rect(100, Screen.height/2 + 400, Screen.width - 100, 100), bus_stops [streetMenu_index + 4])){
				stop_index = streetMenu_index + 4;
			}
			if(GUI.Button(new Rect(100, Screen.height/2 + 500, Screen.width - 100, 100), bus_stops [streetMenu_index + 5])){
				stop_index = streetMenu_index + 5;
			}
			if(GUI.Button(new Rect(100, Screen.height/2 + 600, Screen.width - 100, 100), bus_stops [streetMenu_index + 6])){
				stop_index = streetMenu_index + 6;
			}

			//to avoid coding touch scrolling which would take longer to work with scrollView
			//i'm simply going to update the buttons everytime you press up arrow or down arrow
			//buttons on the screen.


			/*
			if(GUI.Button(new Rect(0, 700, Screen.width, 100), bus_stops [1])){
				stop_index = 7;
			}
			if(GUI.Button(new Rect(0, 800, Screen.width, 100), bus_stops [2])){
				stop_index = 8;
			}
			if(GUI.Button(new Rect(0, 900, Screen.width, 100), bus_stops [0])){
				stop_index = 9;
			}
			if(GUI.Button(new Rect(0, 1000, Screen.width, 100), bus_stops [1])){
				stop_index = 10;
			}
			if(GUI.Button(new Rect(0, 1100, Screen.width, 100), bus_stops [2])){
				stop_index = 11;
			}
			*/
			//GUI.EndScrollView();

			GUI.skin = submitSkin;
			
			if (GUI.Button (new Rect (Screen.width - 146, Screen.height - 146, 96, 96), "")) {

				streetMenu = false;
				stopGrey.SetActive(false);
				stopBanner.SetActive(false);
				mainMenu = true;
				mainGrey.SetActive(true);

			}
		}

		
			
	}

}
