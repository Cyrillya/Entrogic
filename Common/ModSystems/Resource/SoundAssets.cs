using Terraria.Audio;

namespace Entrogic.Common.ModSystems
{
    public class SoundAssets : ModSystem
    {
        internal const string SoundBase = "Entrogic/Assets/Sounds/";

        public static readonly SoundStyle Pistol = new($"{SoundBase}Items/Pistol") {
            Volume = 0.8f,
            PitchVariance = 0.3f,
            MaxInstances = 0
        };

        public static readonly SoundStyle SubmachineGun = new($"{SoundBase}Items/SubmachineGun") {
            Volume = 0.9f,
            PitchVariance = 0.4f,
            MaxInstances = 0
        };

        public static readonly SoundStyle HeavyMachineGun = new($"{SoundBase}Items/HeavyMachineGun") {
            Volume = 0.9f,
            PitchVariance = 0.4f,
            MaxInstances = 0
        };
    }
}
