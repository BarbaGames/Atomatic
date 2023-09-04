namespace Code.Scripts.Achievements
{
    public abstract class Achievement : IFloatModifier
    {
        //deberia tener una imagen?
        public string description;
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
