using UnityEngine; 
namespace Game.Scripts { 
    public class StopEffect : MonoBehaviour 
    { 
        private void Start() 
        { 
            Destroy(gameObject, this.GetComponent<ParticleSystem>().main.duration); 
        } 
        private void Update() {} 
    } 
}