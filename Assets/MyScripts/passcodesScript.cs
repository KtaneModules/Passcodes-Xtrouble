using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KModkit;
using System.Text.RegularExpressions;

public class passcodesScript : MonoBehaviour {

    public KMAudio Audio;
    public KMBombInfo bomb;
    public KMColorblindMode Colorblind;

    public KMSelectable[] buttons;
    public TextMesh[] buttonsText;

    public GameObject backBoard;

    private int answerGuess;

    private bool correctPlayer = true;

    private int Button0is = 10;
    private int Button1is = 10;
    private int Button2is = 10;
    private int Button3is = 10;
    private int Button4is = 10;
    private int Button5is = 10;
    private int Button6is = 10;
    private int Button7is = 10;
    private int Button8is = 10;
    private int Button9is = 10;
    private int TempDigit = -1;

    public Material backboardStart;
    public Material backboardActivate;
    public Material backboardOption;
    public Material[] ledOptions;
    public Renderer[] leds;
    public Light[] ledLights;
    public Color[] lightColors;
    public TextMesh[] cbTexts;

    private int FivesAndSevens = 0;
    private bool IsThereVowel = false;
    private int secondDigit = 0;
    private TimeSpan timeInSeconds;
    private int decasecondOnClock;
    private float tempDecasecond;
    private int numberOfRelevantInd = 0;

    private IEnumerable<int> serialNumbers;
    private IEnumerable<string> litIndicators;

    private string tempText;

    private string[] colorNames = new string[] { "Blue", "Red", "Yellow", "Green" };
    private string[] nmbrLttrs0;
    private string[] nmbrLttrs1;
    private string[] nmbrLttrs2;
    private string[] nmbrLttrs3;
    private string[] nmbrLttrs4;
    private string[] nmbrLttrs5;
    private string[] nmbrLttrs6;
    private string[] nmbrLttrs7;
    private string[] nmbrLttrs8;
    private string[] nmbrLttrs9;

    private string nmbrLttr0;
    private string nmbrLttr1;
    private string nmbrLttr2;
    private string nmbrLttr3;
    private string nmbrLttr4;
    private string nmbrLttr5;
    private string nmbrLttr6;
    private string nmbrLttr7;
    private string nmbrLttr8;
    private string nmbrLttr9;

    private int[] correctAnswers4 = new int[4];
    private int[] correctAnswers6 = new int[6];

    private int tempDigit3for4;

    private int passcodeType = 2;

    private int currentDigit = 6;
    private int maxDigit = 0;

