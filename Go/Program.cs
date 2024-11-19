Console.OutputEncoding = System.Text.Encoding.UTF8;

const int empty = 0;
const int black = 1;
const int white = 2;

var cursorPos = (x: 9, y: 9);
var board = new int[19, 19];
var turn = black;


Init();
while (true)
    Update();


void Init()
{
    Console.SetWindowSize(40, 20);
    Console.SetBufferSize(40, 20);  //Removes scrollbar
    Console.Title = "Go";

    Console.ForegroundColor = ConsoleColor.DarkGray;
    for (int x = 0; x < 19; x++)
    {
        Console.Write("\n ");
        for (int y = 0; y < 19; y++)
        {
            board[x, y] = empty;
            if (x is 3 or 9 or 15 && y is 3 or 9 or 15)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(" +");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else
                Console.Write(" +");
        }
        Thread.Sleep(1);
    }
    Console.ForegroundColor = ConsoleColor.White;
    
    Console.SetCursorPosition(10, 0);
    Console.Write("B L A C K   T U R N");
}

void Update()
{
    Console.SetCursorPosition(cursorPos.x * 2 + 2, cursorPos.y + 1);    //Transforms cursorPos to Console-space
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.LeftArrow:
            if (cursorPos.x > 0)
                cursorPos.x--;
            break;
        case ConsoleKey.RightArrow:
            if (cursorPos.x < 18)
                cursorPos.x++;
            break;
        case ConsoleKey.UpArrow:
            if (cursorPos.y > 0)
                cursorPos.y--;
            break;
        case ConsoleKey.DownArrow:
            if (cursorPos.y < 18)
                cursorPos.y++;
            break;
        case ConsoleKey.Spacebar: 
        case ConsoleKey.Enter:
            PlaceStone();
            break;
    }
}

void PlaceStone()
{
    if (board[cursorPos.x, cursorPos.y] != empty)
        return;
    if (turn == black)
    {
        Console.Write("\u25cb");
        board[cursorPos.x, cursorPos.y] = black;
        turn = white;
        Console.SetCursorPosition(10, 0);
        Console.Write("W H I T E   T U R N");
    }
    else
    {
        Console.Write("\u25cf");
        board[cursorPos.x, cursorPos.y] = white;
        turn = black;
        Console.SetCursorPosition(10, 0);
        Console.Write("B L A C K   T U R N");
    }
    Capture();
}

void Capture()
{
    
}