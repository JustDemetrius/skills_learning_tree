using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IService> _services = new();

        public static IService AddInstance(IService service)
        {
            var type = service.GetType();
            _services.Add(type, service);

            return service;
        }

        public static IService Add<T>(T system) where T : IService
        {
            var type = system.GetType();
            
            if (_services.ContainsKey(type))
            {
                Debug.LogError($"System of type {type} is already added to Systems");
                return null;
            }
            _services.Add(type, system);

            return system;
        }
        
        public static IService Add<T>()
        {
            var system = Activator.CreateInstance(typeof(T)) as IService;
            return Add(system);
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            _services.TryGetValue(type, out var system);
            return system != null ? (T)system : default;
        }

        public static void Init()
        {
            foreach (var system in _services)
            {
                system.Value.Init();
            }
        }

        public static void Start()
        {
            foreach (var system in _services.Values)
            {
                system.Start();
            }
        }
        
        public static void GlobalTick(float deltaTime)
        {
            foreach (var system in _services.Values)
            {
                system.Tick(deltaTime);
            }
        }
    }
}