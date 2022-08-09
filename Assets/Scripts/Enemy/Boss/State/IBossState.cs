using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState
{
    void Init(Boss owner);
    void ExcuteEnter();
    void ExcuteUpdate();
    void ExcuteExit();
}

