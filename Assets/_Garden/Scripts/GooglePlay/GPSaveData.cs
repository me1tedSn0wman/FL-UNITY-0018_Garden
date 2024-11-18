using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPControl
{
    [System.Serializable]
    public class GPSaveData
    {
        public List<QuestionSaveData> questionSaveDatas = new List<QuestionSaveData>();

#if PLATFORM_ANDROID
        public GPSaveData()
        {
        }

        public void SetQuestionData(int levelId, int answer) {
            for (int i = 0; i < questionSaveDatas.Count; i++) {
                if (levelId == questionSaveDatas[i].levelId) {
                    questionSaveDatas[i].answer= answer;
                    return;
                }
            }

            QuestionSaveData temp = new QuestionSaveData();
            temp.levelId = levelId;
            temp.answer = answer;
            questionSaveDatas.Add(temp);
        }
#endif
    }

    [System.Serializable]
    public class QuestionSaveData
    {
        public int levelId;
        public int answer;
    }
}
