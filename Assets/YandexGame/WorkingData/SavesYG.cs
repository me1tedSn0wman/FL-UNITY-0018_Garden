
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int diamonds = 100;                       // Можно задать полям значения по умолчанию
        public int highScore = 0;

        public List<SavedUpgrades> upgradeSaveData = new List<SavedUpgrades>();
        public void SetSavedUpgradeLevel(string uid, int level) {
            for (int i = 0; i < upgradeSaveData.Count; i++) {
                if (uid == upgradeSaveData[i].uid) {
                    upgradeSaveData[i].level = level;
                    return;
                }
            }

            SavedUpgrades temp =new SavedUpgrades();
            temp.uid = uid;
            temp.level = level;
            upgradeSaveData.Add(temp);
        }


        // Ваши сохранения

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений

#if PLATFORM_WEBGL
        public SavesYG()
        {

        }
#endif

        [System.Serializable]
        public class SavedUpgrades {
            public string uid;
            public int level;
        }
    }
}
