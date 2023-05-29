using System;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TheAsker.Models;

namespace TheAsker;

public partial class MainPage : ContentPage
{
    string currentAnswer;
    RandomNumberGenerator randomNumberGenerator;

    string[][] preguntasRespuestasArchivosInGame;
    string[][] preguntasRespuestasArchivos = new string[][]
    {
        new string[]
        {
            "¿A qué distribución de GNU/Linux pertenece esta imagen?",
            "Ubuntu",
            "img0.jpg"
        },
        new string[]
        {
            "¿De qué distribución de GNU/Linux proviene Ubuntu?",
            "Debian",
            "img1.jpg"
        },
        new string[]
        {
            "¿Quién es el creador de Linux?",
            "Linus Torvalds",
            "img2.jpg"
        },
        new string[]
        {
            "¿De qué sistema operativo proviene Linux?",
            "Unix",
            "img3.jpg"
        },
        new string[]
        {
            "¿Cómo se llama la terminal en Linux?",
            "Bash",
            "img4.jpg"
        },new string[]
        {
            "¿A qué distribución de GNU/Linux pertenece esta imagen?",
            "Debian",
            "img5.jpg"
        },
        new string[]
        {
            "¿Qué animal es la mascota de Linux?",
            "Pingüino",
            "img6.jpg"
        },
        new string[]
        {
            "Linux en sí es un ...... o sea, es el núcleo.",
            "Kernel",
            "img7.jpg"
        },
        new string[]
        {
            "¿A qué distribución de GNU/Linux pertenece esta imagen?",
            "Mint",
            "img8.jpg"
        },
        new string[]
        {
            "¿A qué distribución de GNU/Linux pertenece esta imagen?",
            "Arch",
            "img9.jpg"
        }
    };


    public MainPage()
	{
		InitializeComponent();
	}

    private void ButtonNewGame_Clicked(object sender, EventArgs e)
    {
        newGame();
    }

    private void newGame() {
        randomNumberGenerator = new RandomNumberGenerator();
        preguntasRespuestasArchivosInGame = preguntasRespuestasArchivos;
        selectAndShowRandomQuestion();
    }

    private void selectAndShowRandomQuestion() {
        int totalElementsInArray = preguntasRespuestasArchivosInGame.Length;

        if (totalElementsInArray>0)
        {
            int randomIndex = randomNumberGenerator.getRandomNumber(totalElementsInArray - 1);
            lblQuestion.Text = preguntasRespuestasArchivosInGame[randomIndex][0];
            currentAnswer = preguntasRespuestasArchivosInGame[randomIndex][1];
            imgQuestion.Source = preguntasRespuestasArchivosInGame[randomIndex][2];
        }
    }

    private void entryAnswer_Completed(object sender, EventArgs e)
    {
        checkUserAnswer();
    }

    async private Task showMessage(string type) {
        string message;
        Microsoft.Maui.Graphics.Color textColor;

        switch (type) {
            case "correct":
                message = "¡CORRECTO!";
                textColor = Microsoft.Maui.Graphics.Color.FromArgb("#009e05");
                break;
            case "wrong":
                message = "¡INCORRECTO!";
                textColor = Microsoft.Maui.Graphics.Color.FromArgb("#e60000");
                break;
            default:
                message = "";
                textColor = Microsoft.Maui.Graphics.Color.FromArgb("");
                break;
        }

        lblResult.Text = message;
        lblResult.TextColor = textColor;
        await Task.Delay(1000);
        lblResult.Text = "";
    }

    async private void checkUserAnswer()
    {
        string userAnswer;
        string patron = "\\b" + currentAnswer + "\\b";

        if (string.IsNullOrWhiteSpace(entryAnswer.Text)) {
            return;
        }

        userAnswer = (entryAnswer.Text).Trim();
        Regex regex = new Regex(patron, RegexOptions.IgnoreCase);

        if (regex.IsMatch(userAnswer))
        {
            await showMessage("correct");
            selectAndShowRandomQuestion();
        }
        else
        {
            await showMessage("wrong");
        }
    }

    private void ButtonSendAnswer_Clicked(object sender, EventArgs e)
    {
        checkUserAnswer();
    }
}

