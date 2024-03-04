namespace Xunit_Stage_0
{
    public class CalculatorTest
    {
        Calculator.Calculator calculator = new Calculator.Calculator();
       
        [Fact]
        public void AdditionTest ()
        {
            Assert.Equal(3, calculator.Add(1, 2));
        }
    }
}