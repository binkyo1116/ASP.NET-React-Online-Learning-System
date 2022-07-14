using System;

namespace LMSLibrary
{
    public static class GradingHelper
    {
        public static int EvaluateAnswer(string currentAnswer, string expectedAnswer, bool? caseSensitive)
        {
            if (caseSensitive.HasValue && !caseSensitive.Value)
            {
                currentAnswer = currentAnswer.ToLower();
                expectedAnswer = expectedAnswer.ToLower();
            }

            int correct = 0;
            int incorrect = 0;

            int maxLength = Math.Max(expectedAnswer.Length, currentAnswer.Length);
            int minLength = Math.Min(expectedAnswer.Length, currentAnswer.Length);

            for (int i = 0; i < maxLength; i++)
            {
                if (i < minLength && currentAnswer[i] == expectedAnswer[i])
                {
                    correct++;
                }
                else
                {
                    incorrect++;
                }
            }

            return correct * 100 / (correct + incorrect);
        }

        private static int min(int a, int b, int c)
        {
            if (a <= b && a <= c)
            {
                return a;
            }
            else if (b <= a && b <= c)
            {
                return b;
            }
            else
            {
                return c;
            }
        }

        private static int minIndex(int a, int b, int c)
        {
            if (a <= b && a <= c)
            {
                return 1;
            }
            else if (b <= a && b <= c)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        private static int diff(int a, int b)
        {
            if (a == b)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private static bool areEqual(int current, int next)
        {
            return (next == current);
        }
        // not complete
        public static int EvaluateAnswer2(string currentAnswer, string expectedAnswer, bool? caseSensitive, out string feedback, out string editing)
        {
            if (caseSensitive.HasValue && !caseSensitive.Value)
            {
                currentAnswer = currentAnswer.ToLower();
                expectedAnswer = expectedAnswer.ToLower();
            }

            if (expectedAnswer.Equals(currentAnswer))
            {
                feedback = "100% correct!";
                editing = "";
                return 100;
            }

            int expectedLength = expectedAnswer.Length;
            int currentLength = currentAnswer.Length;

            int[,] d = new int[currentLength + 1, expectedLength + 1];

            for (int i = 0; i < currentLength + 1; i++)
            {
                d[i, 0] = i;
            }
            for (int j = 0; j < expectedLength + 1; j++)
            {
                d[0, j] = j;
            }

            for (int i = 1; i < currentLength + 1; i++)
            {
                for (int j = 1; j < expectedLength + 1; j++)
                {
                    int c = diff(currentAnswer[i - 1], expectedAnswer[j - 1]);
                    d[i, j] = min(d[i - 1, j] + 1, d[i, j - 1] + 1, d[i - 1, j - 1] + c);

                }
            }

            int row = currentLength;
            int column = expectedLength;
            int correct = 0;
            int incorrect = 0;
            editing = "";


            while (row >= 0 || column >= 0)
            {
                if (row - 1 < 0 || column - 1 < 0)
                {
                    if (row - 1 < 0 && column - 1 < 0)
                    {
                        row--;
                        column--;
                    }
                    else if (column - 1 < 0)
                    {
                        if (areEqual(d[row, column], d[row - 1, column]))
                        {
                            correct++;
                            editing = "E" + editing;
                        }
                        else
                        {
                            editing = "D" + editing;
                        }
                        row--;
                    }
                    else if (row - 1 < 0)
                    {
                        if (areEqual(d[row, column], d[row, column - 1]))
                        {
                            correct++;
                            editing = "E" + editing;
                        }
                        else
                        {
                            editing = "I" + editing;
                        }
                        column--;
                    }

                }
                else
                {


                    int direction = minIndex(d[row - 1, column - 1], d[row - 1, column], d[row, column - 1]);
                    switch (direction)
                    {
                        case 1:
                            if (areEqual(d[row, column], d[row - 1, column - 1]))
                            {
                                correct++;
                                editing = "E" + editing;
                            }
                            else
                            {
                                editing = "C" + editing;
                            }
                            row--;
                            column--;
                            break;
                        case 2:
                            if (areEqual(d[row, column], d[row - 1, column]))
                            {
                                correct++;
                                editing = "E" + editing;
                            }
                            else
                            {
                                editing = "D" + editing;
                            }
                            row--;
                            break;
                        case 3:
                            if (areEqual(d[row, column], d[row, column - 1]))
                            {
                                correct++;
                                editing = "E" + editing;
                            }
                            else
                            {
                                editing = "I" + editing;
                            }
                            column--;
                            break;
                    }

                }
            }

            if ((correct == expectedLength) && (correct == currentLength))
            {
                feedback = "100% correct!";
            }
            else if ((correct == expectedLength) && (correct < currentLength))
            {
                incorrect = currentLength - correct;
                feedback = "You have " + incorrect + " extra character" + ((incorrect > 1) ? "s" : "") + " in your answer! Hint: ";
            }
            else
            {
                incorrect = expectedLength - correct;
                feedback = "Only " + correct + " character" + ((correct > 1) ? "s are" : " is") + " correct in your answer! Hint: ";
            }

            return correct * 100 / (correct + incorrect);
        }

        public static double CalculateWeightedGrade(int grade, int weight)
        {

            return grade * weight / 100.0;

        }

        public static int GradePercent(int grade, int maxGrade)
        {
            return (int)Math.Round((grade * 100.0) / maxGrade);
        }
    }
}
