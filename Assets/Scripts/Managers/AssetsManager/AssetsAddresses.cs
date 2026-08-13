using UI;
using System;
using Entities;
using System.Collections.Generic;

namespace Managers
{
    public static class AssetsAddresses
    {
        private static readonly Dictionary<Type, string> Addresses = new()
        {
            //Entities
            { typeof(Obstacle), "Obstacle" },
            { typeof(Projectile), "Projectile" },
            { typeof(GameplaySphere), "GameplaySphere" },
            
            //UI
            { typeof(TestHUD), "TestHUD" }
        };

        public static string Get<T>()
        {
            if (Addresses.TryGetValue(typeof(T), out var address))
                return address;

            throw new Exception($"Address for {typeof(T).Name} is not registered.");
        }
    }
}