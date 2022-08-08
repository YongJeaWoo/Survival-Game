using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossState
{
    void Init(Boss owner);
    void ExcuteEnter();
    void ExcuteExit();
}

