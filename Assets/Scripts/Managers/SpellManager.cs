using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    static SpellManager instance = null;

    public List<Spell> Spells = new List<Spell>();
    bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if(!initialized)
        {
            Debug.Log("Initializing spells:" + (Initialize() ? "SUCCESS" : "FAILED"));
            initialized = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static SpellManager GetInstance()
    {
        return instance;
    }

    public bool Initialize()
    {
        bool result = true;

        // Verify if two spells with the same id does not exist
        for (int i = 0; i < Spells.Count; i++)
        {
            for (int j = i + 1; j < Spells.Count; j++)
            {
                if(Spells[i].Id == Spells[j].Id)
                {
                    Debug.LogWarning("Duplicate spell IDs found: " + Spells[i].Id + " (" + Spells[i].Name + "," + Spells[j].Name + ")");
                    result = false;
                }
            }
        }

        return result;
    }

    public Spell Find(int id)
    {
        for (int i = 0; i < Spells.Count; i++)
        {
            if (Spells[i].Id == id)
            {
                return Spells[i];
            }
        }

        return null;
    }
}
