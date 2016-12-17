using System;
using System.Collections.Generic;
using UnityEngine;

public enum HeroId
{
    None = 0,
    Warrior,
    Mage,
}

public class HeroDatabase
{
    private static HeroDatabase instance;

    public static HeroDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HeroDatabase();
            }

            return instance;
        }
    }

    List<HeroBaseData> heroData;

    public void InitializeHeroDatabase()
    {
        heroData = new List<HeroBaseData>();

        AddBaseData(new HeroBaseData((int)HeroId.None, "None"));
        AddBaseData(new HeroBaseData((int)HeroId.Warrior, "Warrior"));
        AddBaseData(new HeroBaseData((int)HeroId.Mage, "Mage"));

        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(1, 10, 0, 150, 100, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(2, 7, 1, 220, 130, 1, 1, 7, 250));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(3, 9, 2, 300, 170, 2, 1, 7, 450));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(4, 12, 3, 390, 220, 2, 1, 7, 750));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(5, 15, 5, 490, 280, 3, 2, 7, 1250));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(6, 18, 7, 600, 350, 3, 2, 7, 2000));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(7, 21, 9, 720, 430, 4, 2, 7, 3000));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(8, 25, 11, 850, 520, 4, 2, 7, 4500));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(9, 29, 13, 990, 620, 5, 3, 7, 7000));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(10, 33, 15, 1240, 730, 5, 3, 7, 14000));

        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(1, 12, 0, 120, 150, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(2, 15, 0, 170, 190, 1, 1, 7, 250));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(3, 19, 0, 230, 240, 1, 2, 7, 450));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(4, 24, 0, 300, 300, 1, 2, 7, 750));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(5, 30, 1, 380, 370, 1, 3, 7, 1250));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(6, 37, 2, 470, 450, 1, 3, 7, 2000));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(7, 45, 3, 570, 540, 2, 4, 7, 3000));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(8, 54, 4, 680, 640, 2, 4, 7, 4500));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(9, 64, 5, 800, 750, 2, 5, 7, 7000));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(10, 75, 6, 930, 870, 2, 6, 7, 14000));
    }

    public bool AddBaseData(HeroBaseData newHeroData)
    {
        try
        {
            heroData.Add(newHeroData);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("HeroDatabase::AddBaseData 에러 - " + e.Message);
            return false;
        }

    }

    public HeroBaseData GetBaseData(int Id)
    {
        foreach (HeroBaseData baseData in heroData)
        {
            if (baseData.Id == Id)
            {
                return baseData;
            }
        }

        return null;
    }
}

public class HeroBaseData
{
    int id;
    string name;
    List<HeroLevelData> heroLevelData;

    public int Id { get { return id; } }
    public string Name { get { return name; } }
    public List<HeroLevelData> HeroLevelData { get { return heroLevelData; } }

    public HeroBaseData()
    {
        id = 0;
        name = "";
        heroLevelData = new List<HeroLevelData>();
    }

    public HeroBaseData(int _id, string _name)
    {
        id = _id;
        name = _name;
        heroLevelData = new List<HeroLevelData>();
    }

    public HeroLevelData GetLevelDataData(int level)
    {
        for (int index = 0; index < heroLevelData.Count; index++)
        {
            if (heroLevelData[index].Level == level)
            {
                return heroLevelData[index];
            }
        }

        return null;
    }

    public bool AddLevelData(HeroLevelData newHeroLevelData)
    {
        try
        {
            heroLevelData.Add(newHeroLevelData);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class HeroLevelData
{
    int level;
    int attack;
    int defense;
    int healthPoint;
    int magicPoint;
    int hpRegeneration;
    int mpRegeneration;
    int moveSpeed;
    int maxExp;

    public int Level { get { return level; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int HealthPoint { get { return healthPoint; } }
    public int MagicPoint { get { return magicPoint; } }
    public int HpRegeneration { get { return hpRegeneration; } }
    public int MpRegeneration { get { return mpRegeneration; } }
    public int MoveSpeed { get { return moveSpeed; } }
    public int MaxExp { get { return maxExp; } }

    public HeroLevelData()
    {
        level = 1;
        attack = 0;
        defense = 0;
        healthPoint = 0;
        magicPoint = 0;
        hpRegeneration = 0;
        mpRegeneration = 0;
        moveSpeed = 1;
        maxExp = 0;
    }

    public HeroLevelData(int newLevel, int newAttack, int newDefense, int newHealthPoint, int newMagicPoint, int newHpRegeneration, int newMpRegeneration, int newMoveSpeed, int newMaxExp)
    {
        level = newLevel;
        attack = newAttack;
        defense = newDefense;
        healthPoint = newHealthPoint;
        magicPoint = newMagicPoint;
        hpRegeneration = newHpRegeneration;
        mpRegeneration = newMpRegeneration;
        moveSpeed = newMoveSpeed;
        maxExp = newMaxExp;
    }
}


