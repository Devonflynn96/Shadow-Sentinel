using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData
{
    //Interface methods
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
