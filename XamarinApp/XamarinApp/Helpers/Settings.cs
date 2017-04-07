// Helpers/Settings.cs

using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace XamarinApp.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. 
    /// All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(AccessToken), String.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(AccessToken), value);
            }
        }

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Username), String.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Username), value);
            }
        }

        public static string Email
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Email), String.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Email), value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Password), String.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Password), value);
            }
        }
    }
}