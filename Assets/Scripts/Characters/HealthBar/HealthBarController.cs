using UnityEngine;
using UnityEngine.UI;

namespace Characters.HealthBar
{
    /// <summary>
    /// Represents a controller for handling characters HP.
    /// </summary>
    public sealed class HealthBarController : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;

        /// <summary>
        /// Updates the health bar.
        /// </summary>
        /// <param name="health">Amount of hp.</param>
        public void SetHealth(int health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        
        /// <summary>
        /// Fills the health bar.
        /// </summary>
        /// <param name="health">Amount of hp.</param>
        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
            fill.color = gradient.Evaluate(1f);
        }
    }
}
