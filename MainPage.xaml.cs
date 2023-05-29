using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TheAsker.Models;

namespace TheAsker;

public partial class MainPage : ContentPage
{
    string currentAnswer;
    int attempts = 0;
    int points = 0;
    int numberQuestions = 0;
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
        setInterface(false);
    }

    private void ButtonNewGame_Clicked(object sender, EventArgs e)
    {
        newGame();
    }

    private void newGame() {
        randomNumberGenerator = new RandomNumberGenerator();
        points = 0;
        attempts = 0;
        numberQuestions = 0;
        preguntasRespuestasArchivosInGame = preguntasRespuestasArchivos;
        setInterface(true);
        selectAndShowRandomQuestion();
    }

    private void setInterface(bool startGame) {

        if (!startGame)
        {
            lblImgText.IsVisible = true;
            lblImgText.Text = "Para jugar, presione 'Nuevo juego'";
            ButtonSendAnswer.IsVisible = false;
            entryAnswer.IsVisible = false;
            imgQuestion.IsVisible = false;
        } else
        {
            lblImgText.IsVisible = false;
            lblImgText.Text = "";
            ButtonSendAnswer.IsVisible = true;
            entryAnswer.IsVisible = true;
            imgQuestion.IsVisible = true;
        }
    }

    private void selectAndShowRandomQuestion() {
        int totalElementsInArray = preguntasRespuestasArchivosInGame.Length;
        numberQuestions ++;
        attempts = 0;

        if (totalElementsInArray>0)
        {
            int randomIndex = randomNumberGenerator.getRandomNumber(totalElementsInArray - 1);
            lblQuestion.Text = preguntasRespuestasArchivosInGame[randomIndex][0];
            currentAnswer = preguntasRespuestasArchivosInGame[randomIndex][1];
            imgQuestion.Source = preguntasRespuestasArchivosInGame[randomIndex][2];
        }

        updateLabelPointsAndQuestions();
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
            case "win":
                message = "¡GANASTE!";
                textColor = Microsoft.Maui.Graphics.Color.FromArgb("#009e05");
                endGame();
                lblImgText.Text = "¡Felicidades!";
                break;
            case "lose":
                message = "¡PERDISTE!";
                textColor = Microsoft.Maui.Graphics.Color.FromArgb("#e60000");
                endGame();
                lblImgText.Text = "¡Perdiste!";
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

    private void endGame()
    {
        entryAnswer.IsVisible = false;
        ButtonSendAnswer.IsVisible = false;
        lblImgText.IsVisible = true;
        imgQuestion.IsVisible = false;
        lblQuestion.Text = "";
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
            attempts = 0;
            points++;
            await showMessage("correct");

            if (await canContinueGame())
            {
                selectAndShowRandomQuestion();
            }
        }
        else
        {
            attempts++;
            await showMessage("wrong");
            if (attempts >= 2)
            {
                if (await canContinueGame())
                {
                    selectAndShowRandomQuestion();
                }
            }
        }
    }

    private void updateLabelPointsAndQuestions()
    {
        lblPoints.Text = $"Puntos: {points.ToString()}";
        lblNumberQuestion.Text = $"Pregunta No. {numberQuestions.ToString()}";
    }

    async private Task<bool> canContinueGame()
    {
        entryAnswer.Text = "";

        if (numberQuestions >= 5 && points < 3)
        {
            await showMessage("lose");
            return false;
        } else if (numberQuestions >= 5 && points > 3)
        {
            await showMessage("win");
            return false;
        }

        return true;
    }

    private void ButtonSendAnswer_Clicked(object sender, EventArgs e)
    {
        checkUserAnswer();
    }
}