    private bool colorblindActive;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { ButtonPress(pressedButton); return false; };
        }
        backBoard.GetComponent<MeshRenderer>().material = backboardStart;
        ledLights[0].enabled = false;
        ledLights[1].enabled = false;
        ledLights[2].enabled = false;
        ledLights[3].enabled = false;
        ledLights[4].enabled = false;
        ledLights[5].enabled = false;
        GetComponent<KMBombModule>().OnActivate += Activate;
    }

    void Start()
    {
        PickType();
        ResetLights();
        DetermineListOfUse();
        GetFivesAndSevens();
        SecondDigitGet();
        getRelevantInd();
        PickNumbersPerButton();
        PickLetterOfButton();
        getDecasecondOnClock();
        GetCorrectAnswers();
        LogCorrectAnswers();
    }

    void Activate()
    {
        if (Colorblind.ColorblindModeActive)
        {
            colorblindActive = true;
            for (int i = 0; i < cbTexts.Length; i++)
            {
                cbTexts[i].text = colorNames[Array.IndexOf(lightColors, ledLights[i].color)].ElementAt(0).ToString();
            }
        }
        backBoard.GetComponent<MeshRenderer>().material = backboardActivate;
        ledLights[0].enabled = true;
        ledLights[1].enabled = true;
        ledLights[2].enabled = true;
        ledLights[3].enabled = true;
        ledLights[4].enabled = true;
        ledLights[5].enabled = true;
    }

    void ResetValues()
    {
        answerGuess = 0;

        correctPlayer = true;

        Button0is = 10;
        Button1is = 10;
        Button2is = 10;
        Button3is = 10;
        Button4is = 10;
        Button5is = 10;
        Button6is = 10;
        Button7is = 10;
        Button8is = 10;
        Button9is = 10;
        TempDigit = -1;

        FivesAndSevens = 0;
        IsThereVowel = false;
        secondDigit = 0;
        decasecondOnClock = 0;
        tempDecasecond = 0;
        numberOfRelevantInd = 0;

        Array.Clear(nmbrLttrs0, 0, nmbrLttrs0.Length);
        Array.Clear(nmbrLttrs1, 0, nmbrLttrs1.Length);
        Array.Clear(nmbrLttrs2, 0, nmbrLttrs2.Length);
        Array.Clear(nmbrLttrs3, 0, nmbrLttrs3.Length);
        Array.Clear(nmbrLttrs4, 0, nmbrLttrs4.Length);
        Array.Clear(nmbrLttrs5, 0, nmbrLttrs5.Length);
        Array.Clear(nmbrLttrs6, 0, nmbrLttrs6.Length);
        Array.Clear(nmbrLttrs7, 0, nmbrLttrs7.Length);
        Array.Clear(nmbrLttrs8, 0, nmbrLttrs8.Length);
        Array.Clear(nmbrLttrs9, 0, nmbrLttrs9.Length);

        nmbrLttr0 = "";
        nmbrLttr1 = "";
        nmbrLttr2 = "";
        nmbrLttr3 = "";
        nmbrLttr4 = "";
        nmbrLttr5 = "";
        nmbrLttr6 = "";
        nmbrLttr7 = "";
        nmbrLttr8 = "";
        nmbrLttr9 = "";

        correctAnswers4 = new int[4];
        correctAnswers6 = new int[6];

        currentDigit = 6;
        maxDigit = 0;

        tempDigit3for4 = 0;
    }

    void PickLetterOfButton()
    {
        nmbrLttr0 = nmbrLttrs0[UnityEngine.Random.Range(0, nmbrLttrs0.Length)];
        nmbrLttr1 = nmbrLttrs1[UnityEngine.Random.Range(0, nmbrLttrs1.Length)];
        nmbrLttr2 = nmbrLttrs2[UnityEngine.Random.Range(0, nmbrLttrs2.Length)];
        nmbrLttr3 = nmbrLttrs3[UnityEngine.Random.Range(0, nmbrLttrs3.Length)];
        nmbrLttr4 = nmbrLttrs4[UnityEngine.Random.Range(0, nmbrLttrs4.Length)];
        nmbrLttr5 = nmbrLttrs5[UnityEngine.Random.Range(0, nmbrLttrs5.Length)];
        nmbrLttr6 = nmbrLttrs6[UnityEngine.Random.Range(0, nmbrLttrs6.Length)];
        nmbrLttr7 = nmbrLttrs7[UnityEngine.Random.Range(0, nmbrLttrs7.Length)];
        nmbrLttr8 = nmbrLttrs8[UnityEngine.Random.Range(0, nmbrLttrs8.Length)];
        nmbrLttr9 = nmbrLttrs9[UnityEngine.Random.Range(0, nmbrLttrs9.Length)];
        buttonsText[0].text = FindButtonText(0);
        buttonsText[1].text = FindButtonText(1);
        buttonsText[2].text = FindButtonText(2);
        buttonsText[3].text = FindButtonText(3);
        buttonsText[4].text = FindButtonText(4);
        buttonsText[5].text = FindButtonText(5);
        buttonsText[6].text = FindButtonText(6);
        buttonsText[7].text = FindButtonText(7);
        buttonsText[8].text = FindButtonText(8);
        buttonsText[9].text = FindButtonText(9);
    }

    private string FindButtonText(int ButtonIs)
    {
        if (ButtonIs == 0)
        {
            if (Button0is == 0)
            {
                return nmbrLttr0;
            }
            if (Button0is == 1)
            {
                return nmbrLttr1;
            }
            if (Button0is == 2)
            {
                return nmbrLttr2;
            }
            if (Button0is == 3)
            {
                return nmbrLttr3;
            }
            if (Button0is == 4)
            {
                return nmbrLttr4;
            }
            if (Button0is == 5)
            {
                return nmbrLttr5;
            }
            if (Button0is == 6)
            {
                return nmbrLttr6;
            }
            if (Button0is == 7)
            {
                return nmbrLttr7;
            }
            if (Button0is == 8)
            {
                return nmbrLttr8;
            }
            if (Button0is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 1)
        {
            if (Button1is == 0)
            {
                return nmbrLttr0;
            }
            if (Button1is == 1)
            {
                return nmbrLttr1;
            }
            if (Button1is == 2)
            {
                return nmbrLttr2;
            }
            if (Button1is == 3)
            {
                return nmbrLttr3;
            }
            if (Button1is == 4)
            {
                return nmbrLttr4;
            }
            if (Button1is == 5)
            {
                return nmbrLttr5;
            }
            if (Button1is == 6)
            {
                return nmbrLttr6;
            }
            if (Button1is == 7)
            {
                return nmbrLttr7;
            }
            if (Button1is == 8)
            {
                return nmbrLttr8;
            }
            if (Button1is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 2)
        {
            if (Button2is == 0)
            {
                return nmbrLttr0;
            }
            if (Button2is == 1)
            {
                return nmbrLttr1;
            }
            if (Button2is == 2)
            {
                return nmbrLttr2;
            }
            if (Button2is == 3)
            {
                return nmbrLttr3;
            }
            if (Button2is == 4)
            {
                return nmbrLttr4;
            }
            if (Button2is == 5)
            {
                return nmbrLttr5;
            }
            if (Button2is == 6)
            {
                return nmbrLttr6;
            }
            if (Button2is == 7)
            {
                return nmbrLttr7;
            }
            if (Button2is == 8)
            {
                return nmbrLttr8;
            }
            if (Button2is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 3)
        {
            if (Button3is == 0)
            {
                return nmbrLttr0;
            }
            if (Button3is == 1)
            {
                return nmbrLttr1;
            }
            if (Button3is == 2)
            {
                return nmbrLttr2;
            }
            if (Button3is == 3)
            {
                return nmbrLttr3;
            }
            if (Button3is == 4)
            {
                return nmbrLttr4;
            }
            if (Button3is == 5)
            {
                return nmbrLttr5;
            }
            if (Button3is == 6)
            {
                return nmbrLttr6;
            }
            if (Button3is == 7)
            {
                return nmbrLttr7;
            }
            if (Button3is == 8)
            {
                return nmbrLttr8;
            }
            if (Button3is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 4)
        {
            if (Button4is == 0)
            {
                return nmbrLttr0;
            }
            if (Button4is == 1)
            {
                return nmbrLttr1;
            }
            if (Button4is == 2)
            {
                return nmbrLttr2;
            }
            if (Button4is == 3)
            {
                return nmbrLttr3;
            }
            if (Button4is == 4)
            {
                return nmbrLttr4;
            }
            if (Button4is == 5)
            {
                return nmbrLttr5;
            }
            if (Button4is == 6)
            {
                return nmbrLttr6;
            }
            if (Button4is == 7)
            {
                return nmbrLttr7;
            }
            if (Button4is == 8)
            {
                return nmbrLttr8;
            }
            if (Button4is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 5)
        {
            if (Button5is == 0)
            {
                return nmbrLttr0;
            }
            if (Button5is == 1)
            {
                return nmbrLttr1;
            }
            if (Button5is == 2)
            {
                return nmbrLttr2;
            }
            if (Button5is == 3)
            {
                return nmbrLttr3;
            }
            if (Button5is == 4)
            {
                return nmbrLttr4;
            }
            if (Button5is == 5)
            {
                return nmbrLttr5;
            }
            if (Button5is == 6)
            {
                return nmbrLttr6;
            }
            if (Button5is == 7)
            {
                return nmbrLttr7;
            }
            if (Button5is == 8)
            {
                return nmbrLttr8;
            }
            if (Button5is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 6)
        {
            if (Button6is == 0)
            {
                return nmbrLttr0;
            }
            if (Button6is == 1)
            {
                return nmbrLttr1;
            }
            if (Button6is == 2)
            {
                return nmbrLttr2;
            }
            if (Button6is == 3)
            {
                return nmbrLttr3;
            }
            if (Button6is == 4)
            {
                return nmbrLttr4;
            }
            if (Button6is == 5)
            {
                return nmbrLttr5;
            }
            if (Button6is == 6)
            {
                return nmbrLttr6;
            }
            if (Button6is == 7)
            {
                return nmbrLttr7;
            }
            if (Button6is == 8)
            {
                return nmbrLttr8;
            }
            if (Button6is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 7)
        {
            if (Button7is == 0)
            {
                return nmbrLttr0;
            }
            if (Button7is == 1)
            {
                return nmbrLttr1;
            }
            if (Button7is == 2)
            {
                return nmbrLttr2;
            }
            if (Button7is == 3)
            {
                return nmbrLttr3;
            }
            if (Button7is == 4)
            {
                return nmbrLttr4;
            }
            if (Button7is == 5)
            {
                return nmbrLttr5;
            }
            if (Button7is == 6)
            {
                return nmbrLttr6;
            }
            if (Button7is == 7)
            {
                return nmbrLttr7;
            }
            if (Button7is == 8)
            {
                return nmbrLttr8;
            }
            if (Button7is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 8)
        {
            if (Button8is == 0)
            {
                return nmbrLttr0;
            }
            if (Button8is == 1)
            {
                return nmbrLttr1;
            }
            if (Button8is == 2)
            {
                return nmbrLttr2;
            }
            if (Button8is == 3)
            {
                return nmbrLttr3;
            }
            if (Button8is == 4)
            {
                return nmbrLttr4;
            }
            if (Button8is == 5)
            {
                return nmbrLttr5;
            }
            if (Button8is == 6)
            {
                return nmbrLttr6;
            }
            if (Button8is == 7)
            {
                return nmbrLttr7;
            }
            if (Button8is == 8)
            {
                return nmbrLttr8;
            }
            if (Button8is == 9)
            {
                return nmbrLttr9;
            }
        }
        if (ButtonIs == 9)
        {
            if (Button9is == 0)
            {
                return nmbrLttr0;
            }
            if (Button9is == 1)
            {
                return nmbrLttr1;
            }
            if (Button9is == 2)
            {
                return nmbrLttr2;
            }
            if (Button9is == 3)
            {
                return nmbrLttr3;
            }
            if (Button9is == 4)
            {
                return nmbrLttr4;
            }
            if (Button9is == 5)
            {
                return nmbrLttr5;
            }
            if (Button9is == 6)
            {
                return nmbrLttr6;
            }
            if (Button9is == 7)
            {
                return nmbrLttr7;
            }
            if (Button9is == 8)
            {
                return nmbrLttr8;
            }
            if (Button9is == 9)
            {
                return nmbrLttr9;
            }
        }
        return "Error";
    }

    void PickNumbersPerButton()
    {
        PickNumbersButton();
        Button0is = TempDigit;
        PickNumbersButton();
        Button1is = TempDigit;
        PickNumbersButton();
        Button2is = TempDigit;
        PickNumbersButton();
        Button3is = TempDigit;
        PickNumbersButton();
        Button4is = TempDigit;
        PickNumbersButton();
        Button5is = TempDigit;
        PickNumbersButton();
        Button6is = TempDigit;
        PickNumbersButton();
        Button7is = TempDigit;
        PickNumbersButton();
        Button8is = TempDigit;
        PickNumbersButton();
        Button9is = TempDigit;
    }

    void PickNumbersButton()
    {
        try
        {
            TempDigit = UnityEngine.Random.Range(0, 10);
            if (TempDigit != Button0is && TempDigit != Button1is && TempDigit != Button2is && TempDigit != Button3is && TempDigit != Button4is && TempDigit != Button5is && TempDigit != Button6is && TempDigit != Button7is && TempDigit != Button8is && TempDigit != Button9is)
            {
                return;
            }
            PickNumbersButton();
        }
        catch
        {
            PickNumbersButton();
        }
    }

    void getRelevantInd()
    {
        if (bomb.IsIndicatorPresent(Indicator.IND))
        {
            numberOfRelevantInd += 1;
        }
        if (bomb.IsIndicatorPresent(Indicator.NSA))
        {
            numberOfRelevantInd += 1;
        }
        if (bomb.IsIndicatorPresent(Indicator.MSA))
        {
            numberOfRelevantInd += 1;
        }
        if (bomb.IsIndicatorPresent(Indicator.BOB))
        {
            numberOfRelevantInd += 1;
        }
        if (bomb.IsIndicatorPresent(Indicator.FRK))
        {
            numberOfRelevantInd += 1;
        }
    }

    void getDecasecondOnClock()
    {
        timeInSeconds = TimeSpan.FromSeconds((int)bomb.GetTime());
        decasecondOnClock = (int)((float)timeInSeconds.Seconds / (float)10);
        if ((numberOfRelevantInd == 1 || numberOfRelevantInd == 2) && decasecondOnClock != 5)
        {
            decasecondOnClock = decasecondOnClock * 2;
        }
    }

    void SecondDigitGet()
    {
        litIndicators = bomb.GetOnIndicators();
        foreach (string litIndicator in litIndicators)
        {
            foreach (char indicatorLetter in litIndicator)
            {
                if (indicatorLetter.ToString() == "A" || indicatorLetter.ToString() == "E" || indicatorLetter.ToString() == "I" || indicatorLetter.ToString() == "O" || indicatorLetter.ToString() == "U")
                {
                    IsThereVowel = true;
                }
            }
        }
        if (IsThereVowel == true)
        {
            secondDigit = bomb.GetPortCount(Port.RJ45) + 1;
            if (secondDigit > 9)
            {
                secondDigit = 8;
            }
        }
        else
        {
            secondDigit = 0;
        }
    }

    void GetFivesAndSevens()
    {
        serialNumbers = bomb.GetSerialNumberNumbers();
        foreach (int serialNumber in serialNumbers)
        {
            if (serialNumber == 5 || serialNumber == 7)
            {
                FivesAndSevens += 1;
            }
        }
    }

    void Update()
    {
        getDecasecondOnClock();
        GetCorrectAnswers();
    }

    void LogCorrectAnswers()
    {
        string[] allletters = new string[] { nmbrLttr0, nmbrLttr1, nmbrLttr2, nmbrLttr3, nmbrLttr4, nmbrLttr5, nmbrLttr6, nmbrLttr7, nmbrLttr8, nmbrLttr9 };
        string letters = "";
        if (currentDigit == 1)
        {
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 1: Number of batteries = {1}", moduleId, correctAnswers4[0]);
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 2: Lit indicator with vowel {1} = {2}{3}", moduleId, correctAnswers4[1] != 0 ? "present" : "not present", correctAnswers4[1] != 0 ? "Number of RJ-45 ports + 1 = " + bomb.GetPortCount(Port.RJ45) + " + 1 = " + (bomb.GetPortCount(Port.RJ45) + 1) : "0", bomb.GetPortCount(Port.RJ45) + 1 > 9 ? " (answer is greater than 9) = 8" : "");
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 3: Number of AA batteries + Number of PS/2 ports + 2 = {1} + {2} + 2 = {3}{4}", moduleId, bomb.GetBatteryCount(Battery.AA), bomb.GetPortCount(Port.PS2), bomb.GetBatteryCount(Battery.AA) + bomb.GetPortCount(Port.PS2) + 2, bomb.GetBatteryCount(Battery.AA) + bomb.GetPortCount(Port.PS2) + 2 > 9 ? " (answer is greater than 9) = 7" : "");
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 4: The tens digit on the bomb's timer with {1} of the indicators IND, NSA, MSA, BOB, or FRK and with the tens digit not being 5 = #{2}", moduleId, numberOfRelevantInd == 1 || numberOfRelevantInd == 2 ? "one or two" : "none or more than 2", numberOfRelevantInd == 1 || numberOfRelevantInd == 2 ? " * 2 (if the tens digit is 5 then this is just #)" : " (if the tens digit is 5 then this is still #)");
            Debug.LogFormat("[Passcodes #{0}] Passcode as Digits: {1}#", moduleId, correctAnswers4[0]+""+correctAnswers4[1]+""+correctAnswers4[2]);
            for (int i = 0; i < 3; i++)
            {
                letters += allletters[correctAnswers4[i]];
            }
            letters = letters.ToUpper();
            letters += "# (# depends on Passcode Digit 4)";
        }
        if (currentDigit == 0)
        {
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 1: 8", moduleId);
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 2: Number of 5's and 7's in the serial number = {1}", moduleId, correctAnswers6[1]);
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 3: Passcode Digit 1 - Passcode Digit 2 = {1} - {2} = {3}", moduleId, correctAnswers6[0], correctAnswers6[1], correctAnswers6[2]);
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 4: Number of AA batteries + Number of PS/2 ports + 2 - 1 = {1} + {2} + 2 - 1 = {3}{4}", moduleId, bomb.GetBatteryCount(Battery.AA), bomb.GetPortCount(Port.PS2), bomb.GetBatteryCount(Battery.AA) + bomb.GetPortCount(Port.PS2) + 2, bomb.GetBatteryCount(Battery.AA) + bomb.GetPortCount(Port.PS2) + 2 > 9 ? " (answer is greater than 9) = 7 - 1 = 6" : " - 1 = " + correctAnswers6[3]);
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 5: Lit indicator with vowel {1} = {2}{3}", moduleId, IsThereVowel ? "present" : "not present", IsThereVowel ? "Number of RJ-45 ports + 1 = " + bomb.GetPortCount(Port.RJ45) + " + 1 = " + (bomb.GetPortCount(Port.RJ45) + 1) : "0", bomb.GetPortCount(Port.RJ45) + 1 > 9 ? " (answer is greater than 9) = 8 + 1 = 9" : " + 1 = " + correctAnswers6[4]);
            Debug.LogFormat("[Passcodes #{0}] Passcode Digit 6: 0 strikes = 9 | Greater than or equal to 1 strikes = (Number of strikes + 1) * Number of strikes - 1 = #", moduleId);
            Debug.LogFormat("[Passcodes #{0}] Passcode as Digits: {1}#", moduleId, correctAnswers6[0]+""+correctAnswers6[1]+""+correctAnswers6[2]+""+ correctAnswers6[3]+""+correctAnswers6[4]);
            for (int i = 0; i < 5; i++)
            {
                letters += allletters[correctAnswers6[i]];
            }
            letters = letters.ToUpper();
            letters += "# (# depends on Passcode Digit 6)";
        }
        Debug.LogFormat("[Passcodes #{0}] The number of ports on the bomb is {1}, therefore the {2} table from the manual will be used to enter the passcode as letters", moduleId, bomb.GetPortCount() % 2 != 0 ? "not even" : "not odd", bomb.GetPortCount() % 2 != 0 ? "top" : "bottom");
        Debug.LogFormat("[Passcodes #{0}] Passcode as Letters: {1}", moduleId, letters);
    }

    void GetCorrectAnswers()
    {
        if (passcodeType == 0)
        {
            correctAnswers6[0] = 8;
            correctAnswers6[1] = FivesAndSevens;
            correctAnswers6[2] = correctAnswers6[0] - correctAnswers6[1];
            tempDigit3for4 = bomb.GetBatteryCount(Battery.AA) + bomb.GetPortCount(Port.PS2) + 2;
            if (tempDigit3for4 > 9)
            {
                tempDigit3for4 = 7;
            }
            correctAnswers6[3] = (tempDigit3for4) - 1;
            correctAnswers6[4] = secondDigit + 1;
            if (bomb.GetStrikes() == 0)
            {
                correctAnswers6[5] = 9;
            }
            else
            {
                correctAnswers6[5] = ((bomb.GetStrikes() + 1) * bomb.GetStrikes()) - 1;
                if (correctAnswers6[5] > 9)
                {
                    correctAnswers6[5] = ((correctAnswers6[5]) % 5) * 2;
                }
            }
        }
        else
        {
            correctAnswers4[0] = bomb.GetBatteryCount();
            correctAnswers4[1] = secondDigit;
            tempDigit3for4 = bomb.GetBatteryCount(Battery.AA) + bomb.GetPortCount(Port.PS2) + 2;
            if (tempDigit3for4 > 9)
            {
                tempDigit3for4 = 7;
            }
            correctAnswers4[2] = tempDigit3for4;
            correctAnswers4[3] = decasecondOnClock;
        }
    }

    void DetermineListOfUse()
    {
        if (bomb.GetPortCount() % 2 == 0)
        {
            nmbrLttrs0 = new string[] { "w", "y", "m" };
            nmbrLttrs1 = new string[] { "g", "o" };
            nmbrLttrs2 = new string[] { "e", "f", "l" };
            nmbrLttrs3 = new string[] { "j", "p", "z" };
            nmbrLttrs4 = new string[] { "c", "k" };
            nmbrLttrs5 = new string[] { "a", "b", "n" };
            nmbrLttrs6 = new string[] { "h", "x" };
            nmbrLttrs7 = new string[] { "i", "v", "u" };
            nmbrLttrs8 = new string[] { "d", "t" };
            nmbrLttrs9 = new string[] { "q", "r", "s" };
        }
        else
        {
            nmbrLttrs0 = new string[] { "a", "b" };
            nmbrLttrs1 = new string[] { "c", "k", "z" };
            nmbrLttrs2 = new string[] { "j", "p" };
            nmbrLttrs3 = new string[] { "e", "f", "l" };
            nmbrLttrs4 = new string[] { "g", "o", "m" };
            nmbrLttrs5 = new string[] { "w", "y", "s" };
            nmbrLttrs6 = new string[] { "q", "r" };
            nmbrLttrs7 = new string[] { "d", "t", "u" };
            nmbrLttrs8 = new string[] { "i", "v", "n" };
            nmbrLttrs9 = new string[] { "h", "x" };
        }
    }

    void ResetLights()
    {
        if (passcodeType == 1)
        {
            currentDigit = 1;
            maxDigit = 4;
            leds[0].material = ledOptions[2];
            leds[1].material = ledOptions[1];
            leds[2].material = ledOptions[1];
            leds[3].material = ledOptions[1];
            leds[4].material = ledOptions[1];
            leds[5].material = ledOptions[2];
            ledLights[0].color = lightColors[2];
            ledLights[1].color = lightColors[1];
            ledLights[2].color = lightColors[1];
            ledLights[3].color = lightColors[1];
            ledLights[4].color = lightColors[1];
            ledLights[5].color = lightColors[2];
        }
        else
        {
            currentDigit = 0;
            maxDigit = 5;
            leds[0].material = ledOptions[1];
            leds[1].material = ledOptions[1];
            leds[2].material = ledOptions[1];
            leds[3].material = ledOptions[1];
            leds[4].material = ledOptions[1];
            leds[5].material = ledOptions[1];
            ledLights[0].color = lightColors[1];
            ledLights[1].color = lightColors[1];
            ledLights[2].color = lightColors[1];
            ledLights[3].color = lightColors[1];
            ledLights[4].color = lightColors[1];
            ledLights[5].color = lightColors[1];
        }
        if (colorblindActive)
        {
            for (int i = 0; i < cbTexts.Length; i++)
            {
                cbTexts[i].text = colorNames[Array.IndexOf(lightColors, ledLights[i].color)].ElementAt(0).ToString();
            }
        }
    }

    void PickType()
    {
        if (passcodeType == 2)
        {
            passcodeType = UnityEngine.Random.Range(0, 2);
        }
        else if (passcodeType == 1)
        {
            passcodeType = 0;
        }
        else
        {
            passcodeType = 1;
        }
        if (passcodeType == 1)
        {
            Debug.LogFormat("[Passcodes #{0}] Selected Type: 4-Digit Passcode", moduleId);
        }
        else
        {
            Debug.LogFormat("[Passcodes #{0}] Selected Type: 6-Digit Passcode", moduleId);
        }
    }

    void ButtonPress(KMSelectable button)
    {
        if (!moduleSolved)
        {
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, button.transform);
            button.AddInteractionPunch();
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonRelease, button.transform);
            correctPlayer = CheckIfAnswerCorrect(passcodeType, currentDigit, button);
            if (correctPlayer && (maxDigit > currentDigit))
            {
                currentDigit += 1;
                ledLights[currentDigit - 1].color = lightColors[0];
                if (colorblindActive)
                {
                    cbTexts[currentDigit - 1].text = colorNames[Array.IndexOf(lightColors, ledLights[currentDigit - 1].color)].ElementAt(0).ToString();
                }
                leds[currentDigit - 1].material = ledOptions[0];
            }
            else if (correctPlayer && (maxDigit == currentDigit))
            {
                moduleSolved = true;
                GetComponent<KMBombModule>().HandlePass();
                leds[0].material = ledOptions[3];
                leds[1].material = ledOptions[3];
                leds[2].material = ledOptions[3];
                leds[3].material = ledOptions[3];
                leds[4].material = ledOptions[3];
                leds[5].material = ledOptions[3];
                ledLights[0].color = lightColors[3];
                ledLights[1].color = lightColors[3];
                ledLights[2].color = lightColors[3];
                ledLights[3].color = lightColors[3];
                ledLights[4].color = lightColors[3];
                ledLights[5].color = lightColors[3];
                if (colorblindActive)
                {
                    for (int i = 0; i < cbTexts.Length; i++)
                    {
                        cbTexts[i].text = colorNames[Array.IndexOf(lightColors, ledLights[i].color)].ElementAt(0).ToString();
                    }
                }
                backBoard.GetComponent<MeshRenderer>().material = backboardOption;
            }
            else
            {
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.LightBuzzShort, transform);
                button.AddInteractionPunch(4f);
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.LightBuzz, transform);
                ResetValues();
                GetComponent<KMBombModule>().HandleStrike();
                Start();
                return;
            }
        }
        else
        {
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, button.transform);
            button.AddInteractionPunch(0.25f);
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, button.transform);
        }
    }

    private bool CheckIfAnswerCorrect(int type, int stage, KMSelectable buttonGuess)
    {
        if (type == 1)
        {
            tempText = buttonGuess.GetComponentsInChildren<TextMesh>()[0].text;
            if (bomb.GetPortCount() % 2 == 0)
            {
                if (tempText == "w" || tempText == "y" || tempText == "m")
                {
                    answerGuess = 0;
                }
                else if (tempText == "q" || tempText == "r" || tempText == "s")
                {
                    answerGuess = 9;
                }
                else if (tempText == "d" || tempText == "t")
                {
                    answerGuess = 8;
                }
                else if (tempText == "i" || tempText == "v" || tempText == "u")
                {
                    answerGuess = 7;
                }
                else if (tempText == "h" || tempText == "x")
                {
                    answerGuess = 6;
                }
                else if (tempText == "a" || tempText == "b" || tempText == "n")
                {
                    answerGuess = 5;
                }
                else if (tempText == "c" || tempText == "k")
                {
                    answerGuess = 4;
                }
                else if (tempText == "j" || tempText == "p" || tempText == "z")
                {
                    answerGuess = 3;
                }
                else if (tempText == "e" || tempText == "f" || tempText == "l")
                {
                    answerGuess = 2;
                }
                else if (tempText == "g" || tempText == "o")
                {
                    answerGuess = 1;
                }
            }
            else
            {
                if (tempText == "a" || tempText == "b")
                {
                    answerGuess = 0;
                }
                else if (tempText == "h" || tempText == "x")
                {
                    answerGuess = 9;
                }
                else if (tempText == "i" || tempText == "v" || tempText == "n")
                {
                    answerGuess = 8;
                }
                else if (tempText == "d" || tempText == "t" || tempText == "u")
                {
                    answerGuess = 7;
                }
                else if (tempText == "q" || tempText == "r")
                {
                    answerGuess = 6;
                }
                else if (tempText == "w" || tempText == "y" || tempText == "s")
                {
                    answerGuess = 5;
                }
                else if (tempText == "g" || tempText == "o" || tempText == "m")
                {
                    answerGuess = 4;
                }
                else if (tempText == "e" || tempText == "f" || tempText == "l")
                {
                    answerGuess = 3;
                }
                else if (tempText == "j" || tempText == "p")
                {
                    answerGuess = 2;
                }
                else if (tempText == "c" || tempText == "k" || tempText == "z")
                {
                    answerGuess = 1;
                }
            }

            if (correctAnswers4[stage-1] == answerGuess)
            {
                Debug.LogFormat("[Passcodes #{0}] Answered {1} which means {2} which is correct!", moduleId, tempText.ToUpper(), answerGuess);
                return true;
            }
        }
        if (type == 0)
        {
            tempText = buttonGuess.GetComponentsInChildren<TextMesh>()[0].text;
            if (bomb.GetPortCount() % 2 == 0)
            {
                if (tempText == "w" || tempText == "y" || tempText == "m")
                {
                    answerGuess = 0;
                }
                else if (tempText == "q" || tempText == "r" || tempText == "s")
                {
                    answerGuess = 9;
                }
                else if (tempText == "d" || tempText == "t")
                {
                    answerGuess = 8;
                }
                else if (tempText == "i" || tempText == "v" || tempText == "u")
                {
                    answerGuess = 7;
                }
                else if (tempText == "h" || tempText == "x")
                {
                    answerGuess = 6;
                }
                else if (tempText == "a" || tempText == "b" || tempText == "n")
                {
                    answerGuess = 5;
                }
                else if (tempText == "c" || tempText == "k")
                {
                    answerGuess = 4;
                }
                else if (tempText == "j" || tempText == "p" || tempText == "z")
                {
                    answerGuess = 3;
                }
                else if (tempText == "e" || tempText == "f" || tempText == "l")
                {
                    answerGuess = 2;
                }
                else if (tempText == "g" || tempText == "o")
                {
                    answerGuess = 1;
                }
            }
            else
            {
                if (tempText == "a" || tempText == "b")
                {
                    answerGuess = 0;
                }
                else if (tempText == "h" || tempText == "x")
                {
                    answerGuess = 9;
                }
                else if (tempText == "i" || tempText == "v" || tempText == "n")
                {
                    answerGuess = 8;
                }
                else if (tempText == "d" || tempText == "t" || tempText == "u")
                {
                    answerGuess = 7;
                }
                else if (tempText == "q" || tempText == "r")
                {
                    answerGuess = 6;
                }
                else if (tempText == "w" || tempText == "y" || tempText == "s")
                {
                    answerGuess = 5;
                }
                else if (tempText == "g" || tempText == "o" || tempText == "m")
                {
                    answerGuess = 4;
                }
                else if (tempText == "e" || tempText == "f" || tempText == "l")
                {
                    answerGuess = 3;
                }
                else if (tempText == "j" || tempText == "p")
                {
                    answerGuess = 2;
                }
                else if (tempText == "c" || tempText == "k" || tempText == "z")
                {
                    answerGuess = 1;
                }
            }

            if (correctAnswers6[stage] == answerGuess)
            {
                Debug.LogFormat("[Passcodes #{0}] Answered {1} which means {2} which is correct!", moduleId, tempText.ToUpper(), answerGuess);
                return true;
            }
        }
        if (type == 1)
        {
            Debug.LogFormat("[Passcodes #{0}] Answered {1} which means {2} which is INCORRECT!!! Correct answer was {3}.", moduleId, tempText.ToUpper(), answerGuess, correctAnswers4[stage-1]);
        }
        if (type == 0)
        {
            Debug.LogFormat("[Passcodes #{0}] Answered {1} which means {2} which is INCORRECT!!! Correct answer was {3}.", moduleId, tempText.ToUpper(), answerGuess, correctAnswers6[stage]);
        }
        return false;
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press ABC [Presses the buttons with the labels 'A', 'B', and 'C'] | !{0} press A at # [Presses the button labeled 'A' when the bomb's tens digit is '#'] | !{0} colorblind [Toggles colorblind mode]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*colorblind\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (colorblindActive)
            {
                colorblindActive = false;
                for (int i = 0; i < cbTexts.Length; i++)
                {
                    cbTexts[i].text = "";
                }
            }
            else
            {
                colorblindActive = true;
                for (int i = 0; i < cbTexts.Length; i++)
                {
                    cbTexts[i].text = colorNames[Array.IndexOf(lightColors, ledLights[i].color)].ElementAt(0).ToString();
                }
            }
            yield break;
        }
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length > 4)
            {
                yield return "sendtochaterror Too many parameters!";
            }
            else if (parameters.Length == 4)
            {
                string[] alpha = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                List<string> valids = new List<string>();
                for (int i = 1; i < 10; i++)
                {
                    valids.Add(FindButtonText(i));
                }
                valids.Add(FindButtonText(0));
                for (int i = 0; i < parameters[1].Length; i++)
                {
                    if (!alpha.Contains((parameters[1].ElementAt(i) + "").ToLower()))
                    {
                        yield return "sendtochaterror The specified label to press '" + parameters[1].ElementAt(i) + "' is invalid!";
                        yield break;
                    }
                }
                for (int i = 0; i < parameters[1].Length; i++)
                {
                    if (!valids.Contains((parameters[1].ElementAt(i) + "").ToLower()))
                    {
                        yield return "sendtochaterror The specified label to press '" + parameters[1].ElementAt(i) + "' is not an option on the module!";
                        yield break;
                    }
                }
                int temp = 0;
                if (int.TryParse(parameters[3], out temp))
                {
                    if (temp < 0 || temp > 5)
                    {
                        yield return "sendtochaterror The specified tens digit to press the button(s) at '" + parameters[3] + "' is out of range 0-5!";
                        yield break;
                    }
                    if ((int)bomb.GetTime() % 60 / 10 < temp && 59 - (temp * 10) + 9 + ((int)bomb.GetTime() % 60) > 15 || (int)bomb.GetTime() % 60 / 10 > temp && (int)bomb.GetTime() % 60 - ((temp * 10) + 9) > 15)
                    {
                        yield return "waiting music";
                    }
                    while (temp != (int)bomb.GetTime() % 60 / 10) { yield return "trycancel Stopped waiting for button press due to a request to cancel!"; }
                    yield return "end waiting music";
                    for (int i = 0; i < parameters[1].Length; i++)
                    {
                        buttons[valids.IndexOf((parameters[1].ElementAt(i) + "").ToLower())].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    yield return "sendtochaterror The specified tens digit to press the button(s) at '" + parameters[3] + "' is invalid!";
                }
            }
            else if (parameters.Length == 3)
            {
                if (parameters[2].EqualsIgnoreCase("at"))
                    yield return "sendtochaterror Please specify the tens digit in the bomb's timer to press the button(s) at!";
                else
                    yield return "sendtochaterror Unrecognized parameter '"+parameters[2]+"', expected 'at'!";
            }
            else if (parameters.Length == 2)
            {
                string[] alpha = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                List<string> valids = new List<string>();
                for (int i = 1; i < 10; i++)
                {
                    valids.Add(FindButtonText(i));
                }
                valids.Add(FindButtonText(0));
                for (int i = 0; i < parameters[1].Length; i++)
                {
                    if (!alpha.Contains((parameters[1].ElementAt(i) + "").ToLower()))
                    {
                        yield return "sendtochaterror The specified label to press '" + parameters[1].ElementAt(i) + "' is invalid!";
                        yield break;
                    }
                }
                for (int i = 0; i < parameters[1].Length; i++)
                {
                    if (!valids.Contains((parameters[1].ElementAt(i) + "").ToLower()))
                    {
                        yield return "sendtochaterror The specified label to press '" + parameters[1].ElementAt(i) + "' is not an option on the module!";
                        yield break;
                    }
                }
                for (int i = 0; i < parameters[1].Length; i++)
                {
                    buttons[valids.IndexOf((parameters[1].ElementAt(i) + "").ToLower())].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (parameters.Length == 1)
            {
                yield return "sendtochaterror Please specify the button(s) you wish to press!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        int start = 0;
        int end = 0;
        if (passcodeType == 1)
        {
            start = currentDigit - 1;
            end = 4;
        }
        else
        {
            start = currentDigit;
            end = 6;
        }
        string[] modletters = new string[] { nmbrLttr0, nmbrLttr1, nmbrLttr2, nmbrLttr3, nmbrLttr4, nmbrLttr5, nmbrLttr6, nmbrLttr7, nmbrLttr8, nmbrLttr9 };
        for (int i = start; i < end; i++)
        {
            string letter = "";
            if (passcodeType == 1)
            {
                letter = modletters[correctAnswers4[i]];
            }
            else
            {
                letter = modletters[correctAnswers6[i]];
            }
            int[] positions = new int[] { 9, 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int j = 0; j < 10; j++)
            {
                if (buttonsText[j].text.Equals(letter))
                {
                    buttons[positions[j]].OnInteract();
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
