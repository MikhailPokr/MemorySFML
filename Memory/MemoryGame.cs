using System;
using System.Collections.Generic;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using SFML.Audio;
using SFML.Learning;

namespace MemoryGame
{
    internal class Memory : Game
    {
        private readonly static string backgroundTexture = LoadTexture("Background.png");
        private readonly static string cardsTexture = LoadTexture("Cards.png");
        private readonly static string cardsPressedTexture = LoadTexture("CardsPressed.png");
        private readonly static string cardsSelectedActiveTexture = LoadTexture("CardsSelectedActive.png");
        private readonly static string cardsSelectedPressedTexture = LoadTexture("CardsSelectedPressed.png");
        private readonly static string clickSound = LoadSound("click.wav");
        private readonly static string clickSound2 = LoadSound("click2.wav");

        private static Random random = new Random();

        private static bool WindowExist = false;
        private static int Timer = 0;
        private static bool ScoreModeClassic = true;
        private static bool TrioMode = false;
        private static bool DoublePairs = false;
        private static List<Element> Pack = new List<Element>();
        private static int Score = 0;
        private static int MaxScore = 0;

        private static bool ShowText = false;

        private static List<Button> Buttons = new List<Button>();

        private enum Element
        {
            Empty,
            Heart,
            Fire,
            Sun,
            Leaf,
            Water,
            Moon,
            Eye,
            Smile,
            Sad,
            Start,
            No,
            TwoMin,
            FiveMin,
            TenMin,
            Timer,
            DoublePairs,
            TrioMode,
            Question,
            Points,
            Restart
        }
        public static void Main()
        {
            if (!WindowExist)
            {
                InitWindow(800, 600, "Memory");
                WindowExist = true;
            }
            SetFont("DS Pixel Cyr.ttf");
            Buttons = new List<Button> { new Button(397, 169, true, Element.Start, 0),
                                         new Button(297, 289, true, Element.Timer, 2),
                                         new Button(347, 259, Timer != 0, Element.No),
                                         new Button(397, 289, Timer != 2, Element.TwoMin),
                                         new Button(447, 259, Timer != 5, Element.FiveMin),
                                         new Button(497, 289, Timer != 10, Element.TenMin),
                                         new Button(247, 379, !ScoreModeClassic, Element.Sun),
                                         new Button(247, 499, ScoreModeClassic, Element.Moon),
                                         new Button(547, 379, !DoublePairs, Element.DoublePairs, 0),
                                         new Button(547, 499, !TrioMode, Element.TrioMode, 0),
                                         new Button(47, 79, !ShowText, Element.Question, 0),
                                         new Button(647, 79, true, Element.Points, 2),
                                         new Button(697, 109, true, Element.Empty, 2),
                                         new Button(747, 79, true, Element.Empty, 2),
                                         new Button(347, 379, false, Element.Heart, 0),
                                         new Button(397, 409, false, Element.Fire, 0),
                                         new Button(447, 379, true, Element.Sun, 0),
                                         new Button(347, 439, true, Element.Leaf, 0),
                                         new Button(397, 469, true, Element.Water, 0),
                                         new Button(447, 439, true, Element.Moon, 0),
                                         new Button(347, 499, true, Element.Eye, 0),
                                         new Button(397, 529, true, Element.Smile,0),
                                         new Button(447, 499, true, Element.Sad,0),
            };
            Pack.Add(Element.Heart);
            Pack.Add(Element.Fire);
            if (Score > MaxScore)
                MaxScore = Score;
            while (true)
            {
                DispatchEvents();
                if (GetMouseButtonUp(Mouse.Button.Left))
                {
                    Button PressedButton = null;
                    foreach (Button button in Buttons)
                    {
                        if (button.Check())
                        {
                            PressedButton = button;
                        }
                    }
                    if (PressedButton != null)
                    {
                        if (PressedButton == Buttons[0])
                        {
                            if (Pack.Count > 1)
                            {
                                break;
                            }
                        }
                        else if (PressedButton == Buttons[1])
                        {
                        }
                        else if (PressedButton == Buttons[2])
                        {
                            if (!Buttons[2].Click(true, false))
                            {
                                Buttons[3].Click(true, true);
                                Buttons[4].Click(true, true);
                                Buttons[5].Click(true, true);
                                Timer = 0;
                            }
                        }
                        else if (PressedButton == Buttons[3])
                        {
                            if (!Buttons[3].Click(true, false))
                            {
                                Buttons[2].Click(true, true);
                                Buttons[4].Click(true, true);
                                Buttons[5].Click(true, true);
                                Timer = 2;
                            }
                        }
                        else if (PressedButton == Buttons[4])
                        {
                            if (!Buttons[4].Click(true, false))
                            {
                                Buttons[2].Click(true, true);
                                Buttons[3].Click(true, true);
                                Buttons[5].Click(true, true);
                                Timer = 5;
                            }
                        }
                        else if (PressedButton == Buttons[5])
                        {
                            if (!Buttons[5].Click(true, false))
                            {
                                Buttons[2].Click(true, true);
                                Buttons[3].Click(true, true);
                                Buttons[4].Click(true, true);
                                Timer = 10;
                            }
                        }
                        else if (PressedButton == Buttons[6])
                        {
                            if (!Buttons[6].Click(true, false))
                            {
                                Buttons[7].Click(true, true);
                                ScoreModeClassic = true;
                            }
                        }
                        else if (PressedButton == Buttons[7])
                        {
                            if (!Buttons[7].Click(true, false))
                            {
                                Buttons[6].Click(true, true);
                                ScoreModeClassic = false;
                            }
                        }
                        else if (PressedButton == Buttons[8])
                        {
                            DoublePairs = !Buttons[8].Click();
                        }
                        else if (PressedButton == Buttons[9])
                        {
                            TrioMode = !Buttons[9].Click();
                        }
                        else if (PressedButton == Buttons[10])
                        {
                            ShowText = !Buttons[10].Click();
                        }
                        else if (Buttons.Contains(PressedButton))
                        {
                            if (PressedButton.Click())
                                Pack.Remove(PressedButton.Picture);
                            else
                                Pack.Add(PressedButton.Picture);

                        }
                    }
                }

                DrawSprite(backgroundTexture, 0, 0);
                DrawText(345, 55, "Memory", 30);
                if (ShowText)
                    DrawAllText();
                foreach (Button button in Buttons)
                {
                    button.Draw();
                }
                DrawText(687, 102,$"{(Score < 10 ? "00" : Score < 100 ? "0" : "")}{Score}");
                DrawText(737, 72, $"{(MaxScore < 10 ? "00" : MaxScore < 100 ? "0" : "")}{MaxScore}");
                DisplayWindow();
                Delay(1);
            }
            int count = Pack.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < (TrioMode ? 3 : 2) * (DoublePairs ? 2 : 1) - 1; j++)
                {
                    Pack.Add(Pack[i]);
                }
            }
            count = Pack.Count;
            int[] PosX = {
                397, 447, 497, 547, 597, 647, 697, 397, 447, 497, 547, 597, 647, 697, 397, 447, 497, 547, 597, 647, 697, 397, 447, 497, 547, 597, 647, 697, 397, 447, 497, 547, 597, 647, 697, 397, 447, 497, 547, 597, 647, 697, 397, 447, 497, 547, 597, 647, 697, 447, 497, 547, 597, 647 };
            int[] PosY = {
                289, 259, 289, 259, 289, 259, 289, 229, 199, 229, 199, 229, 199, 229, 349, 319, 349, 319, 349, 319, 349, 169, 139, 169, 139, 169, 139, 169, 409, 379, 409, 379, 409, 379, 409, 109, 79, 109, 79, 109, 79, 109, 469, 439, 469, 439, 469, 439, 469, 499, 529, 499, 529, 499 };
            Buttons.Clear();
            Buttons = new List<Button>()
            {
                new Button(97, 529, true, Element.No),
                new Button(97, 109, true, Element.Timer, 2),
                new Button(97, 49, true, Timer == 0 ? Element.No : Timer == 2 ? Element.TwoMin : Timer == 5 ? Element.FiveMin : Element.TenMin, 2),
                new Button(147, 139, true, Element.Empty, 2),
                new Button(97, 229, true, Element.Points, 2),
                new Button(147, 259, true, Element.Empty, 2),
            };
            for (int i = 0; i < count; i++)
            {
                int randomImage = random.Next(0, Pack.Count);
                Buttons.Add(new Button(PosX[i], PosY[i], true, Pack[randomImage], 1, true));
                Pack.RemoveAt(randomImage);
            }
            List<Button> combo = new List<Button>();
            bool Show = ScoreModeClassic;
            float timer = 0;
            float multiplication = (Timer == 2 ? 2.5f : Timer == 5 ? 2 : 1.5f) * (TrioMode ? 2 : 1) * (DoublePairs ? 1.2f : 1);
            Score = 0;
            while (true)
            {
                DispatchEvents();
                timer += 1.3f;
                if ((!Buttons.Exists(x => x.Close == true) && !Show && combo.Count == 0) || (((int)timer / 3600) == Timer && Timer != 0))
                {
                    Main();
                }
                if (ScoreModeClassic && Show)
                {
                    if (timer == 1.3f)
                    {
                        foreach (Button button in Buttons)
                        {
                            if (button.Close)
                                button.OpenOrClose();
                            button.Draw();
                        }
                    }
                    if (timer > 100)
                    {
                        Show = false;
                        for (int i = 6; i < Buttons.Count; i++)
                        {
                            Buttons[i].OpenOrClose();
                        }
                    }
                }
                if ((combo.Count == 2 && !TrioMode) || (combo.Count == 3 && TrioMode) || (combo.Count == 2 && combo[0].Picture != combo[1].Picture))
                {
                    Delay(300);
                    int repeats = 0;
                    foreach (Button card in combo)
                    {
                        card.OpenOrClose();
                        repeats += card.OpenCount;
                    }
                    if (ScoreModeClassic)
                        Score -= (int)(2 * multiplication);
                    else
                    {
                        Score -= (int)(repeats/2 * multiplication);
                    }
                    if (Score < 0)
                        Score = 0;
                    combo.Clear();
                }
                if (GetMouseButtonUp(Mouse.Button.Left) && timer > 1.3f)
                {
                    Button PressedButton = null;
                    foreach (Button button in Buttons)
                    {
                        if (button.Check())
                        {
                            PressedButton = button;
                        }
                    }
                    if(PressedButton == Buttons[0])
                    {
                        PressedButton.Click();
                        Main();
                    }
                    else if (PressedButton == Buttons[1] ||
                             PressedButton == Buttons[2] ||
                             PressedButton == Buttons[3] ||
                             PressedButton == Buttons[4] ||
                             PressedButton == Buttons[5])
                    {
                    }
                    else if (Buttons.Contains(PressedButton))
                    {
                        if (PressedButton.Close && PressedButton.Click(true, true))
                        {
                            PressedButton.OpenOrClose();
                            if (combo.Count == 0)
                                combo.Add(PressedButton);
                            else if (combo.Count == 1)
                            {
                                if (!TrioMode)
                                {
                                    if (PressedButton.Picture == combo[0].Picture)
                                    {
                                        PressedButton.Click();
                                        combo[0].Click();
                                        combo.Clear();
                                        Score += (int)(10 * multiplication);
                                    }
                                    else
                                    {
                                        combo.Add(PressedButton);
                                    }
                                }
                                else
                                {
                                    combo.Add(PressedButton);
                                }
                            }
                            else
                            {
                                if (PressedButton.Picture == combo[0].Picture)
                                {
                                    PressedButton.Click();
                                    combo[0].Click();
                                    combo[1].Click();
                                    combo.Clear();
                                    Score += (int)(10 * multiplication);
                                }
                                else
                                {
                                    combo.Add(PressedButton);
                                }
                            }
                                
                        }
                            
                    }
                }

                DrawSprite(backgroundTexture, 0, 0);
                foreach (Button button in Buttons)
                {
                    button.Draw();
                }
                DrawText(135, 130, $"{(int)timer/3600}:{((int)timer/60%60 < 10 ? "0" : "")+((int)timer/60%60)}", 15);
                DrawText(138, 252, $"{(Score < 10 ? "00" : Score < 100 ? "0" : "")}{Score}");
                DisplayWindow();
                Delay(1);
            }
        }
        private static void DrawAllText()
        {
            DrawText(184, 100, "От Покровского Михаила(Afergo) в рамках проекта для SkillFactory", 12);

            DrawText(31, 150, "Справка по меню:", 10);
            DrawText(31, 165, "- Нажав \"Начать\" вы начнете игру.", 10);
            DrawText(31, 180, "- Вы можете нажимать на подсвеченные кнопки.", 10);
            DrawText(31, 195, "- Выбранное вами помечается \"Нажатыми\" кнопками.", 10);
            DrawText(31, 210, "- Внизу вы можете настроить картинки,", 10);
            DrawText(31, 220, "Которые будут использоваться для игры.", 10);
            DrawText(31, 230, "Это влияет на количество фишек,", 10);
            DrawText(31, 240, "Как и параметры справа.", 10);
            DrawText(31, 255, "- Игра не запустится, если", 10);
            DrawText(31, 265, "Будет только одна картинка.", 10);
            DrawText(31, 280, "- Кнопки после иконки часов означают", 10);
            DrawText(31, 290, "Ваш выбор таймера. Первая убирает его.", 10);

            DrawText(540, 150, "Об игре:", 10);
            DrawText(540, 165, "- Ваша задача заключается в нахождении", 10);
            DrawText(540, 180, "одинаковых карточек, поочередно их открывая.", 10);
            DrawText(540, 195, "- За удачное открытие вы будете получать очки,", 10);
            DrawText(540, 205, "а за неудачное терять их в зависимости", 10);
            DrawText(540, 215, "от режима игры.", 10);
            DrawText(540, 230, "- Подсчет очков меняется", 10);
            DrawText(540, 240, "В зависимости от настроек.", 10);

            DrawText(10, 350, "- Вначале поле открывается на пару", 10);
            DrawText(10, 360, "секунд", 10);
            DrawText(10, 375, "- За каждую ошибку снимаются очки", 10);
            DrawText(10, 390, "- Количество снятых очков всегда", 10);
            DrawText(10, 400, "одинакого.", 10);

            DrawText(10, 470, "- Поле не открывается вначале", 10);
            DrawText(10, 485, "- Первая ошибка не тратит очков ", 10);
            DrawText(10, 500, "- С каждой ошибкой штраф", 10);
            DrawText(10, 510, "увеличивается.", 10);

            DrawText(590, 360, "- При игре с этой функцией", 10);
            DrawText(590, 370, "количество каждой из карточек", 10);
            DrawText(590, 380, "удвоится.", 10);

            DrawText(590, 470, "- Данная опция меняет игру,", 10);
            DrawText(590, 480, "требуя от вас найти две", 10);
            DrawText(590, 490, "похожие на открытую карточки,", 10);
            DrawText(590, 500, "вместо одной.", 10);

            DrawText(590, 30, "- Счет. Последний и максимальный", 10);
        }
        private class Button
        {
            private readonly int X;
            private readonly int Y;
            private bool Active;
            public readonly Element Picture;
            private int VisualLock;
            public int OpenCount = 0;
            public bool Close { get; private set; }
            public Button(int x, int y, bool active, Element picture, int visualLock = 1, bool close = false)
            {
                X = x;
                Y = y;
                Active = active;
                Picture = picture;
                VisualLock = visualLock;
                Close = close;
            }
            public bool Check()
            {
                int squareLeg1 = (X - MouseX) * (X - MouseX);
                int squareLeg2 = (Y - MouseY) * (Y - MouseY);
                double length = Math.Sqrt(squareLeg1 + squareLeg2);
                if (length < 29)
                    return true;
                return false;
            }
            public bool Click(bool ignore = false, bool ingnoreCondition = false)
            {
                if (ignore && ingnoreCondition == Active)
                        return Active;
                Active = !Active;
                PlaySound(clickSound, 70);
                return Active;
            }
            public void Draw()
            {
                Element picture = Close ? Element.Empty : Picture;
                string name;
                int x = X - 35;
                int y = Y - 31;
                int left = 0;
                int top = 0;
                int width = 70;
                int height = 62;
                if (!Check() || VisualLock == 3)
                {
                    if (Active)
                        name = cardsTexture;
                    else
                        name = cardsPressedTexture;
                }
                else
                {
                    if (Active)
                    {
                        if (VisualLock != 2)
                            name = cardsSelectedActiveTexture;
                        else
                            name = cardsTexture;
                    }
                    else
                    {
                        if (VisualLock != 1)
                            name = cardsSelectedPressedTexture;
                        else
                            name = cardsPressedTexture;
                    }
                        
                }
                switch (picture)
                {
                    case Element.Empty:
                        left = 0;
                        top = 0;
                        break;
                    case Element.Heart:
                        left = 72;
                        top = 0;
                        break;
                    case Element.Fire:
                        left = 144;
                        top = 0;
                        break;
                    case Element.Sun:
                        left = 216;
                        top = 0;
                        break;
                    case Element.Leaf:
                        left = 72;
                        top = 64;
                        break;
                    case Element.Water:
                        left = 144;
                        top = 64;
                        break;
                    case Element.Moon:
                        left = 216;
                        top = 64;
                        break;
                    case Element.Eye:
                        left = 72;
                        top = 128;
                        break;
                    case Element.Smile:
                        left = 144;
                        top = 128;
                        break;
                    case Element.Sad:
                        left = 216;
                        top = 128;
                        break;
                    case Element.Start:
                        left = 0;
                        top = 64;
                        break;
                    case Element.No:
                        left = 0;
                        top = 128;
                        break;
                    case Element.TwoMin:
                        left = 72;
                        top = 192;
                        break;
                    case Element.FiveMin:
                        left = 144;
                        top = 192;
                        break;
                    case Element.TenMin:
                        left = 216;
                        top = 192;
                        break;
                    case Element.Timer:
                        left = 72;
                        top = 256;
                        break;
                    case Element.DoublePairs:
                        left = 144;
                        top = 256;
                        break;
                    case Element.TrioMode:
                        left = 216;
                        top = 256;
                        break;
                    case Element.Question:
                        left = 72;
                        top = 320;
                        break;
                    case Element.Points:
                        left = 144;
                        top = 320;
                        break;
                    case Element.Restart:
                        left = 216;
                        top = 320;
                        break;
                }
                DrawSprite(name, x, y, left, top, width, height);
            }
            public void OpenOrClose()
            {
                Close = !Close;
                if (Close)
                {
                    VisualLock = 1;
                    OpenCount++;
                }
                else
                    VisualLock = 3;
                PlaySound(clickSound2, 50);
            }
        }

    }
}
