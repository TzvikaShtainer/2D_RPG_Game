using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
   public static SaveManager instance;

   [SerializeField] private string fileName;
   [SerializeField] private string FileLoc;

   private GameData gameData;

   private List<ISaveManager> saveManagers;
   private FileDataHandler dataHandler;

   private void Awake()
   {
      if (instance != null)
         Destroy(instance.gameObject);
      else
         instance = this;
   }

   private void Start()
   {
      dataHandler = new FileDataHandler(Application.persistentDataPath ,fileName);
      saveManagers = findAllSaveManagers();
      
      LoadGame();
   }

   public void NewGame()
   {
      gameData = new GameData();
   }

   public void LoadGame()
   {
      gameData = dataHandler.Load();

      if (gameData == null)
      {
         Debug.Log("no data found");
         NewGame();
      }
      
      foreach (ISaveManager saveManager in saveManagers)
      {
         saveManager.LoadData(gameData);
      }
      
      Debug.Log("loaded currency " +gameData.currency);
   }

   public void SaveGame()
   {
      foreach (ISaveManager saveManager in saveManagers)
      {
         saveManager.SaveData(ref gameData);
      }
      
      dataHandler.Save(gameData);
   }
   
   void OnApplicationQuit()
   {
      SaveGame();
   }

   List<ISaveManager> findAllSaveManagers()
   {
      IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
      return new List<ISaveManager>(saveManagers);
   }
}
