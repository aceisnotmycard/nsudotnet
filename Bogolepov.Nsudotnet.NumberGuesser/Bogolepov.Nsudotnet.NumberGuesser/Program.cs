﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bogolepov.Nsudotnet.NumberGuesser
{
    // Да-да, здесь это не нужно, просто захотел попробовать
    public static class ArrayExtensions
    {
        public static T RandomElement<T>(this T[] array, Random random) => array[random.Next() % array.Length];
    }

    class Program
    {
        private static readonly string[] Comments =
        {
            "You're sooo stupid, {0}.",
            "Someone is dumber than Hippo. Yes, {0} it's you.",
            "Bring me someone smarter, {0}.",
            "{0}, you completely disappointed me."
        };

        private const int Fuckometer = 4;

        static void Main(string[] args)
        {
            var guessCounter = 0;
            var guesses = new string[1000];
            var random = new Random();
            var answer = random.Next(0, 100);

            Console.WriteLine("Hello! What's your name?");
            var name = Console.ReadLine();
            var startTime = DateTime.Now;
            while (true)
            {
                Console.WriteLine($"Your guess?");
                var input = Console.ReadLine();
                var guess = 0;
                if (int.TryParse(input, out guess))
                {
                    if (guess == answer)
                    {
                        Console.WriteLine("Correct!");
                        Console.WriteLine(guessCounter == 0
                            ? "Wow! Amazing! Such answer!"
                            : $"Your {guessCounter} guesses are:\n {string.Join("\n", guesses.Take(guessCounter))}"
                        );
                        Console.WriteLine($"It took {(DateTime.Now - startTime).TotalMinutes} minutes");
                        Console.Read();
                        return;
                    }
                    else
                    {
                        var difference = guess < answer ? "smaller" : "bigger";
                        Console.WriteLine($"Your guess is {difference} than answer");
                        guesses[guessCounter] = $"{guess} which is {difference}";
                        guessCounter++;
                        if (guessCounter % Fuckometer == 0)
                        {
                            Console.WriteLine(Comments.RandomElement(random), name);
                        }
                    }
                }
                else if (input == "q")
                {
                    Console.WriteLine("Goodbye, sucker");
                    Console.Read();
                    return;
                }
                else
                {
                    Console.WriteLine("Well, it is definitely not number nor 'q', but it should be.");
                }
            }
        }
    }
}
