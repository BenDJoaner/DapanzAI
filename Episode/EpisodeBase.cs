using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeBase
{
    public float Horizontal;//横向输入
    public float Vertical;   //纵向输入
    public bool Jump;      //跳跃输入

    public static EpisodeBase zero { get => Stop();}
        
    public static EpisodeBase right { get => h_direct(-1); }

    public static EpisodeBase left { get => h_direct(1); }

    public static EpisodeBase up { get => v_direct(1); }

    public static EpisodeBase down { get => v_direct(-1); }

    public static EpisodeBase Stop()
    {
        EpisodeBase data = new EpisodeBase();
        return data;
    }

    public static EpisodeBase h_direct(float v)
    {
        EpisodeBase data = new EpisodeBase();
        data.Horizontal = v;
        return data;
    }

    public static EpisodeBase v_direct(float v)
    {
        EpisodeBase data = new EpisodeBase();
        data.Vertical = v;
        return data;
    }
}
