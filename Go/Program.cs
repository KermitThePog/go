﻿Console.OutputEncoding = System.Text.Encoding.UTF8;

const int empty = 0;
const int black = 1;
const int white = 2;

var cursorPos = (x: 9, y: 9);
var board = new int[19, 19];
var hasLiberty = new bool[19, 19];
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
    board[cursorPos.x, cursorPos.y] = turn;
    if (!Capture())
    {
        board[cursorPos.x, cursorPos.y] = empty;
        return;
    }
    Console.SetCursorPosition(cursorPos.x * 2 + 2, cursorPos.y + 1);    //Transforms cursorPos to Console-space
    if (turn == black)
    {
        Console.Write("\u25cb");
        turn = white;
        Console.SetCursorPosition(10, 0);
        Console.Write("W H I T E   T U R N");
    }
    else
    {
        Console.Write("\u25cf");
        turn = black;
        Console.SetCursorPosition(10, 0);
        Console.Write("B L A C K   T U R N");
    }
}

bool Capture(bool currentMoveProtected = true)
{
    var _board = board;
    //reset all liberties
    for (int x = 0; x < 19; x++)
    {
        for (int y = 0; y < 19; y++)
        {
            hasLiberty[x, y] = false;
        }
    }
    
    if (currentMoveProtected)
        hasLiberty[cursorPos.x, cursorPos.y] = true;

    //find liberties
    var changed = true;
    while (changed)
    {
        changed = false;
        for (int x = 0; x < 19; x++)
        {
            for (int y = 0; y < 19; y++)
            {
                if (hasLiberty[x,y])
                    continue;
                foreach (var tile in ((int x, int y)[])[(x, y-1), (x, y+1), (x-1, y), (x+1, y)])
                {
                    if (tile.x is -1 or 19 || tile.y is -1 or 19)
                        continue;
                    if (_board[tile.x, tile.y] == empty || (_board[tile.x, tile.y] == _board[x, y] && hasLiberty[tile.x, tile.y]))
                    {
                        hasLiberty[x, y] = true;
                        changed = true;
                    }
                }
            }
        }
    }
    
    //remove all stones without liberties
    bool captured = false;
    Console.ForegroundColor = ConsoleColor.DarkGray;
    for (int x = 0; x < 19; x++)
    {
        for (int y = 0; y < 19; y++)
        {
            if (!hasLiberty[x, y])
            {
                captured = true;
                _board[x, y] = empty;
                Console.SetCursorPosition(x * 2 + 1, y + 1);
                if (x is 3 or 9 or 15 && y is 3 or 9 or 15)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" +");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else
                    Console.Write(" +");
            }
        }
    }
    Console.ForegroundColor = ConsoleColor.White;

    if (captured && !currentMoveProtected)
        return false;
    if (!captured && currentMoveProtected)    //if placed stone would be insta-captured
        return Capture(false);
    board = _board;
    return true;    //move is valid
}