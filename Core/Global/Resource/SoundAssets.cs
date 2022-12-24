namespace Entrogic.Core.Global.Resource
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
            Volume = 0.65f,
            PitchVariance = 0.4f,
            MaxInstances = 0
        };

        public static readonly SoundStyle HeavyMachineGun = new($"{SoundBase}Items/HeavyMachineGun") {
            Volume = 0.65f,
            PitchVariance = 0.4f,
            MaxInstances = 0
        };

        public static readonly SoundStyle BowFilled = new($"{SoundBase}Items/BowFilled") {
            Volume = 1f,
            PitchVariance = 0.2f,
            Pitch = -0.2f,
            MaxInstances = 0
        };

        public static readonly SoundStyle BowReleased = new($"{SoundBase}Items/BowReleased") {
            MaxInstances = 0
        };

        public static readonly SoundStyle BowPulling = new($"{SoundBase}Items/BowPulling") {
            Volume = 1f,
            PitchRange = (-0.6f, -0.3f),
            MaxInstances = 0
        };

        public static readonly SoundStyle StoneHand = new($"{SoundBase}NPCs/StoneHand") {
            Volume = 1f,
            PitchRange = (-0.2f, 0.2f),
            MaxInstances = 0
        };
    }
}
