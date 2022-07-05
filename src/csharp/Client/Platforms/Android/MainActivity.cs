﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace DewIt;

[Activity(Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    Label="DewIt",
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
