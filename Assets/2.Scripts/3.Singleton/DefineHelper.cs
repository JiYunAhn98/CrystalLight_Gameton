using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineHelper
{
    #region[Progress]
    public enum eProgress
    {
        Init,
        Ready,
        Play,
        Stop,
        Replay,
        End,
        Result
    }
    public enum eSceneName
    {
        Loading,
        Lobby,
        InGame,
        Store,
        Story,

        Cnt
    }
    #endregion[Progress]


    #region[System Define Name]
    public enum eLevelMode
    {
        Story,
        Infinite,

        Cnt
    }
    public enum eInfiniteLevel
    {
        NORMAL,
        FAST,
        HARD,

        Cnt
    }
    public enum eTutorialLevel
    {
        STORY1,
        STORY2,
        STORY3,

        Cnt
    }
    public enum eMotion
    {
        Select,
        Hit,

        Cnt
    }
    public enum eCharacterState //hitstate로 바꾸기
    {
        Idle,
        Bounce,
        Dead,

        Cnt
    }
    public enum eDebuffState    // eCharacterState로 바꾸기
    {
        OnlyBounce,
        Bounce,
        OnlyDead,
        SawBlade,
        Ice,
        Oil,
        Fire,
        Electronic,
        Super,

        Cnt
    }
    public enum eParticle
    {
        Bounce,
        Crystal,
        Dead,
        DeadImmune,
        Electric,
        Fire,
        Hit,
        Magnet,
        Oil,
        Shield,
        SlowTime,
        Super,

        Cnt
    }
    public enum eItem
    {
        Crystal,
        Shield,
        Magnet,
        SlowTime,

        Cnt
    }
    public enum ePassReward
    {
        Crystal,
        Accessory,
        Face,
        Diamond,

        Cnt
    }
    public enum ePassState
    {
        Lock,
        CanGet,
        AlreadyGet,
        CantGet,

        Cnt
    }
    public enum eMissionType
    {
        PlayGame,
        GetItem,
        NormalScore,
        FirePass,
        ElectronicPass,
        SawBladePass,
        SafeTime,
        WatchAD,
        GoGacha,

        Cnt
    }
    #endregion[System Define Name]


    #region[Resources Name]
    public enum eWall
    {
        None = 0,
        WallSmall = 65,
        WallLarge = 95,

        Cnt
    }
    public enum eDictionaryCategory
    {
        Character,
        Face,
        Accessory,

        Cnt
    }
    public enum eCharacter
    {
        Stone,
        Copper,
        Silver,
        RealGold,
        Emerald,
        Diamond,
        Ruby,
        Sapphire,
        Metal,
        Crystal,

        Cnt
    }
    public enum eLockThings
    {
        STORY2,
        STORY3,
        FastMode,
        HardMode,

        Cnt
    }
    public enum eAccessory
    {
        None,
        ArmyHelmet,
        BallCap,
        Bandana,
        BigHikingBag,
        BunnyHat,
        CaptianHat,
        ColaHelmet,
        Cupcake,
        Earmuffs,
        ElfHat,
        FemaleHair1,
        FemaleHair2,
        FemaleHair3,
        FemaleHair4,
        FemaleHair5,
        FlatTopHat,
        FootballHelmet,
        GasMask,
        Glasses1,
        HunterHat,
        IndianaJonesHat,
        IrishHat,
        LongBeard,
        LongHikingBag,
        MaleHair1,
        MaleHair2,
        MaleHair3,
        MaleHair4,
        MaleHair5,
        MexicanHat,
        MiniHat,
        NormalHelmet,
        PartyCap,
        PotHat,
        QueenCrown,
        RomanHelmet,
        SafeHelmet,
        SamuraiHelmet,
        SharkHelmet,
        ShortBeard,
        SquidHelmet,
        TacticleHelmet,
        UniconHat,
        WarriorHelmet,
        WeldingHelmet,
        WideHikingBag,
        YellowHat,


        Cnt
    }
    public enum eFace
    {
        Ore,
        Touch,
        Sneaky,
        Surprised,
        Sparkling,
        Latte,
        Dense,
        Cool,
        Tongue,
        Scared,
        Smile,
        Salty,
        Love,
        Blue,
        Disappointed,
        Dizzy,
        SoWhat,
        Leisurely,
        Dumps,
        Cry,
        Undressed,
        GritTeeth,
        Benign,
        Wink,
        Tired,
        Bright,
        RealityCheck,
        Scold,
        Confused,
        Scoff,
        Grin,

        Cnt
    }
    #endregion[Resources Name]

    #region [Sound]
    public enum eBGMSound
    {
        Loading,
        Lobby,
        Store,
        Ingame,
        Story1,
        Story2,
        Story3,
        Story4,

        Cnt
    }
    public enum eEffectSound
    {
        Unlock,
        TouchToCharacter,
        GetItem,
        GetCrystal,
        Slide,

        Oil,
        Dead,
        Electric,
        DeadImmune,
        Ice,
        Super,
        Fire,

        Cnt
    }
    #endregion [Sound]
}
