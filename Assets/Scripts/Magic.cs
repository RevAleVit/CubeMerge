﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour {

    //It's magic script without comments

    [SerializeField] private UnityEngine.UI.Image bImage;
    private UnityEngine.UI.Text bText;
    
    public static int MagicsCount { get; set; }
    private static Color NowInMagic;
    private static int PointsToGetMagic;
    private static int WayTomagic = 3;

    // Use this for initialization
    void Start () {
        MagicsCount = 0;
        PointsToGetMagic = 0;
        NowInMagic = new Color();

        bText = bImage.GetComponentInChildren<UnityEngine.UI.Text>();        
    }

    void Update()
    {
        bImage.fillAmount = (float)(PointsToGetMagic + 1) / (WayTomagic);
        bImage.color = NowInMagic;
        bText.text = MagicsCount.ToString();
    }

    public static void MagicHere(Address3 address)
    {
        if (NowInMagic == ItCube.sides[address.Side].fragments[address.Row, address.Col].GetColor())
        {
            PointsToGetMagic++;
        }
        else
        {
            NowInMagic = ItCube.sides[address.Side].fragments[address.Row, address.Col].GetColor();
            PointsToGetMagic = 0;
        }

        if (PointsToGetMagic >= WayTomagic)
        {
            MagicsCount++;
            PointsToGetMagic = 0;
        }
    }

    public static bool MagicNow(Address3 address)
    {
        if (MagicsCount > 0 && ItCube.sides[address.Side].fragments[address.Row, address.Col].value != 0 && ItCube.sides[address.Side].fragments[address.Row, address.Col].value != 1)
        {
            ItCube.sides[address.Side].fragments[address.Row, address.Col].value = 1;
            MagicsCount--;
            GameManager.AfterMagic(address);
            return true;
        }
        return false;
    }
}
