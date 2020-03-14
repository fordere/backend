using System;

namespace forderebackend.ServiceInterface.Entities
{
    public class Skill
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public DateTime CalculationTime { get; set; }

        public double Level { get; set; }

        public double Derivation { get; set; }
    }
}
