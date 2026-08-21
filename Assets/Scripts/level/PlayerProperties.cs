using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine.UIElements;

// container for player runtime data, to be bound to UI
// property change stuff learned from https://www.youtube.com/watch?v=Fy88eYKGed0&t=579s
[CreateAssetMenu(fileName = "PlayerProperties", menuName="Game/PlayerProperties")]
public class PlayerProperties : ScriptableObject, IDataSourceViewHashProvider, INotifyBindablePropertyChanged
{
    [SerializeField] private long viewVersion = 0;
    
    // just making this public for ease of use (for now - need to figure this stuff out)
    [CreateProperty] public int CurrentNumParticles;
    {
        get => CurrentNumParticles;
        set
        {
            // prevent recursive loops when setting property
            if (CurrentNumParticles == value)
            {
                return;
            }

            CurrentNumParticles = value;
            NotifyPropertyChanged();
            
        }
    }


    public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

    public long GetViewHashCode() => viewVersion;

    // only have one property, so making this function is kind of unnecessary (for now, at least)
    // but it follows the guide referenced above, and will be able to handle more properties in the future
    void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        viewVersion++;
        propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(propertyName));
    }
}
