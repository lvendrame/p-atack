// TestesCpp.cpp : main project file.

#include "stdafx.h"
/*#include <stdio.h>
#include <conio.h>

float ObterNota(char* mensagem);

int main() 
{ 
	float n1 = ObterNota("Digite a primeira nota\n");
	float n2 = ObterNota("Digite a segunda nota\n");
	float n3 = ObterNota("Digite a terceira nota\n");
	float media = (n1+n2+n3)/3.0f; 
	
	if (media<3) 
	{
		printf("Reprovado\n");
	} 
	else if (media<7) //já será maior ou igual a 3, por isto vc usou o elseif não é??
	{
		printf("Exame\n");
	} 
	else //não precisa de comparação, pois se os dados inseridos estiverem corretos, será menor  ou igual a 10
	{
		printf("Aprovado");
	} 
	getch(); 
} 

float ObterNota(char* mensagem)
{
	float result = -1;
	int continua = 1;
	do
	{
		printf(mensagem); 
		scanf("%f",&result);
		if(result < 0 || result > 10)
			printf("Digite um valor entre 0 e 10.\n");
		else
			continua = 0;
	}
	while(continua);
	return result;
}*/
#include <stdio.h> 
#include <windows.h> 
int main(void) 
{ 
    int i = 0; 
    int cont = 0; 
    printf("Duracao do Som: "); 
    scanf("%d",&i); 
    for(cont = 2600; cont <= 5000; cont++) 
    { 
        Beep(cont,i); 
    } 
    return 0; 
} 