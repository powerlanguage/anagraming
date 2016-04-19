﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RackScript : MonoBehaviour {

	public float slotWidth;
	public float spacerWidth;
	public GameObject slotPrefab;
	public GameObject[] slots;
	public GameObject otherRack;
	private RackScript otherRackScript;



	//Worth storing SlotScript references in array?

	void Start(){
		otherRackScript = otherRack.GetComponent<RackScript> ();
	}

	private GameObject GetFirstEmptySlot(){
		foreach (GameObject slot in slots) {
			if(!slot.GetComponent<SlotScript>().isOccupied){
				return slot;
			}
		}
		// No empty slots
		return null;
	}

	//Add a tile to the first empty slot
	public void AddTileToFirstEmptySlot(GameObject tile){
		GameObject emptySlot = GetFirstEmptySlot ();
		if (emptySlot != null) {
			emptySlot.GetComponent<SlotScript>().AddTile(tile);
		}
	}

	//Add a tile to a specific slot
	//Checks if slot is in rack, that slot is empty then adds tile
	public void AddTileToSlot(GameObject tile, GameObject slot){
		if(ContainsSlot(slot)){
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(!slotScript.isOccupied){
				slotScript.AddTile(tile);
			}
		}
	}

	//Looping over array to find element.  V.inefficient.  IndexOf works with Android?
	//Remove tile from a specific slot
	public void RemoveTileFromSlot(GameObject tile, GameObject slot){
		if(ContainsTile(tile) && ContainsSlot(slot)){
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.tile == tile){
				slotScript.ClearSlot();
			}
		}
	}

	public void RemoveTile(GameObject tile){
		if(ContainsTile(tile)){
			GameObject slot = GetSlotContainingTile(tile);
			slot.GetComponent<SlotScript>().ClearSlot();
		}
	}

	//Loop over all slots and to check for tile
	public bool ContainsTile(GameObject tileToFind){
		foreach (GameObject slot in slots) {
			if(slot.GetComponent<SlotScript>().tile == tileToFind){
				return true;
			}
		}
		return false;
	}

	//Check if slot is in this rack
	private bool ContainsSlot(GameObject slotToFind){
		foreach (GameObject slot in slots) {
			if(slot == slotToFind){
				return true;
			}
		}
		return false;
	}
	
	public GameObject GetSlotContainingTile(GameObject tile){
		foreach (GameObject slot in slots) {
			if(slot.GetComponent<SlotScript>().tile == tile){
				return slot;
			}
		}
		return null;
	}

	//Returns null if there is no tile in the slot
	public GameObject GetTileInSlot(GameObject slot){
		return slot.GetComponent<SlotScript>().tile;
	}

	public void shuffleRack(){
		List<GameObject> tilesToShuffle = new List<GameObject> ();
		//Remove tiles from slots and add to a list
		foreach (GameObject slot in slots) {
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.isOccupied){
				tilesToShuffle.Add(slotScript.tile);
				slotScript.ClearSlot();
			}
		}
		//Shuffle the list
		GameObject[] tiles = tilesToShuffle.ToArray();
		for (int i = 0; i < tiles.Length; i++) {
			GameObject temp = tiles[i];
			int randomIndex = Random.Range(i, tiles.Length);
			tiles[i] = tiles[randomIndex];
			tiles[randomIndex] = temp;
		}
		//Add back to the slots
		for (int i = 0; i < tiles.Length; i++) {
			AddTileToFirstEmptySlot (tiles [i]);
		}
	}

	//Loop over slots.  For those that have a tile, add its letter to the string
	public string GetRackString(){
		string rackString = "";
		foreach (GameObject slot in slots) {
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.isOccupied){
				rackString += slotScript.tile.GetComponent<TileScript>().letter;
			}
		}
		return rackString;
	}

	public void ClearRack(){
		foreach (GameObject slot in slots) {
			slot.GetComponent<SlotScript>().ClearSlot();
		}
	}

	public void TileTapped(GameObject tile){
		RemoveTile (tile);
		otherRackScript.AddTileToFirstEmptySlot (tile);
	}

}
