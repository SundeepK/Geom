using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace geom
{
    //class BloomSettings
    //{

    //    public  string name;
    //    public  float bloomThreshold;
    //    public  float blurAmount;
    //    public  float bloomIntensity;
    //    public  float baseIntensity;
    //    public  float bloomSaturation;
    //    public  float baseSaturation;

    //    public BloomSettings(string name, float bloomThreshold, float blurAmount,
    //                 float bloomIntensity, float baseIntensity,
    //                 float bloomSaturation, float baseSaturation)
    //    {
    //        this.name = name;
    //        this.bloomThreshold = bloomThreshold;
    //        this.blurAmount = blurAmount;
    //        this.bloomIntensity = bloomIntensity;
    //        this.baseIntensity = baseIntensity;
    //        this.bloomSaturation = bloomSaturation;
    //        this.baseSaturation = baseSaturation;
    //    }

    //    public static BloomSettings[] presets = 
    //    {                   
    //                             //Name      Thresh    Blur   Bloom   Base   BloomSat  BaseSat
    //        //new BloomSettings("DefaultBloom",  0.5f,  4,   1.5f, 1,    1,       1)
    //         new BloomSettings("Saturated",   0.20f,  3f,   1000,    2.5f,    6,      0.5f)
        

    //    };


    //}

    public static class BloomSettings
        {
        public static string name = "DefaultBloom";
        public static float bloomThreshold = 0.20f;
        public static float blurAmount = 3f;
        public static float bloomIntensity = 1000;
        public static float baseIntensity = 2.5f;
        public static float bloomSaturation = 7;
        public static float baseSaturation = 0.5f;

        }



}
