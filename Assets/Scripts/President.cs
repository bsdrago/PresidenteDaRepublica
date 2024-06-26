﻿using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Testes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class President : MonoBehaviour
{
    [SerializeField] private GameObject     consoleContainer;
    [SerializeField] private TMP_Text       textPrefab;
    [SerializeField] private TMP_InputField inputPrefab;

    public GameState _state = GameState.INIT;

    public  List<ConsoleLine>          consoleLines = new List<ConsoleLine>();
    private Dictionary<string, string> _inputs      = new Dictionary<string, string>();
    private bool                       _control     = false;
    private bool                       _canPrint    = true;

    private TMP_InputField _consoleInput;
    private ConsoleLine    _consoleData;



    // variaveis da simulação
    private float  ML = 1_000_000f;
    private int    P; // POPULACAO - Population?
    private int    U; // DESEMPREGADOS - UNEMPOLYMENT?
    private int    IV   = 236; // INVERSAO
    private int    GE   = 118; // GASTO DO GOVERNO - GOVENMENT EXPENSE ?
    private int    GU   = 0; // CUSTO DO DESEMPREGO
    private int    GI   = 0; //  $ POR IMPOSTOS
    private int    WN   = 100; //  NOVOS SALARIOS
    private int    WO   = 100; //  ANTIGOS SALARIOS
    private int    IP   = 5; //  % DE INFLACAO
    private int    GDP  = 440; //  PROD.NAC.BRUTO
    private int    AGDP = 440; //  BASE DO PROD.NAC.BRUTO
    private int    RGDP = 440; //  PNB REAL
    private int    CN   = 354; //  CONSTANTE ECONOMICA
    private int    Z    = 1;
    private int    GAME = 0;
    private int    FLAG = 0;
    private int    Y    = 0; // ANOS NA PRESIDENCIA
    private string AS; // Nome do presidente
    private int    BD;

    private void RefreshTerminal()
    {
        foreach (ConsoleLine line in consoleLines)
        {
            if (_canPrint)
            {
                _control = false;
                Debug.Log("CL: " + consoleLines.Count);
                if (line.type == ConsoleLine.Type.text)
                {
                    _canPrint = false;
                    TMP_Text consoleLine = Instantiate(textPrefab, consoleContainer.transform);
                    consoleLine.SetText(line.content);
                    consoleLines.Remove(line);
                    _control  = true;
                    _canPrint = true;
                }

                if (line.type == ConsoleLine.Type.input)
                {
                    _canPrint     = false;
                    _consoleInput = Instantiate(inputPrefab, consoleContainer.transform);

                    _consoleInput.onEndEdit.AddListener((string str) =>
                    {
                        Debug.Log("Input: " + str);
                        Debug.Log("Variavel: " + _consoleData.content);
                        _inputs.TryAdd(_consoleData.content, str);
                        Debug.Log("Tamanho:" + _inputs.Count);
                        _consoleInput.onEndEdit.RemoveAllListeners();
                        _control  = true;
                        _canPrint = true;

                    });
                    _consoleData = line;
                    consoleLines.Remove(line);
                }
                StartCoroutine(WaitControl());
            }
        }
    }

    private IEnumerator WaitControl()
    {
        yield return new WaitWhile(() => _control != true);
    }

    private void Print(string text)
    {
        consoleLines.Add(new ConsoleLine() { type = ConsoleLine.Type.text, content = text });

    }

    private void InputText(string variable)
    {
        ConsoleLine cl = new ConsoleLine() { type = ConsoleLine.Type.input, content = variable };
        consoleLines.Add(cl);
    }


    private void Cls()
    {
        foreach (Transform child in consoleContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        Cls();

        switch (_state)
        {
            case GameState.INIT:
                Inicializacao();
                break;
            case GameState.GASTOS_DO_GOVERNO:
                break;
            case GameState.GASTOS_COM_SALARIOS:
                break;
            case GameState.PERMITE_IMIGRANTES:
                break;
            case GameState.QUANTOS_IMIGRANTES:
                break;
            case GameState.CALCULA:
                break;
            case GameState.ENDGAME:
                break;
        }
        RefreshTerminal();
    }

    private void Inicializacao()
    {
        ML   = 1000000;
        P    = Mathf.RoundToInt(3 * ML);
        U    = P / 10;
        IV   = 236;
        GE   = 118;
        GU   = 0;
        GI   = 0; //  $ POR IMPOSTOS
        WN   = 100; //  NOVOS SALARIOS
        WO   = 100; //  ANTIGOS SALARIOS
        IP   = 5; //  % DE INFLACAO
        GDP  = 440; //  PROD.NAC.BRUTO
        AGDP = 440; //  BASE DO PROD.NAC.BRUTO
        RGDP = 440; //  PNB REAL
        CN   = 354; //  CONSTANTE ECONOMICA
        Z    = 1;
        GAME = 0;
        FLAG = 0;
        Y    = 0; // ANOS NA PRESIDENCIA
        BD   = 0;

        Print("Digite o seu nome, Presidente");
        InputText("name");
        // if (_inputs.ContainsKey("name"))
            // _state = GameState.GASTOS_DO_GOVERNO;
    }


    private void LoopPrincipal()
    {

        P = Mathf.FloorToInt(P + (P * 273 / ML)); // Atualiza população
        // Impressao tava aqui - era print + input
        // 1130 GOSUB 1650: REM CALCULOS
        // 1140 ' AQUI E VERIFICADO DO FIM DA SIMULAÇÃO
        // 1150 GOSUB 1860: REM NIVEL DE VIDA
        // 1160 GOSUB 2590: REM ACABOU?
        // 1170 GOSUB 1940: REM NIVEL DE INFLACAO
        // 1180 GOSUB 2590: REM ACABOU?
        // 1190 GOSUB 2010: REM DESEMPREGO
        // 1200 GOSUB 2590: REM ACABOU?
        // 1210 GOSUB 1790: REM DEFICIT
        // 1220 GOSUB 2590: REM ACABOU?
        // 1230 GOTO 1110
    }

    private void Impressao(GameState gs)
    {
        Cls();
        Print("Presidente:" + AS);
        Print("No poder ha' " + (Y + Z / 4) + " anos.");
        Print("=====================================");
        Print("--------- SITUACAO DO PAIS ----------");
        Print("=====================================");
        Print("Populacao: " + P);
        Print("Desempregados: " + Mathf.FloorToInt(U) + " " + (100 * U / P) + "%");
        Print("Sal.minimo: $" + Mathf.FloorToInt(WO) + " Inflacao: " + Mathf.FloorToInt(IP) + "%");
        Print(" ");
        Print("Gastos no ultimo periodo: $M" + Mathf.FloorToInt(GE));
        Print("Custo do desemprego:$M " + Mathf.FloorToInt(10 * GU) / 10);
        Print(" ");
        Print("Entradas por impostos: $M" + Mathf.FloorToInt(GI * 10) / 10);
        Print("Caixa atual: $M" + Mathf.FloorToInt(BD * 10) / 10);
        Print("Produto Nacional Bruto: $M" + Mathf.FloorToInt(GDP * 10) / 10);
        if (Y + 7 / 4 > .5)
            Print("MUDANCA NO NIVEL DE VIDA" + Mathf.FloorToInt((2 * (((float)RGDP / AGDP) * 100) - 100) / 3) + "%");
        Print("Invest. publicos: $M " + Mathf.FloorToInt(IV * 10) / 10);
        Print("-----------------------------------");
        Print("Sr.Presidente " + AS + ":");

        switch (gs)
        {
            case GameState.GASTOS_DO_GOVERNO:
                Print("Informe os gastos do governo $M");
                InputText("GE");
                break;

        }
        // 1450 GOSUB 2610: INPUT "
        // 1460 GOSUB 2610
        // 1470 INPUT "e os gastos com salarios $M";WN
        // 1480 WN = WN + 1E-03
        // 1490 GOSUB 2610
        // 1500 PRINT "E' a favor dos imigrantes (S/N) ?"
        // 1510 X$ = INKEY$
        // 1520 IF X$<>"S" AND X$<>"N" AND X$<>"s" AND X$<>"n" THEN GOTO 1510
        // 1530 PRINT TAB(2D) "Ok... "X$ 
        // 1540 FOR H=1 TO 500: NEXT H
        // 1550 IF X$<>"S" AND X$<>"s" THEN RETURN
        // 1560 GOSUB 2610
        // 1570 PRINT "Permitira' a entrada de quantos imigrantes";
        // 1580 INPUT M
        // 1590 IF M<O THEN GOTO 1580 
        // 1600 P = P+M
        // 1610 RETURN
    }


}





// 1620 ' ****************************************
// 1630 ' CALCULOS
// 1640 ' ****************************************
// 1650 CN = CN+(CN*IP/100)
// 1660 U = P*(GE+IV)/(CN*10)+P*(IP/1000)
// 1670 GU = U*WN/ML: ' CUSTO DO DESEMPREGO
// 1680 GI = (((P-U)*WN*.4)/ML): ' REM RENDA DOS IMPOSTOS
// 1690 BD = BD+GI-GU-GE: ' REM DEFICIT
// 1700 AGDP = AGDP*(1+(IP/100))
// 1710 GDP = GE+IV+((P-U) *WN/ML)
// 1720 RGDP = GDP*440/AGDP
// 1730 IP = ((GE+IV)/CN *.1 + (WN/WO) /100) * 100
// 1740 IV = (CN*67)/(IP*IP)
// 1750 WO = WN
// 1760 Z = Z+1: IF Z>4 THEN LET Z=1 LET Y= Y+1
// 1770 RETURN
// 1780 ' ****************************************
// 1790 ' * COMPROVA O DEFICIT
// 1800 ' ****************************************
// 1810 IF BD > -1000 THEN RETURN
// 1820 GAME = 1
// 1830 FLAG = 1
// 1840 RETURN
// 1850 ' ****************************************
// 1860 ' * COMPROVA O NIVEL DE VIDA
// 1870 ' ****************************************
// 1880 IF Y<.75 THEN RETURN
// 1890 IF INT ((2*((RGDP/AGDP)*100) -100)/3) > —15 THEN RETURN
// 1900 GAME = 1
// 1910 FLAG = 2
// 1920 RETURN
// 1930 ' ****************************************
// 1940 ' * COMPROVA A INFLACAO
// 1950 ' ****************************************
// 1960 IF IP<15 THEN RETURN
// 1970 GAME = 1
// 1980 FLAG = 3
// 1990 RETURN
// 2000 ' ****************************************
// 2010 ' * COMPROVA O DESEMPREGO
// 2020 ' ****************************************
// 2030 IF INT(U*100/P)<15 THEN RETURN
// 2040 GAME = 1
// 2050 FLAG = 4
// 2060 RETURN
// 2070 ' ****************************************
// 2080 ' * FINALIZA A SIMULACAO
// 2090 ' ****************************************
// 2100 CLS
// 2110 PRINT "Presidente "A$":"
// 2120 PRINT:PRINT"A linha economica de sua administracao"
// 2130 PRINT "levou o nosso pais a um inaceitavel"
// 2140 IF FLAG = 1 THEN PRINT "deficit de balanca."
// 2150 IF FLAG = 2 THEN PRINT "nivel de vida."
// 2160 IF FLAG = 3 THEN PRINT "indice inflacionario."
// 2170 IF FLAG = 4 THEN PRINT "numero de desempregados"
// 2180 PRINT: PRINT " A falta de eficacia na sua administra-"
// 2190 PRINT "cao e tal que foi solicitada a sua re-"
// 2200 PRINT "nuncia...": PRINT
// 2210 FOR H=1 TO 1000: NEXT H
// 2220 PRINT " O vice-presidente passa a ocupar o seu"
// 2230 PRINT "cargo.": PRINT
// 2240 LOCATE 15,23: PRINT "TECLE ALGO."
// 2250 IF INKEY$ = "" THEN 2250
// 2260 CLS: PRINT "RELATORIO DE RENUNCIA:": PRINT
// 2270 PRINT "" A$ " foi presidente por " Y+(Z*.25)" anos.": PRINT
// 2280 PRINT " Durante o seu mandato a populacao": PRINT
// 2290 PRINT "cresceu "P-3*ML" pessoas.": PRINT
// 2300 PRINT "A taxa de desemprego passou a ser ";: PRINT INT(UX1DOD/P/10)"%.": PRINT
// 2310 PRINT " A taxa de inflacao era de" INT(10*IP)/10"%.":PRINT
// 2320 PRINT "O balanco passou a ser $M ";: PRINT INT(10*BD)/10 "."
// 2330 LOCATE 0,21: STOP



// 2580 ' * VERIFICA SE ACABOU A SIMULACAO
// 2590 IF GAME=1 THEN CLS: GOTO 2070
// 2600 RETURN
// 2610 ' * APAGA PARTE INFERIOR DA TELA
// 2620 FOR H=1 TO 2
// 2630 LOCATE 0,20+H: PRINT "                                   "
// 2640 NEXT H
// 2650 LOCATE 0,21
// 2660 RETURN
