// CallBackFunc.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

void PrintHello()
{
	printf("hello world!\n");
}

void DoSomeThing(void (*p)())
{
	p();
}

int main()
{
	DoSomeThing(PrintHello);
    return 0;
}

