using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Register the Player as a singleton so it can be accessed globally
        builder.RegisterComponentInHierarchy<Player>().AsSelf();
        builder.RegisterComponentInHierarchy<UIManager>().AsSelf();
        builder.RegisterComponentInHierarchy<FloatingJoystick>().AsSelf();
        builder.RegisterComponentInHierarchy<ZombieSpawnerManager>().AsSelf();
        builder.RegisterComponentInHierarchy<GunFactory>().AsSelf();
        builder.RegisterComponentInHierarchy<GameManager>().AsSelf(); 
    }
}
