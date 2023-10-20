using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreadAndSet.CardSystem
{
    public class CardSystem
    {
        readonly private int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        readonly private string[] cardSuits = { "Clubs", "Spades", "Diamonds", "Hearts" };
        public int SelectedNumber { get; set; }
        public Guid Id{ get; set; }
		
        public string SelectedCard { get; set; }
		public CardSystem()
		{
			Id = Guid.NewGuid();

			string  idString= Id.ToString();
			char randomFactor = idString[idString.Length - 1];
			var random = new Random((int)randomFactor);

			int numberIndex = random.Next(0, cardNumbers.Length - 1);
			int suitIndex = random.Next(0, cardSuits.Length - 1);

			this.SelectedNumber = cardNumbers[numberIndex];
			this.SelectedCard = $"{cardNumbers[numberIndex]} of {cardSuits[suitIndex]}";
			
		}

		public CardSystem(int selectedCardNumber, CardSuit suitNumber)
		{
			this.SelectedNumber = cardNumbers[selectedCardNumber];
			this.SelectedCard = $"{cardNumbers[selectedCardNumber]} of {cardSuits[(int)suitNumber]}";
		}


	}
    public enum CardSuit
    {
		Clubs = 0,
		Spades = 1,
		Diamonds = 2,
		Hearts = 3,
	}
}
