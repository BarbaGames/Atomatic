using UI;

namespace Achievements
{
    public abstract class Achievement : IFloatModifier
    {
        public Configuration config;
        protected double modifier;
        public abstract void Modify(ref double originalValue);
    }

    internal class MultiplierAchievement : Achievement
    {
        public override void Modify(ref double originalValue)
        {
            originalValue *= modifier;
        }
    }

    internal class AdditiveAchievement : Achievement
    {
        public override void Modify(ref double originalValue)
        {
            originalValue += modifier;
        }
    }

    public interface IFloatModifier
    {
        void Modify(ref double originalValue);
    }
}
