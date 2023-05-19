using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerState : NetworkBehaviour {

    [Networked(OnChanged = nameof(OnHPChanged))]
    public byte Hp {get; set;}

    [Networked]
    public bool isDead {get; set;}

    public bool skipSettingStartValues = false;

    private byte maxHp = 100;

    [SerializeField]
    private Image hpBar;

    void Start() {

        if (!skipSettingStartValues) {
            Hp = maxHp;
            isDead = false;
        }
    }
    
    public void TakeDemage (byte demage) {

        if (isDead) {
            return;
        }

        if (Object.HasStateAuthority) {
            Hp -= demage;
        }

        if (Hp <= 0) {
            Debug.LogFormat ("{0} died", transform.name);
            isDead = true;
        }
    }

    static void OnHPChanged (Changed<PlayerState> changed) {

        Debug.LogFormat ("{0} has {1}hp", changed.Behaviour.transform.name, changed.Behaviour.Hp);

        changed.Behaviour.OnHPChanged ();

    }

    private void OnHPChanged() {

        hpBar.fillAmount = (float)Hp / maxHp;

    }
}
