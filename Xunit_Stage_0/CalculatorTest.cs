using Stage_0;

namespace Xunit_Stage_0
{
    public class CalculatorTest
    {
        private Calc calculator = new Calc();
       
        [Fact]
        public void AdditionTest ()
        {
            Assert.Equal(3, calculator.Add(1, 2));
        }
    }
}