using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private  int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    // [SerializeField]
    // private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField]
    private AudioSource source;

    // private GridData floorData, furnitureData;

    // [SerializeField]
    // private PreviewSystem preview;

    // private Vector3Int lastDetectedPosition = Vector3Int.zero;

    // [SerializeField]
    // private ObjectPlacer objectPlacer;

    // IBuildingState buildingState;

    // [SerializeField]
    // private SoundFeedback soundFeedback;

    private void Start()
    {
        StopPlacement();
        // gridVisualization.SetActive(false);
    //     floorData = new();
    //     furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0){
            Debug.Log($"No object with ID {ID}");
            return;
        }
            
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
    //     buildingState = new PlacementState(ID,
    //                                        grid,
    //                                        preview,
    //                                        database,
    //                                        floorData,
    //                                        furnitureData,
    //                                        objectPlacer,
    //                                        soundFeedback);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    // public void StartRemoving()
    // {
    //     StopPlacement();
    //     gridVisualization.SetActive(true) ;
    //     buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer, soundFeedback);
    //     inputManager.OnClicked += PlaceStructure;
    //     inputManager.OnExit += StopPlacement;
    // }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        source.Play();
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        // buildingState.OnAction(gridPosition);

    }

    // //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    // //{
    // //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
    // //        floorData : 
    // //        furnitureData;

    // //    return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    // //}

    private void StopPlacement()
    {
        // soundFeedback.PlaySound(SoundType.Click);
        // if (buildingState == null)
        //     return;
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        // buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        // lastDetectedPosition = Vector3Int.zero;
        // buildingState = null;
    }

    private void Update()
    {
        // if (buildingState == null)
        //     return;
        if (selectedObjectIndex < 0)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        Vector3 cellSize = grid.cellSize;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition) + new Vector3(cellSize.x / 2f, 0.01f, cellSize.z / 2f);

        // if(lastDetectedPosition != gridPosition)
        // {
        //     buildingState.UpdateState(gridPosition);
        //     lastDetectedPosition = gridPosition;
        // }
        
    }
}
