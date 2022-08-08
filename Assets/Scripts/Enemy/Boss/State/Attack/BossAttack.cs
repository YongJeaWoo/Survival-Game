using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossAttack
{
    void AttackEnter();
    void AttackExit();
    void Init(Boss owner);
}
