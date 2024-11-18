using System.Runtime.Serialization.Formatters.Binary;
#if PLATFORM_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
#endif
using UnityEngine;
using System;
using System.IO;
using Utils;
using Newtonsoft.Json;

namespace GPControl
{
    public class GPCloudSaveManager : Soliton<GPCloudSaveManager>
    {

        public static GPSaveData gpSaveData { get; private set; }
#if PLATFORM_ANDROID
        [SerializeField] private DataSource _dataSource;
        [SerializeField] private ConflictResolutionStrategy _conflict;
#endif
        private string saveName = "save_0";

        private BinaryFormatter _formatter;

        public Action OnLoadDataGP;
        public bool dataIsLoaded = false;

        public bool isOnlyLocal = false;

        [SerializeField] private string filePathLocal = "/saves.json";

        [Header("Set Dynamically")]
        [SerializeField] private string fullFilePath;



#if PLATFORM_ANDROID
        public override void Awake()
        {
            base.Awake();
            fullFilePath = Application.persistentDataPath + filePathLocal;
            _formatter = new BinaryFormatter();
            gpSaveData = new GPSaveData();
        }

        public void Load()
        {
            OnLoadDataGP?.Invoke();
            dataIsLoaded = true;
        }

        public static void SaveProgress()
        {
            Instance.SaveLocal();
            if (!Instance.isOnlyLocal)
            {
                Instance.SaveToCloud();
            }
        }

        public void ApplyCloudData(GPSaveData data, bool dataExists)
        {
            if (!dataExists || data == null)
            {
                LoadLocal();
                return;
            }
            gpSaveData = data;

            OnLoadDataGP?.Invoke();
            dataIsLoaded = true;

        }

        private void SaveToCloud()
        {
            OpenCloudSave(OnSaveResponse);
        }

        private void OnSaveResponse(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                var rawData = gpSaveData;

                var data = SerializeSaveData(rawData);
                if (data == null)
                {
                    return;
                }

                var update = new SavedGameMetadataUpdate.Builder().Build();
                //                GPAuthentication.Platform.SavedGame.CommitUpdate(metadata, update, data, SaveCallback);

            }
            else
            {
                Debug.Log("On Save Response erroe");
            }
        }

        private void SaveCallback(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            if (status == SavedGameRequestStatus.Success)
                Debug.Log("Data saved succesfully");
            else
                Debug.Log("Data is not saved succs");
        }


        private void LoadFromCloud()
        {
            OpenCloudSave(OnLoadResponse);
        }

        private void OnLoadResponse(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                //                GPAuthentication.Platform.SavedGame.ReadBinaryData(metadata, LoadCallback);
            }
            else LoadLocal();
        }

        private void LoadCallback(SavedGameRequestStatus status, byte[] data)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                ApplyCloudData(DeserializeSaveData(data), data.Length > 0);
            }
            else LoadLocal();
        }

        private void OpenCloudSave(Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
        {
            if (!Social.localUser.authenticated
                || !PlayGamesClientConfiguration.DefaultConfiguration.EnableSavedGames
                || string.IsNullOrEmpty(saveName)
                )
                Debug.Log("OPenCloudSaveError");

            //            GPAuthentication.Platform.SavedGame.OpenWithAutomaticConflictResolution(saveName, _dataSource, _conflict, callback);
        }

        private byte[] SerializeSaveData(GPSaveData data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    _formatter.Serialize(ms, data);
                    return ms.GetBuffer();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }

        private GPSaveData DeserializeSaveData(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    return (GPSaveData)_formatter.Deserialize(ms);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }

        private void SaveLocal()
        {
            StreamWriter writer = new StreamWriter(fullFilePath, false);
            string saves = JsonConvert.SerializeObject(gpSaveData);

            Debug.Log("Save " + fullFilePath + "___" + saves);
            writer.WriteLine(saves);
            writer.Close();
        }

        public void LoadLocal()
        {
            if (File.Exists(fullFilePath))
            {

                StreamReader reader = new StreamReader(fullFilePath);
                string saves = reader.ReadToEnd();
                Debug.Log("Load " + fullFilePath + "___" + saves);
                gpSaveData = JsonConvert.DeserializeObject<GPSaveData>(saves);
                reader.Close();
            }
            OnLoadDataGP?.Invoke();
            dataIsLoaded = true;
        }

        public static void RemoveSaves()
        {
            if (File.Exists(Instance.fullFilePath))
            {
                File.Delete(Instance.fullFilePath);
            }
            gpSaveData = new GPSaveData();
        }
#endif
    }
}
