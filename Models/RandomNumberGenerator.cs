using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAsker.Models
{
    public class RandomNumberGenerator
    {
        private Random random;
        private List<int> generatedNumbers;

        public RandomNumberGenerator()
        {
            random = new Random();
            generatedNumbers = new List<int>();
        }

        public int getRandomNumber(int lastItemIndex)
        {
            int randomNumber;

            do
            {
                randomNumber = random.Next(0, lastItemIndex);
            } while (generatedNumbers.Contains(randomNumber));

            generatedNumbers.Add(randomNumber);

            return randomNumber;
        }
    }
}
