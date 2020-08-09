using System.Collections;
using System.Collections.Generic;
using NunclearGame.Battle;
using UnityEditor;
using UnityEngine;

namespace NunclearGame.EditorHelpers
{
    [CustomEditor(typeof(BotAttackController))]
    public class AIBotEditorHelper : Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            
            BotAttackController botController = target as BotAttackController;;
            if (botController == null)
                return;
            
            GUILayout.Space(5f);
            
            if (GUILayout.Button("Make unit USELESS"))
            {
                var gameUnit = botController.GetComponent<GameUnit>();
                if (gameUnit != null)
                {
                    gameUnit.IsInvulnerability = true;
                }

                var unitDamage = botController.GetComponent<UnitDamage>();
                if (unitDamage == null)
                {
                    unitDamage = botController.GetComponentInChildren<UnitDamage>();
                    if (unitDamage != null)
                    {
                        unitDamage.DebugMultipler = 0;
                    }
                }
            }
            
            GUILayout.Space(5f);
        }
    }
}

